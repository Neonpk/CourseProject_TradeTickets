using System.Net.Http;

namespace CourseProject_SellingTickets.Interfaces.Factories;

public interface IHttpClientFactory
{
    HttpClient CreateHttpClient();
    MultipartFormDataContent CreateMultiPartFormDataContent();
    StringContent CreateStringContent(string content);
    ByteArrayContent CreateByteArrayContent(byte[] bytes);
}
