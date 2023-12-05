using BookShop.domain.Checkout;
using BookShop.domain.Receipt;
using BookShop.shared;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReceiptController : ControllerBase
{
    private readonly IProvideReceiptDetails _receiptDetailsProvider;
    public ReceiptController(IProvideReceiptDetails receiptDetailsProvider)
    {
        _receiptDetailsProvider = receiptDetailsProvider;
    }

    [HttpGet]
    [Route("{receiptId}")]
    public ReceiptResponse Get([FromRoute] Guid receiptId)
    {
        var (bookDetails, paidPrice) = _receiptDetailsProvider.Get(new ReceiptId(receiptId));
        var bookDetailsResponse = bookDetails.Select(detail => new ReceiptBookResponse(detail.Title, detail.Author, detail.PictureUrl?.ToString() ?? string.Empty, detail.OrderedQuantity, new Price(detail.UnitPrice.Amount, detail.UnitPrice.Currency))).ToArray();

        return new ReceiptResponse(bookDetailsResponse, new Price(paidPrice.Amount, paidPrice.Currency));
    }
}
