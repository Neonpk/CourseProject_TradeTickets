using Microsoft.EntityFrameworkCore;

namespace CourseProject_SellingTickets.DbContexts;

public class TradeTicketsDbContextFactory : ITradeTicketsDbContextFactory
{
    private string? _connectionString;

    public TradeTicketsDbContextFactory(string? connectionString)
    {
        _connectionString = connectionString;
    }

    public TradeTicketsDbContext CreateDbContext()
    {
        DbContextOptions options = new DbContextOptionsBuilder().
            UseLazyLoadingProxies().UseNpgsql(_connectionString).Options;
        
        return new TradeTicketsDbContext(options);
    }
    
}