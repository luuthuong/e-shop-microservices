using Core.CQRS.Command;

namespace ProductSyncService.Application.Quotes.Commands.ConfirmQuote;

public record ConfirmQuoteCommand(Guid Id): ICommand;