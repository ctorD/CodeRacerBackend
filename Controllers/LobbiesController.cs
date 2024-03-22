using System.Linq;
using CodeRacerBackend.Hubs;
using Microsoft.AspNetCore.Mvc;

namespace CodeRacerBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LobbiesController : ControllerBase
{
    private readonly ILobbyManager lobbyManager;

    public LobbiesController(ILobbyManager lobbyManager)
    {
        this.lobbyManager = lobbyManager;
    }

    [HttpGet]
    public ActionResult<string> GetLobbies()
    {
        var lobbies = lobbyManager.GetAllLobbies().ToArray().Where(e => e.MaxPlayers > 1);

        return Ok(lobbies);
    }
}