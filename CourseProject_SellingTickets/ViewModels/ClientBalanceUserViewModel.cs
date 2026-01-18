using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CourseProject_SellingTickets.Commands.PhotoCommands;
using CourseProject_SellingTickets.Commands.UserClientCommands;
using CourseProject_SellingTickets.Interfaces;
using CourseProject_SellingTickets.Interfaces.CommonInterface;
using CourseProject_SellingTickets.Interfaces.FileServiceInterface;
using CourseProject_SellingTickets.Interfaces.FreeImageServiceInterface;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Models;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class ClientBalanceUserViewModel : ViewModelBase, IParameterReceiver
{
    
    // Custom parameters for viewModel 
    
    public Int64 UserId { get; private set; }
    
    // Services 
    
    private readonly IUserDbProvider _userDbProvider;
    
    private readonly IFreeImageService _freeImageService;
    
    private readonly IFileService _fileService;
    
    private INavigationService? _navigationClientUserService;
    public INavigationService? NavigationClientUserService { get => _navigationClientUserService; set => this.RaiseAndSetIfChanged(ref _navigationClientUserService, value); }

    
    // Observable values 
    
    private bool _databaseHasConnected;
    public bool DatabaseHasConnected { get => _databaseHasConnected; set => this.RaiseAndSetIfChanged(ref _databaseHasConnected, value); }
    
    private string _userName = String.Empty;
    public string UserName { get => _userName;  set => this.RaiseAndSetIfChanged(ref _userName, value); }
    
    private string _discountText = String.Empty; 
    public string DiscountText { get => _discountText; set => this.RaiseAndSetIfChanged(ref _discountText, value); }
    
    private DateTime _birthDay;
    public DateTime BirthDay { get => _birthDay; set => this.RaiseAndSetIfChanged(ref _birthDay, value); }
    
    private string _passport = String.Empty;
    public string Passport { get => _passport; set => this.RaiseAndSetIfChanged(ref _passport, value); }
    
    private decimal _balance;
    public decimal Balance { get => _balance; set => this.RaiseAndSetIfChanged(ref _balance, value); }
    
    private decimal _amount;
    public decimal Amount { get => _amount; set => this.RaiseAndSetIfChanged(ref _amount, value); }

    private Photo? _photo;
    public Photo? Photo { get => _photo; set => this.RaiseAndSetIfChanged(ref _photo, value); }
    
    private ResultStatus _resultDepositBalance; 
    public ResultStatus ResultDepositBalance { get => _resultDepositBalance; set => this.RaiseAndSetIfChanged(ref _resultDepositBalance, value); }

    private string _depositBalanceMsg = String.Empty;
    public string DepositBalanceMsg { get => _depositBalanceMsg; set => this.RaiseAndSetIfChanged(ref _depositBalanceMsg, value); }

    private string _fileUrl = String.Empty;
    public string FileUrl { get => _fileUrl; set => this.RaiseAndSetIfChanged(ref _fileUrl, value); }
    
    private int _selectedUploadFileMode = -1;
    public int SelectedUploadFileMode
    {
        get => _selectedUploadFileMode;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedUploadFileMode, value);
            this.RaisePropertyChanged(nameof(IsUrlInputVisible));
            this.RaisePropertyChanged(nameof(IsFileInputVisible));
        }
    }

    public bool IsUrlInputVisible => (UploadFileMode)SelectedUploadFileMode == UploadFileMode.FromUrl;
    public bool IsFileInputVisible => (UploadFileMode)SelectedUploadFileMode == UploadFileMode.FromFile; 
    
    private string _errorMessage = String.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            this.RaiseAndSetIfChanged(ref _errorMessage, value);
            this.RaisePropertyChanged(nameof(HasErrorMessage));
        }
    }

    public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
    
    private bool _isLoading;
    public bool IsLoading { get => _isLoading; set => this.RaiseAndSetIfChanged(ref _isLoading, value); }
    
    // Commands

    private ReactiveCommand<Unit, Task>? _loadUserDataCommand;
    public ReactiveCommand<Unit, Task> LoadUserDataCommand =>
        _loadUserDataCommand ??= new LoadUserDataCommand(this, _userDbProvider);

    private ReactiveCommand<Unit, Task>? _depositBalanceUserCommand;
    public ReactiveCommand<Unit, Task> DepositBalanceUserCommand =>
        _depositBalanceUserCommand ??= new DepositBalanceUserCommand(this, _userDbProvider);
    
    private ReactiveCommand<TopLevel, Task<IResult<FileMeta>>>? _choosePhotoCommand;
    public ReactiveCommand<TopLevel, Task<IResult<FileMeta>>> ChoosePhotoCommand =>
        _choosePhotoCommand ??= new ChoosePhotoCommand(_fileService);
    
    private ReactiveCommand<FileMeta, Task<IResult<string>>>? _generateFreeImageCommand;
    public ReactiveCommand<FileMeta, Task<IResult<string>>> GenerateFreeImageCommand =>
        _generateFreeImageCommand ??= new GenerateFreeImageCommand(_freeImageService);
    
    private ReactiveCommand<string, Task>? _saveUserAvatarCommand;
    public ReactiveCommand<string, Task> SaveUserAvatarCommand =>
        _saveUserAvatarCommand ??= new SaveUserAvatarCommand(this, _userDbProvider);
    
    private ReactiveCommand<IResult<FileMeta>, Task>? _useCaseUploadImageCommand;
    private ReactiveCommand<IResult<FileMeta>, Task> UseCaseUploadImageCommand =>
        _useCaseUploadImageCommand ??= new UseCaseUploadImageCommand(this);
    
    public ClientBalanceUserViewModel(
        INavigationService? navClientUserService, 
        IUserDbProvider userDbProvider,
        IFileService fileService,
        IFreeImageService freeImageService
        )
    {
        _userDbProvider = userDbProvider;
        _fileService = fileService;
        _freeImageService = freeImageService;
        
        NavigationClientUserService = navClientUserService;

        ChoosePhotoCommand.
            Subscribe(async void (fResult) => await UseCaseUploadImageCommand.Execute(await fResult));
        
        ConnectionDbState.CheckConnectionState
            .Subscribe(isConnected => DatabaseHasConnected = isConnected.Result);
    }
    
    public void ReceieveParameter(object parameter)
    {
        UserId = (Int64)(parameter is Int64 ? parameter : 0);
        LoadUserDataCommand.Execute();
    }
}
