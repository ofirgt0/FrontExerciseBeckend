using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FrontendExercise
{
    [Route("api/Name")]
    public class HomeController : Controller
    {
        private static string Name;

        [HttpGet]
        public async Task<IActionResult> GetName()
        {
            return Ok(Name);
        }

        [HttpPost]
        [Route("{newName}")]
        public async Task<IActionResult> SetName(string newName)
        {
            Name = newName;
            return Ok();
        }

    }
}