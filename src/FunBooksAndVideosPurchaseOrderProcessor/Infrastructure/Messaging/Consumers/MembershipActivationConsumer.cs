using FunBooksAndVideos.PurchaseOrderProcessor.Application.Events;
using Infrastructure.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Consumers
{
    public class MembershipActivationConsumer : IConsumer<ActivateMembershipMessage>
    {
        private readonly MembershipActivationService _membershipService;
        private readonly ILogger<MembershipActivationConsumer> _logger;

        public MembershipActivationConsumer(MembershipActivationService membershipService,
            ILogger<MembershipActivationConsumer> logger)
        {
            _membershipService = membershipService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ActivateMembershipMessage> context)
        {
            try
            {
                await _membershipService.ActivateMembershipsAsync(context.Message.puchaseOrderId);

                //Publish membership activation completed event
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to activate memberships for customer {context.Message.puchaseOrderId}");

                //Publish membership activation failed event
                throw;
            }
        }
    }
}
