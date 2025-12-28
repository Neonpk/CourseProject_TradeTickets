using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_SellingTickets.Services.TicketProvider;

public class TicketDbProvider : ITicketDbProvider
{
    private readonly ITradeTicketsDbContextFactory _dbContextFactory;
    
    public TicketDbProvider(ITradeTicketsDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public async Task<IEnumerable<Ticket>> GetAllTickets()
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
             IEnumerable<TicketDTO> ticketDtos = await context.Tickets.
                AsNoTracking().
                Include( x => x.Flight ).
                Include( x => x.FlightClass ). 
                Include( x => x.Discount ).
                Include( x => x.Flight.DestinationPlace ).
                Include( x => x.Flight.Aircraft ).
                Include( x => x.Flight.DeparturePlace ).
                Include( x => x.Flight.Airline ).
                Include( x => x.Flight.DestinationPlace!.Photo ).
                Include( x => x.Flight.DeparturePlace!.Photo ).
                Include( x => x.Flight.Aircraft!.Photo ).
                Include( x => x.User).
                ToListAsync();

            return ticketDtos.Select(ticket => ToTicket(ticket));
        }
    }

    public async Task<IEnumerable<Ticket>> GetTopTickets(int topRows = 50)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<TicketDTO> ticketDtos = await context.Tickets.
                OrderByDescending(x => x.Id).
                TakeOrDefault(topRows).
                AsNoTracking().
                Include( x => x.Flight ).
                Include( x => x.FlightClass ). 
                Include( x => x.Discount ).
                Include( x => x.Flight.DestinationPlace ).
                Include( x => x.Flight.Aircraft ).
                Include( x => x.Flight.DeparturePlace ).
                Include( x => x.Flight.Airline ).
                Include( x => x.Flight.DestinationPlace!.Photo ).
                Include( x => x.Flight.DeparturePlace!.Photo ).
                Include( x => x.Flight.Aircraft!.Photo ).
                Include( x => x.User).
                ToListAsync();

            return ticketDtos.Select(ticket => ToTicket(ticket));
        }
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByFilter(Expression<Func<TicketDTO, bool>> searchFunc, int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<TicketDTO> ticketDtos = await context.Tickets.
                Where(searchFunc).
                OrderByDescending(x => x.Id).
                TakeOrDefault(topRows).
                AsNoTracking().
                Include( x => x.Flight ).
                Include( x => x.FlightClass ). 
                Include( x => x.Discount ).
                Include( x => x.Flight.DestinationPlace ).
                Include( x => x.Flight.Aircraft ).
                Include( x => x.Flight.DeparturePlace ).
                Include( x => x.Flight.Airline ).
                Include( x => x.Flight.DestinationPlace!.Photo ).
                Include( x => x.Flight.DeparturePlace!.Photo ).
                Include( x => x.Flight.Aircraft!.Photo ).
                Include( x => x.User).
                ToListAsync();

            return ticketDtos.Select(ticket => ToTicket(ticket));
        }
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByFilterSort<TKeySelector>(Expression<Func<TicketDTO, bool>> searchFunc, Expression<Func<TicketDTO, TKeySelector>> sortFunc, SortMode? sortMode,
        int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<TicketDTO> ticketDtos = await context.Tickets.
                Where(searchFunc).
                OrderByModeOrDefault( sortFunc, sortMode ).
                TakeOrDefault(topRows).
                AsNoTracking().
                Include( x => x.Flight ).
                Include( x => x.FlightClass ). 
                Include( x => x.Discount ).
                Include( x => x.Flight.DestinationPlace ).
                Include( x => x.Flight.Aircraft ).
                Include( x => x.Flight.DeparturePlace ).
                Include( x => x.Flight.Airline ).
                Include( x => x.Flight.DestinationPlace!.Photo ).
                Include( x => x.Flight.DeparturePlace!.Photo ).
                Include( x => x.Flight.Aircraft!.Photo ).
                Include( x => x.User).
                ToListAsync();

            return ticketDtos.Select(ticket => ToTicket(ticket));
        }
    }

    public async Task<int> CreateOrEditTicket(Ticket ticket)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            TicketDTO ticketDto = ToTicketDto(ticket);

            if (ticketDto.Id.Equals(default))
                await context.Tickets.AddAsync(ticketDto);
            else
                context.Tickets.Attach(ticketDto).State = EntityState.Modified;

            return await context.SaveChangesAsync();
        }
    }

    public async Task<int> DeleteTicket(Ticket ticket)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            TicketDTO ticketDto = ToTicketDto(ticket);
            
            context.Tickets.Remove(ticketDto);
            return await context.SaveChangesAsync();
        }
    }
    
    private static Ticket ToTicket(TicketDTO ticketDto)
    {
        return new Ticket( 
            ticketDto.Id, 
            new Flight ( ticketDto.Flight ),
            new FlightClass( ticketDto.FlightClass.Id, ticketDto.FlightClass.ClassName ),
            ticketDto.PlaceNumber, 
            new Discount( ticketDto.Discount.Id, ticketDto.Discount.Name, ticketDto.Discount.DiscountSize, ticketDto.Discount.Description ),
            ticketDto.IsSold,
            ticketDto.User is not null ? 
            new User( ticketDto.User.Id, ticketDto.User.Login, ticketDto.User.Name, ticketDto.User.Role, ticketDto.User.Password, ticketDto.User.Balance ) : new User()
        );
    }
    
    private static TicketDTO ToTicketDto(Ticket ticket)
    {
        return new TicketDTO
        {
            Id = ticket.Id,
            FlightId = ticket.Flight.Id,
            ClassId = ticket.FlightClass.Id,
            PlaceNumber = ticket.PlaceNumber,
            DiscountId = ticket.Discount.Id,
            IsSold = ticket.IsSold,
            UserId = ticket.User!.Id
        };
    }
    
}