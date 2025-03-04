using Core.CQRS.Command;
using Core.EF;
using Core.Exception;
using ProductSyncService.Domain.Quotes;

namespace ProductSyncService.Application.Quotes.Commands.ConfirmQuote;

public class ConfirmQuoteCommandHandler(
    IUnitOfWork unitOfWork, IQuoteRepository quoteRepository): ICommandHandler<ConfirmQuoteCommand>
{
    public async Task Handle(ConfirmQuoteCommand request, CancellationToken cancellationToken)
    {
        var quote = await quoteRepository.GetByIdAsync(QuoteId.From(request.Id), cancellationToken);

        if (quote is null)
        {
            throw new DomainLogicException("Quote not found.");
        }
        
        quote.Confirm();
        await unitOfWork.SaveChangeAsync(cancellationToken);
    }
}