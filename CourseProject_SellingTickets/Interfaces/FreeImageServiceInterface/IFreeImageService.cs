using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.Common;

namespace CourseProject_SellingTickets.Interfaces.FreeImageServiceInterface;

public interface IFreeImageService
{
    Task<IResult<string>> UploadImageAsync(byte[] byteImages, string fileName);
}
