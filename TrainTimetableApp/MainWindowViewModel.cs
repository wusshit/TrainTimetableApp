// MainWindowViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Linq; // Required for IndexOf

namespace TrainTimetableApp
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        // --- INotifyPropertyChanged Implementation ---
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            // Trigger dependent property changes or command updates if necessary
            if (propertyName == nameof(IsEditing)) { OnPropertyChanged(nameof(AddButtonContent)); }
            return true;
        }

        // --- Properties for Data Binding ---
        public ObservableCollection<TrainScheduleEntry> TrainSchedules { get; set; }

        private TrainScheduleEntry? _selectedScheduleEntry;
        public TrainScheduleEntry? SelectedScheduleEntry
        {
            get => _selectedScheduleEntry;
            set
            {
                // Store previous editing state check
                bool wasEditing = IsEditing;
                // Set the field (this will trigger PropertyChanged for SelectedScheduleEntry)
                SetField(ref _selectedScheduleEntry, value);

                // Update editing state and load/clear form AFTER setting the field
                if (_selectedScheduleEntry != null)
                {
                    LoadEntryForEditing(_selectedScheduleEntry);
                    IsEditing = true; // Enter edit mode
                }
                else
                {
                    IsEditing = false; // Exit edit mode if null
                                       // Only clear if we were actually editing OR selection is manually cleared
                    if (wasEditing || value == null)
                    {
                        ClearNewEntryFields();
                    }
                }

                // Explicitly trigger CanExecuteChanged for commands that depend on selection or edit state
                (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (AddOrUpdateCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (CancelEditCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        // --- Properties for the Add/Edit Form ---
        // Use SetField to automatically raise PropertyChanged
        private string _newTrainNumber = string.Empty;
        public string NewTrainNumber { get => _newTrainNumber; set { if (SetField(ref _newTrainNumber, value)) (AddOrUpdateCommand as RelayCommand)?.RaiseCanExecuteChanged(); } }

        private string _newOrigin = string.Empty;
        public string NewOrigin { get => _newOrigin; set { if (SetField(ref _newOrigin, value)) (AddOrUpdateCommand as RelayCommand)?.RaiseCanExecuteChanged(); } }

        private string _newDestination = string.Empty;
        public string NewDestination { get => _newDestination; set { if (SetField(ref _newDestination, value)) (AddOrUpdateCommand as RelayCommand)?.RaiseCanExecuteChanged(); } }

        private DateTime _newDepartureTime;
        public DateTime NewDepartureTime { get => _newDepartureTime; set => SetField(ref _newDepartureTime, value); }

        private DateTime _newArrivalTime;
        public DateTime NewArrivalTime { get => _newArrivalTime; set => SetField(ref _newArrivalTime, value); }

        private string? _newPlatform;
        public string? NewPlatform { get => _newPlatform; set => SetField(ref _newPlatform, value); }

        private string _newStatus = "On Time";
        public string NewStatus { get => _newStatus; set => SetField(ref _newStatus, value); }


        // --- Editing State ---
        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            private set => SetField(ref _isEditing, value); // Setter uses SetField now
        }

        // Property for the Add/Update button's text
        public string AddButtonContent => IsEditing ? "Update Entry" : "Add Entry";


        // --- Commands ---
        public ICommand AddOrUpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CancelEditCommand { get; }


        // --- Constructor ---
        public MainWindowViewModel()
        {
            TrainSchedules = new ObservableCollection<TrainScheduleEntry>();
            LoadSampleData();

            // Create commands and link them to methods
            AddOrUpdateCommand = new RelayCommand(ExecuteAddOrUpdateCommand, CanExecuteAddOrUpdateCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteCommand);
            CancelEditCommand = new RelayCommand(ExecuteCancelEditCommand, CanExecuteCancelEditCommand);

            // Initialize time fields
            ResetTimeFields();
        }


        // --- Command Methods ---

        private bool CanExecuteAddOrUpdateCommand(object? parameter)
        {
            // Basic validation: Ensure required fields are not empty
            return !string.IsNullOrWhiteSpace(NewTrainNumber) &&
                   !string.IsNullOrWhiteSpace(NewOrigin) &&
                   !string.IsNullOrWhiteSpace(NewDestination);
        }

        private void ExecuteAddOrUpdateCommand(object? parameter)
        {
            if (IsEditing && SelectedScheduleEntry != null) // Update existing entry
            {
                // Capture index before potentially changing selection
                int index = TrainSchedules.IndexOf(SelectedScheduleEntry);

                // Copy data from form fields back to the selected object
                SelectedScheduleEntry.TrainNumber = NewTrainNumber;
                SelectedScheduleEntry.Origin = NewOrigin;
                SelectedScheduleEntry.Destination = NewDestination;
                SelectedScheduleEntry.DepartureTime = NewDepartureTime;
                SelectedScheduleEntry.ArrivalTime = NewArrivalTime;
                SelectedScheduleEntry.Platform = NewPlatform;
                SelectedScheduleEntry.Status = NewStatus;

                // Force the DataGrid to refresh the updated item visually.
                if (index != -1)
                {
                    TrainSchedules.RefreshItem(index); // Use helper method
                }

                // Clear selection AFTER update & refresh. This will trigger form clearing via setter.
                SelectedScheduleEntry = null;
            }
            else if (!IsEditing) // Add new entry
            {
                var newEntry = new TrainScheduleEntry
                {
                    TrainNumber = NewTrainNumber,
                    Origin = NewOrigin,
                    Destination = NewDestination,
                    DepartureTime = NewDepartureTime,
                    ArrivalTime = NewArrivalTime,
                    Platform = NewPlatform,
                    Status = NewStatus
                };
                TrainSchedules.Add(newEntry);
                ClearNewEntryFields(); // Clear form for next potential add
            }
            else
            {
                // Handle case where IsEditing is true but SelectedItem is null (shouldn't normally happen)
                IsEditing = false;
                ClearNewEntryFields();
            }
        }

        private bool CanExecuteDeleteCommand(object? parameter)
        {
            // Can only delete if an item is actually selected
            return SelectedScheduleEntry != null;
        }

        private void ExecuteDeleteCommand(object? parameter)
        {
            if (SelectedScheduleEntry != null)
            {
                TrainScheduleEntry itemToDelete = SelectedScheduleEntry;
                SelectedScheduleEntry = null; // De-select first (triggers form clear)
                TrainSchedules.Remove(itemToDelete); // Then remove
            }
        }

        private bool CanExecuteCancelEditCommand(object? parameter)
        {
            // Can only cancel if we are currently editing
            return IsEditing;
        }

        private void ExecuteCancelEditCommand(object? parameter)
        {
            // Simply clear the selection; the setter logic handles resetting the form and IsEditing flag.
            SelectedScheduleEntry = null;
        }


        // --- Helper Methods ---

        private void ClearNewEntryFields()
        {
            NewTrainNumber = string.Empty;
            NewOrigin = string.Empty;
            NewDestination = string.Empty;
            ResetTimeFields();
            NewPlatform = string.Empty; // Set to empty string or null as appropriate
            NewStatus = "On Time";

            // Ensure IsEditing is false AFTER clearing fields that might trigger command updates
            IsEditing = false;
        }

        private void ResetTimeFields()
        {
            var now = DateTime.Now;
            NewDepartureTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0).AddHours(1);
            NewArrivalTime = NewDepartureTime.AddHours(1);
        }

        private void LoadEntryForEditing(TrainScheduleEntry entry)
        {
            // Use properties to ensure validation/updates trigger if setters have logic
            NewTrainNumber = entry.TrainNumber;
            NewOrigin = entry.Origin;
            NewDestination = entry.Destination;
            NewDepartureTime = entry.DepartureTime;
            NewArrivalTime = entry.ArrivalTime;
            NewPlatform = entry.Platform;
            NewStatus = entry.Status;
            // IsEditing flag is set by the caller (SelectedScheduleEntry setter)
        }

        private void LoadSampleData()
        {
            TrainSchedules.Add(new TrainScheduleEntry { TrainNumber = "EXP 101", Origin = "City A", Destination = "City B", DepartureTime = DateTime.Today.AddHours(8).AddMinutes(30), ArrivalTime = DateTime.Today.AddHours(10).AddMinutes(45), Platform = "1", Status = "On Time" });
            TrainSchedules.Add(new TrainScheduleEntry { TrainNumber = "REG 55", Origin = "City C", Destination = "City D", DepartureTime = DateTime.Today.AddHours(9).AddMinutes(00), ArrivalTime = DateTime.Today.AddHours(9).AddMinutes(55), Platform = "3A", Status = "On Time" });
            TrainSchedules.Add(new TrainScheduleEntry { TrainNumber = "IC 204", Origin = "City B", Destination = "City E", DepartureTime = DateTime.Today.AddHours(11).AddMinutes(15), ArrivalTime = DateTime.Today.AddHours(14).AddMinutes(30), Platform = "2", Status = "Delayed" });
        }
    }

    // --- Helper Extension Method for Refreshing DataGrid Item ---
    public static class ObservableCollectionExtensions
    {
        // Renamed for clarity
        public static void RefreshItem<T>(this ObservableCollection<T> collection, int index)
        {
            if (collection == null || index < 0 || index >= collection.Count) return;

            // This common workaround forces the UI bound to the collection to re-render the item at the specified index.
            // It's often needed when the item *itself* doesn't implement INotifyPropertyChanged for its own properties.
            T item = collection[index];
            collection.RemoveAt(index);
            collection.Insert(index, item);
        }
    }
}