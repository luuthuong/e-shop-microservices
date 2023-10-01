using Application.CQRS.PaymentMethods.Commands;
using Application.DTOs;
using Core.BaseController;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PaymentMethodController: BaseController
{
    public PaymentMethodController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(AddPaymentMethodRequest request)
    {
        var result = await Mediator.Send(new AddPaymentMethodCommand(request));
        return Ok(result);
    } 
}