using System;
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

    private string _userName = String.Empty;
    public string UserName { get => _userName;  set => this.RaiseAndSetIfChanged(ref _userName, value); }

    private decimal _balance;
    public decimal Balance { get => _balance; set => this.RaiseAndSetIfChanged(ref _balance, value); }

    // Commands

    private ReactiveCommand<Int64, Task<User>>? _getUserDataCommand;
    
    public ReactiveCommand<long, Task<User>> GetUserDataCommand =>
        _getUserDataCommand ??= new GetUserDataCommand(_userDbProvider);

    public ClientBalanceUserViewModel(IUserDbProvider userDbProvider)
    {
        _userDbProvider = userDbProvider;
    }
    
    public void ReceieveParameter(object parameter)
    {
        UserId = (Int64)(parameter is Int64 ? parameter : 0);
        
        GetUserDataCommand.Execute(UserId).Subscribe(async void (userModel) =>
        {
            var user = await userModel;

            UserName = user.Name;
            Balance = user.Balance;
        });
    }
}
