using System.Threading.Tasks;

namespace CourseProject_SellingTickets.Interfaces;

public interface IConnectionStateProvider
{
    Task<bool> IsConnected();
}
