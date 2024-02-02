using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;

namespace CourseProject_SellingTickets.Controls;

public class DoubleBounceSpinner : TemplatedControl
{
    
    public static readonly StyledProperty<bool> IsActiveProperty =
        AvaloniaProperty.Register<DoubleBounceSpinner, bool>(
            nameof(IsActive));
    
    public static readonly StyledProperty<int> DiameterProperty =
        AvaloniaProperty.Register<DoubleBounceSpinner, int>(
            nameof(Diameter));

    public static readonly StyledProperty<ISolidColorBrush> ForegroundSpinnerProperty =
        AvaloniaProperty.Register<DoubleBounceSpinner, ISolidColorBrush>(
            nameof(ForegroundSpinner));

    public static readonly DirectProperty<DoubleBounceSpinner, ISolidColorBrush> SecondForegroundProperty =
        AvaloniaProperty.RegisterDirect<DoubleBounceSpinner, ISolidColorBrush>(
            nameof(SecondForeground),
            x => x.SecondForeground,
            (x, y) => x.SecondForeground = y,
            defaultBindingMode: BindingMode.TwoWay);

    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public int Diameter
    {
        get => GetValue(DiameterProperty);
        set => SetValue(DiameterProperty, value);
    }

    public ISolidColorBrush ForegroundSpinner
    {
        get => GetValue(ForegroundSpinnerProperty);
        set => SetValue(ForegroundSpinnerProperty, value);
    }

    private ISolidColorBrush _secondForeground = null!;
    public ISolidColorBrush SecondForeground
    {
        get => _secondForeground;
        set => SetAndRaise(SecondForegroundProperty, ref _secondForeground, value);
    }
    
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {

        if (change.Property == ForegroundSpinnerProperty)
        {
            var newValue = change.GetNewValue<ISolidColorBrush>().Color;

            byte r = Convert.ToByte(newValue.R * 0.5);
            byte g = Convert.ToByte(newValue.G * 0.5);
            byte b = Convert.ToByte(newValue.B * 0.5);
            
            SecondForeground = new SolidColorBrush
            {
                Color = new Color(255, r, g, b)
            };
        }
        
        base.OnPropertyChanged(change);
    }
}