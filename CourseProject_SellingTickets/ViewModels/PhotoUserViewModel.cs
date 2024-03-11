using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseProject_SellingTickets.Commands.PhotoCommands;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Services;
using CourseProject_SellingTickets.Services.PhotoProvider;
using ReactiveUI;

namespace CourseProject_SellingTickets.ViewModels;

public class PhotoUserViewModel : ViewModelBase
{
    private readonly IPhotoVmProvider _photoVmProvider;
    
    //Observable properties 
    
    // => // Filters
    
    private bool _sideBarShowed;
    public bool SideBarShowed { get => _sideBarShowed; set => this.RaiseAndSetIfChanged(ref _sideBarShowed, value); }
    
    // => // Filters => // Search Terms
    
    private string? _searchTerm;
    public string? SearchTerm { get => _searchTerm; set { this.RaiseAndSetIfChanged(ref _searchTerm, value); this.RaisePropertyChanged(nameof(HasSearching)); } }
    public bool HasSearching => !string.IsNullOrEmpty(SearchTerm);
    
    private int _limitRows = 50;
    public int LimitRows { get => _limitRows; set => this.RaiseAndSetIfChanged(ref _limitRows, value); }

    private int _selectedSearchMode;
    public int SelectedSearchMode { get => _selectedSearchMode; set => this.RaiseAndSetIfChanged(ref _selectedSearchMode, value); }
    
    // Selected Model from the list
    
    private Photo? _selectedPhoto;
    public Photo SelectedPhoto { get => _selectedPhoto!; set => this.RaiseAndSetIfChanged(ref _selectedPhoto, value); }
    
    // => // Loading page properties 

    private bool _databaseHasConnected;
    public bool DatabaseHasConnected { get => _databaseHasConnected; set => this.RaiseAndSetIfChanged(ref _databaseHasConnected, value); }

    private bool? _isLoading;
    public bool? IsLoading { get => _isLoading; set => this.RaiseAndSetIfChanged(ref _isLoading, value); }

    private bool? _isLoadingEditMode;
    public bool? IsLoadingEditMode { get => _isLoadingEditMode; set => this.RaiseAndSetIfChanged(ref _isLoadingEditMode, value); }

    private string? _errorMessage;
    public string ErrorMessage { get => _errorMessage!; set { this.RaiseAndSetIfChanged(ref _errorMessage, value); this.RaisePropertyChanged(nameof(HasErrorMessage)); } }

    public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
    
    // => // ObservableCollection And SourceCache
    
    private ObservableCollection<Photo>? _photoItems;
    public ObservableCollection<Photo> PhotoItems => _photoItems ??= new ObservableCollection<Photo>();
    
    // Commands 
    
    private ICommand? _hideSideBarCommand;
    public ICommand HideSideBarCommand => _hideSideBarCommand ??= ReactiveCommand.Create<Unit>(_ => SideBarShowed = false);

    private ReactiveCommand<bool, Unit>? _addEditDataCommand;
    public ReactiveCommand<bool, Unit> AddEditDataCommand => _addEditDataCommand ??= new AddEditPhotoCommand(this);
    
    private ReactiveCommand<IEnumerable<Photo>, Task>? _loadPhotoDataCommand;
    public ReactiveCommand<IEnumerable<Photo>, Task> LoadPhotoDataCommand => _loadPhotoDataCommand ??= new LoadPhotoDataCommand(this, _photoVmProvider!);

    private ReactiveCommand<Unit, Task<IEnumerable<Photo>?>>? _searchPhotoDataCommand;
    public ReactiveCommand<Unit, Task<IEnumerable<Photo>?>> SearchPhotoDataCommand => _searchPhotoDataCommand ??= new SearchPhotoDataCommand(this, _photoVmProvider!);

    private ReactiveCommand<Unit, Task>? _savePhotoDataCommand;
    public ReactiveCommand<Unit, Task> SavePhotoDataCommand => _savePhotoDataCommand ??= new SavePhotoDataCommand(this, _photoVmProvider!);

    private ReactiveCommand<Unit, Task>? _deletePhotoDataCommand;
    public ReactiveCommand<Unit, Task> DeletePhotoDataCommand => _deletePhotoDataCommand ??= new DeletePhotoDataCommand(this, _photoVmProvider!);
    
    // Constructor 
    
    public PhotoUserViewModel(IPhotoVmProvider? photoVmProvider)
    {
        _photoVmProvider = photoVmProvider!;
        
        LoadPhotoDataCommand.Execute();
        SearchPhotoDataCommand.Subscribe(filteredPhotos => LoadPhotoDataCommand!.Execute(filteredPhotos.Result!));
        
        ConnectionDbState.CheckConnectionState.Subscribe(isConnected => DatabaseHasConnected = isConnected.Result);
    }
}