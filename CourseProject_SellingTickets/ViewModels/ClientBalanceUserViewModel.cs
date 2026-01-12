using System;
using System.Reactive;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Commands.UserClientCommands;
using CourseProject_SellingTickets.Interfaces;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Models;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class ClientBalanceUserViewModel : ViewModelBase, IParameterReceiver
{
    
    // Custom parameters for viewModel 
    
    public Int64 UserId { get; private set; }
    
    // Services 
    
    private IUserDbProvider _userDbProvider;
    
    // Observable values 
    
    private bool _databaseHasConnected;
    public bool DatabaseHasConnected { get => _databaseHasConnected; set => this.RaiseAndSetIfChanged(ref _databaseHasConnected, value); }
    
    private string _userName = String.Empty;
    public string UserName { get => _userName;  set => this.RaiseAndSetIfChanged(ref _userName, value); }
    
    private string _discountText = String.Empty; 
    public string DiscountText { get => _discountText; set => this.RaiseAndSetIfChanged(ref _discountText, value); }
    
    private decimal _balance;
    public decimal Balance { get => _balance; set => this.RaiseAndSetIfChanged(ref _balance, value); }
    
    private decimal _amount;
    public decimal Amount { get => _amount; set => this.RaiseAndSetIfChanged(ref _amount, value); }

    private ResultStatus _resultDepositBalance; 
    public ResultStatus ResultDepositBalance { get => _resultDepositBalance; set => this.RaiseAndSetIfChanged(ref _resultDepositBalance, value); }

    private string? _depositBalanceMsg;
    public string? DepositBalanceMsg { get => _depositBalanceMsg; set => this.RaiseAndSetIfChanged(ref _depositBalanceMsg, value); }
    
    private string? _errorMessage;
    public string? ErrorMessage { get => _errorMessage; set { this.RaiseAndSetIfChanged(ref _errorMessage, value); this.RaisePropertyChanged(nameof(HasErrorMessage)); } }

    public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
    
    private bool? _isLoading;
    public bool? IsLoading { get => _isLoading; set => this.RaiseAndSetIfChanged(ref _isLoading, value); }

    // Commands

    private ReactiveCommand<Unit, Task>? _loadUserDataCommand;
    public ReactiveCommand<Unit, Task> LoadUserDataCommand =>
        _loadUserDataCommand ??= new LoadUserDataCommand(this, _userDbProvider);

    private ReactiveCommand<Unit, Task>? _depositBalanceUserCommand;
    public ReactiveCommand<Unit, Task> DepositBalanceUserCommand =>
        _depositBalanceUserCommand ??= new DepositBalanceUserCommand(this, _userDbProvider);
    
    public ClientBalanceUserViewModel(IUserDbProvider userDbProvider)
    {
        _userDbProvider = userDbProvider;
        
        ConnectionDbState.CheckConnectionState
            .Subscribe(isConnected => DatabaseHasConnected = isConnected.Result);
    }
    
    public void ReceieveParameter(object parameter)
    {
        UserId = (Int64)(parameter is Int64 ? parameter : 0);
        LoadUserDataCommand.Execute();
    }
}
