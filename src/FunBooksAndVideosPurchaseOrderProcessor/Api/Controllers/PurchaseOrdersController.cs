using Api.Filters;
using Application.Interfaces.Services;
using Application.Models.Inputs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace FunBooksAndVideos.PurchaseOrderProcessor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseOrdersController : ControllerBase
{
    private readonly IPurchaseOrderService _purchaseOrderService;

    public PurchaseOrdersController(IPurchaseOrderService purchaseOrderService)
    {
        _purchaseOrderService = purchaseOrderService;
    }

    /// <summary>
    /// Process a purchase order
    /// </summary>
    [HttpPost]
    [SwaggerRequestExample(typeof(PurchaseOrderDto), typeof(PurchaseOrderExample))]
    public async Task<IActionResult> Process([FromBody] PurchaseOrderDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _purchaseOrderService.ProcessAsync(request);

        return result.IsSuccess? Ok() : BadRequest(result.Error); 
    }
}
