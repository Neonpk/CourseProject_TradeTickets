using System.Reactive;
using System.Reactive.Linq;
using CourseProject_SellingTickets.Models;
using CourseProject_SellingTickets.ViewModels;
using ReactiveUI;

namespace CourseProject_SellingTickets.Commands.DiscountCommands;

public class AddEditDiscountCommand : ReactiveCommand<bool, Unit>
{
    private static void EditData(DiscountUserViewModel discountUserViewModel, bool isNewInstance)
    {
        if (isNewInstance)
            discountUserViewModel.SelectedDiscount = new Discount();

        discountUserViewModel.SideBarShowed = true;
    }
    
    public AddEditDiscountCommand(DiscountUserViewModel discountUserViewModel) : 
        base(  isNewInstance => Observable.Start(() => EditData(discountUserViewModel, isNewInstance)), 
            canExecute: Observable.Return(true)  )
    {
        
    }
}