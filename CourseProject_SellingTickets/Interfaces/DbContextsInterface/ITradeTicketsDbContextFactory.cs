using CourseProject_SellingTickets.DbContexts;

namespace CourseProject_SellingTickets.Interfaces.DbContextsInterface;

public interface ITradeTicketsDbContextFactory
{
    TradeTicketsDbContext CreateDbContext();
}
