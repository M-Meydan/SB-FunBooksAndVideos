using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Models.Inputs;

/// <summary>
/// Data Transfer Object for Purchase Order.
/// </summary>
public class PurchaseOrderDto
{
    [Required(ErrorMessage = "PurchaseOrderId is required.")]
    [Range(1, long.MaxValue, ErrorMessage = "PurchaseOrderId must be greater than 0.")]
    [JsonPropertyName("purchaseOrder")]
    public long PurchaseOrderId { get; set; }

    [Required(ErrorMessage = "Total is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Total must be greater than or equal to 0.")]
    public decimal Total { get; set; }

    [Required(ErrorMessage = "CustomerId is required.")]
    [Range(1, long.MaxValue, ErrorMessage = "CustomerId must be greater than 0.")]
    [JsonPropertyName("customer")]
    public long CustomerId { get; set; }

    [Required(ErrorMessage = "ItemLines are required.")]
    [MinLength(1, ErrorMessage = "At least one ItemLine is required.")]
    public List<string> ItemLines { get; set; } = new List<string>();
}


