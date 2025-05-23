﻿using MediatR;

namespace OrderManagement.Application.Commands;

public class ConfirmOrderPaymentCommand : IRequest
{
    public Guid OrderId { get; set; }
    public Guid PaymentId { get; set; }
    public string TransactionId { get; set; }
}