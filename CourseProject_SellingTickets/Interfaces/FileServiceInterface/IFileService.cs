using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CourseProject_SellingTickets.Interfaces.CommonInterface;

namespace CourseProject_SellingTickets.Interfaces.FileServiceInterface;

public interface IFileService
{
    Task<IResult<IReadOnlyList<IStorageFile>>> OpenFileAsync(TopLevel visual, string title, string types, string[] allowedExtensions, bool allowMultiple = false);
    
    Task<IResult<byte[]>> ReadAllBytes(string fileName);
}
