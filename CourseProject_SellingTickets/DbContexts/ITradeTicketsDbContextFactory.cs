using System;

namespace CourseProject_SellingTickets.DbContexts;

public interface ITradeTicketsDbContextFactory
{
    TradeTicketsDbContext CreateDbContext();
}