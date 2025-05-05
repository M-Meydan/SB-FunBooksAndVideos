using Domain.Interfaces;
using FunBooksAndVideos.PurchaseOrderProcessor.Application.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Consumers
{

    public class ShippingSlipGenerationConsumer : IConsumer<GenerateShippingSlipMessage>
    {
        private readonly IShippingSlipService _shippingService;
        private readonly ILogger<ShippingSlipGenerationConsumer> _logger;

        public ShippingSlipGenerationConsumer(
           IShippingSlipService shippingService,
            ILogger<ShippingSlipGenerationConsumer> logger)
        {
            _shippingService = shippingService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<GenerateShippingSlipMessage> context)
        {
            var message = context.Message;

            try
            {

                await _shippingService.GenerateShippingSlipAsync(message.puchaseOrderId);
                
                //Publish shipping slip generated event
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to generate shipping slip for order {message.puchaseOrderId}");

                //Publish shipping slip generation failed event
                throw;
            }
        }
    }
}
