using BookShop.shared;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommandController : ControllerBase
{
    private readonly CommandService _commandService;

    public CommandController(CommandService commandService)
    {
        _commandService = commandService;
    }

    [HttpGet]
    [Route("{commandId}")]
    public async Task<CommandResponse> GetCommand([FromRoute] Guid commandId)
    {
        return await _commandService.Get(commandId);
    }
}

// TODO : put in Domain layer and read data from TransactionLog
public class CommandService
{
    public async Task<CommandResponse> Get(Guid commandId)
    {
        return new CommandResponse(new[]
        {
            new BookResponse("12", "Encaisse", "René", "", 1, new Price(2, "EUR")),
            new BookResponse("12", "Encaisse", "René", "", 1, new Price(2, "EUR")),
            new BookResponse("3", "Encaisse 2 le retour", "René", "", 1, new Price(2, "EUR")),
        }, 
            new Price(12m, "USD"));
    }
}