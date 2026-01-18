using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.DiscountCommands;

public class AddEditDiscountCommand : ReactiveCommand<bool, Unit>
{
    private static void EditData(DiscountUserViewModel discountUserVm, bool isNewInstance)
    {
        if (isNewInstance)
            discountUserVm.SelectedDiscount = new Discount();

        discountUserVm.SideBarShowed = true;
    }
    
    public AddEditDiscountCommand(DiscountUserViewModel discountUserVm) : 
        base(  isNewInstance => Observable.Start(() => EditData(discountUserVm, isNewInstance)), 
            canExecute: Observable.Return(true)  )
    {
        
    }
}
