using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.CommonInterface;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.UserClientCommands;

public class UseCaseUploadImageCommand : ReactiveCommand<IResult<FileMeta>, Task>
{
    private static async Task UseCaseUploadImage(ClientBalanceUserViewModel clientBalanceUserVm, IResult<FileMeta> fileMeta)
    {
        if (!fileMeta.IsSuccess)
        {
            clientBalanceUserVm.ErrorMessage = fileMeta.Message!;
            return;
        }

        clientBalanceUserVm.IsLoading = true;
        var freeImageResultTask = await clientBalanceUserVm.GenerateFreeImageCommand.Execute(fileMeta.Value).ToTask();
        var freeImageResult = await freeImageResultTask;
        
        if (!freeImageResult.IsSuccess)
        {
            clientBalanceUserVm.ErrorMessage = freeImageResult.Message!;
            clientBalanceUserVm.IsLoading = false;
            return;
        }

        await clientBalanceUserVm.SaveUserAvatarCommand.Execute(freeImageResult.Value!);
        clientBalanceUserVm.IsLoading = false;
    }
    
    protected internal UseCaseUploadImageCommand(ClientBalanceUserViewModel clientBalanceUserVm) 
        : base (fileMeta => Observable.Start(async () => await UseCaseUploadImage(clientBalanceUserVm, fileMeta)), Observable.Return(true) )
    {
    }
}
