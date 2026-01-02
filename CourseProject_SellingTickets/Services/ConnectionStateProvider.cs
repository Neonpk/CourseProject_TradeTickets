using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Interfaces;

namespace CourseProject_SellingTickets.Services;

public class ConnectionStateProvider : IConnectionStateProvider
{
    private ITradeTicketsDbContextFactory _dbContextFactory;
    
    public ConnectionStateProvider(ITradeTicketsDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<bool> IsConnected()
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            return await context.Database.CanConnectAsync();
        }
    }
}
