using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CourseProject_SellingTickets.Interfaces.CommonInterface;
using CourseProject_SellingTickets.Interfaces.FileServiceInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Models.Common;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.UserClientCommands;

public class ChoosePhotoCommand : ReactiveCommand<TopLevel, Task<IResult<FileMeta>>>
{
    private static async Task<IResult<FileMeta>> ChoosePhoto(TopLevel topLevel, IFileService fileService)
    {
        var files = await fileService.OpenFileAsync(
            topLevel, 
            "Выберите файл с изображением", 
            "Изображения", 
            new [] { "*.jpg", "*.jpeg", "*.png" }
        );
        
        if (!files.IsSuccess)
        {
            return Result<FileMeta>.Failure(files.Message!);
        }

        if (files.Value!.Count == 0)
        {
            return Result<FileMeta>.Failure("Файл не выбран.");
        }

        string fileName = files.Value.First().Path.LocalPath;
        IResult<byte[]> fileBytes = await fileService.ReadAllBytes(fileName);

        if (!fileBytes.IsSuccess)
        {
            return Result<FileMeta>.Failure(fileBytes.Message!);
        }
        
        return Result<FileMeta>.Success(new FileMeta
        {
            FileName = fileName,
            Bytes = fileBytes.Value!
        });
    }
    
    protected internal ChoosePhotoCommand(IFileService fileService) 
        : base(level => Observable.Start(async () => await ChoosePhoto(level, fileService)), Observable.Return(true))
    {
        
    }
}
