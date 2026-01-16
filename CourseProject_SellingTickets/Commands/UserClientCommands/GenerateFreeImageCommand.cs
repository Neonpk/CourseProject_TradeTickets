using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.Common;
using CourseProject_SellingTickets.Interfaces.FreeImageServiceInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.Models.Common;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.UserClientCommands;

public class GenerateFreeImageCommand : ReactiveCommand<FileMeta, Task<IResult<string>>>
{
    private static async Task<IResult<string>> GenerateUserAvatar(FileMeta fileMeta, IFreeImageService freeImageService)
    {

        var uploadFreeImageResult = await freeImageService.UploadImageAsync(fileMeta.Bytes, fileMeta.FileName);

        if (!uploadFreeImageResult.IsSuccess)
        {
            return Result<string>.Failure(uploadFreeImageResult.Message!);
        }

        return Result<string>.Success(uploadFreeImageResult.Value!);
    }
    
    protected internal GenerateFreeImageCommand(IFreeImageService freeImageService) 
        : base (fileMeta => Observable.Start(async () => await GenerateUserAvatar(fileMeta, freeImageService)), Observable.Return(true) )
    {
    }
}
