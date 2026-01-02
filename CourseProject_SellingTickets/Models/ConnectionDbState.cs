using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Interfaces;
using CourseProject_SellingTickets.Services;
using ReactiveUI;
using Splat;

namespace CourseProject_SellingTickets.Models;

public sealed class ConnectionDbState
{
    private static readonly IConnectionStateProvider? _connectionStateProvider =
        new ConnectionStateProvider(Locator.Current.GetService<ITradeTicketsDbContextFactory>()!);

    private static ReactiveCommand<Unit, Task<bool>>? _checkConnectionState;
    public static ReactiveCommand<Unit, Task<bool>> CheckConnectionState =>
        _checkConnectionState ??= ReactiveCommand.CreateFromObservable(() => Observable.Start(async () => await _connectionStateProvider!.IsConnected()));
}
