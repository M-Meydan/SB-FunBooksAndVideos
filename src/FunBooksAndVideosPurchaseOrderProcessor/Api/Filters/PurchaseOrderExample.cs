using Swashbuckle.AspNetCore.Filters;

namespace Api.Filters;

    /// <summary>
    /// Provides an example of a purchase order for Swagger documentation.
    /// </summary>
    public class PurchaseOrderExample : IExamplesProvider<PurchaseOrderExampleDto>
    {
        public PurchaseOrderExampleDto GetExamples()
        {
            return new PurchaseOrderExampleDto
            {
                PurchaseOrder = 3344656,
                Total = 48.50m,
                Customer = 4567890,
                ItemLines = new List<string>
            {
                "Video \"Comprehensive First Aid Training\"",
                "Book \"The Girl on the train\"",
                "Book Club Membership"
            }
            };
        }
    }

/// <summary>
/// Matches the RAW JSON structure before conversion
/// </summary>
public class PurchaseOrderExampleDto
{
    public long PurchaseOrder { get; set; }
    public decimal Total { get; set; }
    public long Customer { get; set; }
    public List<string> ItemLines { get; set; }
}
