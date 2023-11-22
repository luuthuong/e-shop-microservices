using Core.CQRS.Command;
using Core.CQRS.Query;
using Core.Infrastructure.Api;

namespace API.Controllers;

public class PaymentMethodController: BaseController
{

    public PaymentMethodController(ICommandBus commandBus, IQueryBus queryBus) : base(commandBus, queryBus)
    {
    }
}