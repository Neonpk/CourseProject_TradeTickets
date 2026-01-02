using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.UserProviderInterface;
using CourseProject_SellingTickets.Models;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.UserClientCommands;

public class GetUserDataCommand : ReactiveCommand<Int64, Task<User>>
{
    private static async Task<User> GetUserData(Int64 userId, IUserDbProvider userDbProvider)
    {
        return await userDbProvider.GetUserById(userId);
    }
    
    protected internal GetUserDataCommand(IUserDbProvider userDbProvider) 
        : base (userId => Observable.Start(async () => await GetUserData(userId, userDbProvider)), Observable.Return(true) )
    {
    }
}
