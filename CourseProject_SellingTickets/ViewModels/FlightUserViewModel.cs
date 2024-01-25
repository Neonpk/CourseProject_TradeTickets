using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services.TradeTicketsProvider;
using DynamicData;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class FlightUserViewModel : ViewModelBase
{
    // Private 

    private IFlightProvider? _flightProvider;
    
    // Observable properties 

    private bool _sideBarShowed;
    public bool SideBarShowed { get => _sideBarShowed; set { _sideBarShowed = value; OnPropertyChanged(nameof(SideBarShowed)); } }

    // ObservableCollection
    
    private ObservableCollection<Flight>? _flightsItems;
    public ObservableCollection<Flight>? FlightItems { get => _flightsItems; set { _flightsItems = value; OnPropertyChanged(nameof(FlightItems)); } }
    
    // Commands 

    private ICommand? _hideSideBarCommand;
    public ICommand HideSideBarCommand { get => _hideSideBarCommand ??= ReactiveCommand.Create<object>((_) => SideBarShowed = false); }
    
    private ICommand? _addEditDataCommand;
    public ICommand AddEditDataCommand { get => _addEditDataCommand ??= ReactiveCommand.Create<object>((_) => SideBarShowed = true); }

    
    // Constructor 
    public FlightUserViewModel(IFlightProvider? flightProvider)
    {
        _flightProvider = flightProvider;
    }

}