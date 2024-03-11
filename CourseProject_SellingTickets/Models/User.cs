using System;

namespace CourseProject_SellingTickets.Models;

public class User
{
    public Int64 Id { get; set; }
    
    public string Mode { get; set; }
    
    public string Password { get; set; }

    public User(Int64 id, string mode, string password)
    {
        Id = id;
        Mode = mode;
        Password = password;
    }
    
}