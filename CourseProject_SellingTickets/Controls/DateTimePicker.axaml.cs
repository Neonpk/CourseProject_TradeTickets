using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace CourseProject_SellingTickets.Controls;

public class DateTimePicker : TemplatedControl
{
    
    public static readonly StyledProperty<DateTime?> SelectedDateProperty =
        AvaloniaProperty.Register<DateTimePicker, DateTime?>(
            nameof(SelectedDate),
            enableDataValidation: true,
            defaultBindingMode: BindingMode.TwoWay);

    public static readonly DirectProperty<DateTimePicker, DateTime?> DateOnlyProperty =
        AvaloniaProperty.RegisterDirect<DateTimePicker, DateTime?>(
            nameof(DateOnly),
            x => x.DateOnly,
            (x, y) => x.DateOnly = y,
            enableDataValidation: true,
            defaultBindingMode: BindingMode.TwoWay);
    
    public static readonly DirectProperty<DateTimePicker, TimeSpan?> TimeOnlyProperty =
        AvaloniaProperty.RegisterDirect<DateTimePicker, TimeSpan?>(
            nameof(TimeOnly),
            x => x.TimeOnly,
            (x,y) => x.TimeOnly = y,
            enableDataValidation: true,
            defaultBindingMode: BindingMode.TwoWay);

    private bool _dateTimeOnlyChanging;
    
    public DateTime? SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    private DateTime? _dateOnly;
    public DateTime? DateOnly { get => _dateOnly; set { SetAndRaise(DateOnlyProperty, ref _dateOnly, value); } }

    private TimeSpan? _timeOnly;
    public TimeSpan? TimeOnly { get => _timeOnly; set { SetAndRaise(TimeOnlyProperty, ref _timeOnly, value); } }
    
    
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == SelectedDateProperty)
        {
            if (_dateTimeOnlyChanging)
                return;
            
            var newValue = change.GetNewValue<DateTime?>();
            
            DateOnly = newValue!.Value.Date;
            TimeOnly = newValue.Value.TimeOfDay;
        }
        
        if (change.Property == DateOnlyProperty)
        {
            var newValue = change.GetNewValue<DateTime?>();
            
            _dateTimeOnlyChanging = true;
            SetCurrentValue(SelectedDateProperty, newValue + TimeOnly);
            _dateTimeOnlyChanging = false;
        }
        
        if (change.Property == TimeOnlyProperty)
        {
            var newValue = change.GetNewValue<TimeSpan?>();

            _dateTimeOnlyChanging = true;
            SetCurrentValue(SelectedDateProperty, DateOnly + newValue);
            _dateTimeOnlyChanging = false;
        }
        
        base.OnPropertyChanged(change);
    }
}