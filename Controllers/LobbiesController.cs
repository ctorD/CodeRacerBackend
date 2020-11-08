using CodeRacerBackend.Hubs.SignalRChat.Hubs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CodeRacerBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LobbiesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<String> GetLobbies()
        {
            //Code to get new github snippit

            var lobbies = LobbyHub.lobbies.ToArray().Where(e => e.MaxPlayers > 1);

            return Ok(lobbies);
        }
    }
}