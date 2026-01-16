using System.Text.Json.Serialization;

namespace CourseProject_SellingTickets.Models.JsonAbstractions;

public record FreeImageResponse(
    [property: JsonPropertyName("image")] FreeImageData Image,
    [property: JsonPropertyName("status_code")] int StatusCode
);