// TrainScheduleEntry.cs
using System;

namespace TrainTimetableApp
{
    public class TrainScheduleEntry
    {
        public string TrainNumber { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string? Platform { get; set; }
        public string Status { get; set; } = "On Time";

        public override string ToString()
        {
            return $"{TrainNumber}: {Origin} ({DepartureTime:HH:mm}) -> {Destination} ({ArrivalTime:HH:mm}) - {Status}";
        }
    }
}