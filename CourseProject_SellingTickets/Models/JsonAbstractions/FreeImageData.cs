using System.Text.Json.Serialization;

namespace CourseProject_SellingTickets.Models.JsonAbstractions;

public record FreeImageData(
    [property: JsonPropertyName("url")] string Url
);
