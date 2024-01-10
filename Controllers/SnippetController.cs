using CodeRacerBackend.Utils;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CodeRacerBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SnippetController : ControllerBase
    {
        [HttpGet]
        public ActionResult<String> GetCodeSnippet()
        {
            //Code to get new github snippit

           // SnippetFinder snippetFinder = new SnippetFinder();

           var snippit = "";

            return Ok(snippit);
        }
    }
}