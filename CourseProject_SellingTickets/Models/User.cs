using System;
using CourseProject_SellingTickets.ValidationRules;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class User : ReactiveObject, IValidatableViewModel
{
    // Guid (Нужен для идентификации объектов сравнения)

    public Guid Guid { get; } = Guid.NewGuid();
    
    // Columns
    
    private Int64? _id;
    public Int64? Id { get => _id; set => this.RaiseAndSetIfChanged(ref _id, value); }

    private string _login = String.Empty;
    public string Login { get => _login; set => this.RaiseAndSetIfChanged(ref _login, value); }

    private string _name = String.Empty;
    public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }

    private string _role;
    public string Role { get => _role; set => this.RaiseAndSetIfChanged(ref _role, value); }

    private string _password;
    public string Password { get => _password; set => this.RaiseAndSetIfChanged(ref _password, value); }

    private string _newUserPassword = String.Empty;
    public string NewUserPassword { get => _newUserPassword; set => this.RaiseAndSetIfChanged(ref _newUserPassword, value); }
    
    private decimal _balance;
    public decimal Balance { get => _balance; set => this.RaiseAndSetIfChanged(ref _balance, value); }

    private DateTime _birthDay;
    public DateTime BirthDay { get => _birthDay; set => this.RaiseAndSetIfChanged(ref _birthDay, value); }
    
    private string _passport;
    public string Passport
    {
        get => _passport;
        set
        {
            this.RaiseAndSetIfChanged(ref _passport, value);
            this.RaisePropertyChanged(nameof(PassportVisual));
        }
    }

    public string PassportVisual =>
        String.Format("Серия: {0} Номер: {1}", Passport.Substring(0, 4), Passport.Substring(4));
    
    private Discount _discount;
    public Discount Discount { get => _discount; set => this.RaiseAndSetIfChanged(ref _discount, value); }

    private Photo _photo;
    public Photo Photo { get => _photo; set => this.RaiseAndSetIfChanged(ref _photo, value); }
    
    // Validations 
        
    private string _errorValidations;
    public string ErrorValidations { get => _errorValidations; set => this.RaiseAndSetIfChanged(ref _errorValidations, value); }
    public ValidationContext ValidationContext { get; } = new ValidationContext();
    
    public User()
    {
        Login = String.Empty;
        Name = String.Empty;
        Role = "user";
        Password = String.Empty;
        NewUserPassword = String.Empty;
        Balance = 0;
        BirthDay = DateTime.Now;
        Passport = "0000000000";
        Discount = new Discount();
        Photo = new Photo();
        
        // Validations
        
        this.InitializeValidationRules();
    }
    
    public User(
        Int64 id, string login, string name, 
        string role, string password, decimal balance,
        DateTime birthDay, string passport,
        Discount discount, Photo photo
        )
    {
        Id = id;
        Login = login;
        Name = name;
        Role = role;
        Password = password;
        NewUserPassword = String.Empty;
        Balance = balance;
        BirthDay = birthDay;
        Passport = passport;
        Discount = discount;
        Photo = photo;
        
        
        // Validations
        
        this.InitializeValidationRules();
    }

    public User CloneWithPassword(string password)
    {
        Password = password;
        return this;
    }
    
    public override bool Equals(object? obj)
    {
        return obj is User user && Id.Equals(user.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
