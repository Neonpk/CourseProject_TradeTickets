using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CourseProject_SellingTickets.Interfaces.Common;
using CourseProject_SellingTickets.Interfaces.FileServiceInterface;
using CourseProject_SellingTickets.Models.Common;

namespace CourseProject_SellingTickets.Services.FileService;

public class FileService : IFileService
{
    public async Task<IResult<IReadOnlyList<IStorageFile>>> OpenFileAsync(TopLevel topLevel, string title, string types, string[] allowedExtensions, bool allowMultiple = false)
    {
        if (Nullable.Equals(topLevel, null)) return Result<IReadOnlyList<IStorageFile>>.Failure("TopLevel not found");
        
        var options = new FilePickerOpenOptions
        {
            Title = title,
            AllowMultiple = allowMultiple,
            FileTypeFilter = new List<FilePickerFileType>
                { new(types) { Patterns = allowedExtensions } }
        };
        
        var storageProvider = topLevel.StorageProvider;
        
        if (storageProvider.CanOpen) 
            return Result<IReadOnlyList<IStorageFile>>.Success(await storageProvider.OpenFilePickerAsync(options));
   
        return Result<IReadOnlyList<IStorageFile>>.Failure("Failed to open file");
    }

    public async Task<IResult<byte[]>> ReadAllBytes(string fileName)
    {
        try
        {
            byte[] bytes = await File.ReadAllBytesAsync(fileName);
            return Result<byte[]>.Success(bytes);
        }
        catch (Exception ex)
        {
            return Result<byte[]>.Failure(ex.Message);
        }
    }
}
