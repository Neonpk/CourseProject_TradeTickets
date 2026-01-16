using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CourseProject_SellingTickets.Interfaces.Common;
using CourseProject_SellingTickets.Interfaces.Factories;
using CourseProject_SellingTickets.Interfaces.FreeImageServiceInterface;
using CourseProject_SellingTickets.Models.Common;
using CourseProject_SellingTickets.Models.JsonAbstractions;

namespace CourseProject_SellingTickets.Services.FreeImageService;

public class FreeImageService : IFreeImageService
{
    private IHttpClientFactory _httpClientFactory;
    private string _apiKey;
    private const string ApiUrl = "https://freeimage.host/api/1/upload";
    
    public FreeImageService(IHttpClientFactory httpClientFactory, string apiKey)
    {
        _httpClientFactory = httpClientFactory;
        _apiKey = apiKey;
    }
    
    public async Task<IResult<string>> UploadImageAsync(byte[] byteImages, string fileName)
    {
        using (var httpClient = _httpClientFactory.CreateHttpClient())
        {
            try
            {
                var imageContent = _httpClientFactory.CreateByteArrayContent(byteImages);
                
                using var content = _httpClientFactory.CreateMultiPartFormDataContent();
                content.Add(_httpClientFactory.CreateStringContent(_apiKey), "key");
                content.Add(_httpClientFactory.CreateStringContent("upload"), "action");
                content.Add(imageContent, "source", fileName);
                
                var response = await httpClient.PostAsync(ApiUrl, content);

                if (!response.IsSuccessStatusCode)
                    return Result<string>.Failure($"API Error: {response.StatusCode}");

                var data = await response.Content.ReadFromJsonAsync<FreeImageResponse>();

                if (data?.Image.Url == null)
                    return Result<string>.Failure("Invalid response format");

                return Result<string>.Success(data.Image.Url);
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(ex.Message);
            }
        }
    }
}
