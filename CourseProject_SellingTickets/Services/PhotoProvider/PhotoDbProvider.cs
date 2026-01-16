using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseProject_SellingTickets.DbContexts;
using CourseProject_SellingTickets.Extensions;
using CourseProject_SellingTickets.Interfaces.Factories;
using CourseProject_SellingTickets.Interfaces.PhotoProviderInterface;
using CourseProject_SellingTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_SellingTickets.Services.PhotoProvider;

public class PhotoDbProvider : IPhotoDbProvider
{
    private readonly ITradeTicketsDbContextFactory _dbContextFactory;
    
    public PhotoDbProvider(ITradeTicketsDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public async Task<IEnumerable<Photo>> GetAllPhotos()
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<PhotoDTO> photoDtos = await context.Photos.
                AsNoTracking().
                ToListAsync();

            return photoDtos.Select(photo => ToPhoto(photo));
        }
    }

    public async Task<IEnumerable<Photo>> GetTopPhotos(int topRows = 50)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<PhotoDTO> photoDtos = await context.Photos.
                OrderByDescending(x => x.Id).
                TakeOrDefault(topRows).
                AsNoTracking().
                ToListAsync();

            return photoDtos.Select(photo => ToPhoto(photo));
        }
    }

    public async Task<IEnumerable<Photo>> GetPhotosByFilter(Expression<Func<PhotoDTO, bool>> searchFunc, int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<PhotoDTO> photoDtos = await context.Photos.
                Where(searchFunc).
                OrderByDescending(x => x.Id).
                TakeOrDefault(topRows).
                AsNoTracking().
                ToListAsync();

            return photoDtos.Select(photo => ToPhoto(photo));
        }
    }

    public async Task<IEnumerable<Photo>> GetPhotosByFilterSort<TKeySelector>(Expression<Func<PhotoDTO, bool>> searchFunc, Expression<Func<PhotoDTO, TKeySelector>> sortFunc, SortMode? sortMode,
        int topRows = -1)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<PhotoDTO> photoDtos = await context.Photos.
                Where(searchFunc).
                OrderByModeOrDefault( sortFunc, sortMode ).
                TakeOrDefault(topRows).
                AsNoTracking().
                ToListAsync();

            return photoDtos.Select(ticket => ToPhoto(ticket));
        }
    }

    public async Task<int> CreateOrEditPhoto(Photo photo)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            PhotoDTO photoDto = ToPhotoDto(photo);

            if (photoDto.Id.Equals(default))
                await context.Photos.AddAsync(photoDto);
            else
                context.Photos.Attach(photoDto).State = EntityState.Modified;

            return await context.SaveChangesAsync();
        }
    }

    public async Task<int> DeletePhoto(Photo photo)
    {
        using (TradeTicketsDbContext context = _dbContextFactory.CreateDbContext())
        {
            PhotoDTO photoDto = ToPhotoDto(photo);
            
            context.Photos.Remove(photoDto);
            return await context.SaveChangesAsync();
        }
    }

    public static PhotoDTO ToPhotoDto(Photo photo)
    {
        return new PhotoDTO
        {
            Id = photo.Id,
            Name = photo.Name,
            UrlPath = photo.UrlPath,
            IsDeleted = photo.IsDeleted
        };
    }
    
    public static Photo ToPhoto(PhotoDTO photoDto)
    {
        return new Photo(photoDto.Id, photoDto.Name, photoDto.UrlPath, photoDto.IsDeleted);
    }
}
