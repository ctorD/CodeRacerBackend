using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeRacerBackend.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

            SnippetFinder snippetFinder = new SnippetFinder();


            var snippit = snippetFinder.getSnippet("javascript"); 


            return Ok(snippit);

        }

    }
}
