using System;
using ReactiveUI;

namespace CourseProject_SellingTickets.Models;

#pragma warning disable
public class User : ReactiveObject
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

    private decimal _balance;
    public decimal Balance { get => _balance; set => this.RaiseAndSetIfChanged(ref _balance, value); }

    private Discount _discount;
    public Discount Discount { get => _discount; set => this.RaiseAndSetIfChanged(ref _discount, value); }
    
    public User()
    {
        Login = String.Empty;
        Name = String.Empty;
        Role = "user";
        Password = String.Empty;
        Balance = 0;
        Discount = new Discount();
    }
    
    public User(
        Int64 id, string login, string name, 
        string role, string password, decimal balance,
        Discount discount
        )
    {
        Id = id;
        Login = login;
        Name = name;
        Role = role;
        Password = password;
        Balance = balance;
        Discount = discount;
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
