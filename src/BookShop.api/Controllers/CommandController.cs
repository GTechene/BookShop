using BookShop.domain.Checkout;
using BookShop.domain.Receipt;
using BookShop.shared;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommandController : ControllerBase
{
    private readonly IProvideReceiptDetails _receiptDetailsProvider;
    public CommandController(IProvideReceiptDetails receiptDetailsProvider)
    {
        _receiptDetailsProvider = receiptDetailsProvider;
    }

    [HttpGet]
    [Route("{commandId}")]
    public CommandResponse GetCommand([FromRoute] Guid commandId)
    {
        var (bookDetails, paidPrice) = _receiptDetailsProvider.Get(new ReceiptId(commandId));
        var bookDetailsResponse = bookDetails.Select(detail => new ReceiptBookResponse(detail.Title, detail.Author, detail.PictureUrl?.ToString() ?? string.Empty, detail.OrderedQuantity, new Price(detail.UnitPrice.Amount, detail.UnitPrice.Currency))).ToArray();

        return new CommandResponse(bookDetailsResponse, new Price(paidPrice.Amount, paidPrice.Currency));
    }
}
