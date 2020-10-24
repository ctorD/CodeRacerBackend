using CodeRacerBackend.CodeRacerLogic;
using CodeRacerBackend.Hubs.SignalRChat.Hubs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            var lobbies = LobbyHub.lobbies.ToArray();

            return Ok(lobbies);

        }

    }
}

