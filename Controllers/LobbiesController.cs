using CodeRacerBackend.Hubs.SignalRChat.Hubs;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CodeRacerBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LobbiesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetLobbies()
        {
            //Code to get new github snippet

            var lobbies = LobbyHub.Lobbies.ToArray().Where(e => e.MaxPlayers > 1);

            return Ok(lobbies);
        }
    }
}