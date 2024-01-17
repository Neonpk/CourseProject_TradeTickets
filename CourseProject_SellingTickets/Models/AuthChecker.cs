using System;

namespace CourseProject_SellingTickets.Models;

public static class AuthChecker
{
    public static AuthStates CheckDispatcherPassword(string Password)
    {
        if (Password.Trim() == String.Empty) 
            return AuthStates.None;

        return Password == "123" ? AuthStates.Success : AuthStates.Failed;
    }
    
    public static AuthStates CheckAdminPassword(string Password)
    {
        if (Password.Trim() == String.Empty) 
            return AuthStates.None;

        return Password == "321" ? AuthStates.Success : AuthStates.Failed;
    }
    
}