using API.Requests;
using Application.Customers.Commands;
using Core.CQRS.Command;
using Core.CQRS.Query;
using Core.Infrastructure.Api;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CustomerController : BaseController
{
    public CustomerController(ICommandBus commandBus, IQueryBus queryBus) : base(commandBus, queryBus)
    {
    }


    [HttpPost("create")]
    public Task<IActionResult> Create(CreateCustomerRequest request)
    {
        return Response(
            CreateCustomer.Create(
                request.Email,
                request.Password,
                request.PasswordConfirm,
                request.Name,
                request.ShippingAddress
            )
        );
    }
}