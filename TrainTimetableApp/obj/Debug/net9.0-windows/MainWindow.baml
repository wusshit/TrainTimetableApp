   M S B A M L   `   `   `  ����L  HTrainTimetableApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:TrainTimetableAppTrainTimetableApp  5   	   "    TrainTimetableApp.MainWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:TrainTimetableApp     5   	     � Title$  Train Timetable��5   	   $	��600��6!   $	��850��6.    � DataContext 5
      +   %TrainTimetableApp.MainWindowViewModel  5   
    � 	Resources 5      4   .TrainTimetableApp.BooleanToVisibilityConverter   BoolToVisibilityConverter��%   &	          5   
   6-   5      .��5      � $��15q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5       $
��Auto �6   .+�5%   
   �� $��0��6   $#��Train Departures and Arrivals��6!   $��22(�6F   $
��Bold"�6T   $��Center=�6f   $��0,0,0,15q�6�   &   System.Windows.Controls.DataGrid  5(   
   $��1��6      AutoGenerateColumns$ False��5)         
IsReadOnly$
 True��6/      CanUserAddRows$ False��6A      SelectionMode$ Single=�6X   $��0,0,0,10q�5*         AlternatingRowBackground
 �����	   CanUserSortColumns$
	 True��6J   ��5(       �� *TrainSchedules+
 �� SelectedItem
 6G   �� *SelectedScheduleEntry+ �� Mode$ TwoWay=�   Columns 5+      0  *System.Windows.Controls.DataGridTextColumn  5,      ,  &System.Windows.Controls.DataGridColumn   Header$ 	Train No.��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 1* 6X      MinWidth$ 80Q�6c   1  +System.Windows.Controls.DataGridBoundColumn   Binding 68   �� *TrainNumber+6c     5-      $ Origin��6%   $
 1.5* 6P   $	 100Q�6]    65   �� *Origin+6]     5.      $ Destination��6%   $
 1.5* 6Z   $	 100Q�6g    6:   �� *Destination+6g     5/      $ 	Departure��6%   $
 1.2* 6}   $	 120Q�6�    68   �� *DepartureTime+ �� StringFormat$ {0:dd/MM/yy HH:mm}��6�     50      $ Arrival��6%   $
 1.2* 6y   $	 120Q�6�    66   �� *ArrivalTime+$ {0:dd/MM/yy HH:mm}��6�     51      $ Platform��6%   $
 0.8* 6T   $ 60Q�6a    67   �� *
Platform+6a     52      $ Status��6%   $ 1* 6P   $ 80Q�6[    65   �� *Status+6[   54      �� 57   
   $��2��6   $	��0,5q�6!   �� 5:   
   $��3��6    �� Orientation$ 
Horizontal=�6"   $��0,10,0,5q�6;   .+�5<      �� $��Add New Schedule Entry��6   $��16(�66   $��SemiBold"�6D   y�5=      �� *	IsEditing+$ OneWay=� �� 	Converter# [   �� ConverterParameter$ Invert���� 5?      $��Edit Schedule Entry��6   $��16(�63   $��SemiBold"�6A   y�5@      �� *	IsEditing+$ OneWay=�# [  5A      � 5E   
   $��4��6   $��0,5,0,15q�6    � ColumnDefinitions 5F      �� 5G      $
��Auto �6#   �� 5H      $��* �6#   �� 5I      $
��Auto �6#   �� 5J      $��* �6#   �� 5K      $
��Auto �6#   �� 5L      $��* �6#    5N      �� 5O      $
��Auto �6    �� 5P      $
��Auto �6    �� 5Q      $
��Auto �6    .+�5U      �� $��0��6   $��0��6!   $��_Train No.:��61   $��Center=�6u    �� Target 6G   ��  �� ElementName$ txtTrainNumber��6u   �� 5V      -   5         ��Name$ txtTrainNumber��5V      $��0��6.   $��1��6;   $��5q�6�   $��Center=�6�   ��6K   �� *NewTrainNumber+ �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5W      $��0��6   $��2��6!   $��_Origin:��61   $��Center=�6m    6D   �� $ 	txtOrigin��6m   �� 5X      -   5        $ 	txtOrigin��5X      $��0��6)   $��3��66   $��5q�6�   $��Center=�6�   ��6F   �� *	NewOrigin+$ PropertyChanged=�6�   �� 5Y      $��0��6   $��4��6!   $��_Destination:��61   $��Center=�6w    6I   �� $ txtDestination��6w   �� 5Z      -   5        $ txtDestination��5Z      $��0��6.   $��5��6;   $��5q�6�   $��Center=�6�   ��6K   �� *NewDestination+$ PropertyChanged=�6�   �� 5]      $��1��6   $��0��6!   $��D_eparture:��61   $��Center=�6s    6G   �� $ txtDeparture��6s   �� 5^      -   5        $ txtDeparture��5^      $��1��6,   $��1��69   $��5q�6�   $��Center=�6�    � ToolTip$ Format: YYYY-MM-DD HH:MM��6�   ��6I   �� *NewDepartureTime+$ {0:yyyy-MM-dd HH:mm}��6�   �� 5_      $��1��6   $��2��6!   $��	_Arrival:��61   $��Center=�6o    6E   �� $ 
txtArrival��6o   �� 5`      -   5        $ 
txtArrival��5`      $��1��6*   $��3��67   $��5q�6�   $��Center=�6�   $ Format: YYYY-MM-DD HH:MM��6�   ��6G   �� *NewArrivalTime+$ {0:yyyy-MM-dd HH:mm}��6�   �� 5c      $��2��6   $��0��6!   $��
_Platform:��61   $��Center=�6q    6F   �� $ txtPlatform��6q   �� 5d      -   5        $ txtPlatform��5d      $��2��6+   $��1��68   $��5q�6�   $��Center=�6�   ��6H   �� *NewPlatform+$ PropertyChanged=� �� TargetNullValue$  ��6�   �� 5f      $��2��6   $��2��6!   $��_Status:��61   $��Center=�6m    6D   �� $ 	txtStatus��6m   �� 5g      -   5        $ 	txtStatus��5g      $��2��6)   $��3��66   $��3��6F   $��5q�6�   $��Center=�6�   ��6Z   �� *	NewStatus+$ PropertyChanged=�6�   5h      �� 5k   
   $��5��6   $ 
Horizontal=�6"   $��Right=�6;   .+�5l      �� $	��110��6a   $��5q�6m   $
��10,5q�6x   $��Medium"�6�   ��6   �� *AddButtonContent+��6:   �� *AddOrUpdateCommand+6�   �� 5m      $��Cancel Edit��6   $	��110��6�   $��5q�6�   $
��10,5q�6�   ��6+   �� *CancelEditCommand+y�6Q   �� *	IsEditing+# [  6�   �� 5n      $��Delete Selected��6   $	��110��6Q   $��5q�6]   $
��10,5q�6h   ��6/   �� *DeleteCommand+6h   5o      5q      5r      