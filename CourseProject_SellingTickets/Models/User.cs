using System;
using ReactiveUI;

namespace CourseProject_SellingTickets.Models;

public class User : ReactiveObject
{
    public Int64? Id { get; set; }
    
    public string Login { get; set; }
    
    public string Name { get; set; }
    
    public string Role { get; set; }
    
    public string Password { get; set; }
    
    public decimal Balance { get; set; }
    
    public User()
    {
        Login = String.Empty;
        Name = String.Empty;
        Role = "user";
        Password = String.Empty;
        Balance = 0;
    }
    
    public User(
        Int64 id, string login, string name, 
        string role, string password, decimal balance
        )
    {
        Id = id;
        Login = login;
        Name = name;
        Role = role;
        Password = password;
        Balance = balance;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is User o)
        {
            return !Nullable.Equals(o, null) && 
                   Id.Equals(o.Id) &&
                   Name.Equals(o.Name) &&
                   Login.Equals(o.Login) &&
                   Role.Equals(o.Role) &&
                   Password.Equals(o.Password) &&
                   Balance.Equals(o.Balance);
        }
        
        return base.Equals(obj);
    }

    public override int GetHashCode() 
    {
        return base.GetHashCode();
    }
}
