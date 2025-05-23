﻿using MediatR;

namespace OrderManagement.Application.Commands;

public class ReportOutOfStockCommand : IRequest
{
    public Guid OrderId { get; set; }
    public List<Guid> OutOfStockProductIds { get; set; } = new List<Guid>();
}