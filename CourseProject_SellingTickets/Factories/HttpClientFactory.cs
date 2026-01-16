using System.Net.Http;
using CourseProject_SellingTickets.Interfaces.Factories;

namespace CourseProject_SellingTickets.Factories;

public class HttpClientFactory : IHttpClientFactory
{
    public HttpClient CreateHttpClient()
    {
        return new HttpClient();
    }

    public MultipartFormDataContent CreateMultiPartFormDataContent()
    {
        return new MultipartFormDataContent();
    }

    public StringContent CreateStringContent(string content)
    {
        return new StringContent(content);
    }

    public ByteArrayContent CreateByteArrayContent(byte[] bytes)
    {
        return new ByteArrayContent(bytes);
    }
}
