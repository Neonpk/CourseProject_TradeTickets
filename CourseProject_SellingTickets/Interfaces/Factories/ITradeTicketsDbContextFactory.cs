using CourseProject_SellingTickets.DbContexts;

namespace CourseProject_SellingTickets.Interfaces.Factories;

public interface ITradeTicketsDbContextFactory
{
    TradeTicketsDbContext CreateDbContext();
}
