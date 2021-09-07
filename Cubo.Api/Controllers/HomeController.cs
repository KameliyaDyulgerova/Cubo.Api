using Cubo.Api.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cubo.Controllers
{
    public class HomeController : Controller
    {
        private AppSettings _settings;

        public HomeController(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        [HttpGet("")]
        public IActionResult Get()
        => Content($"Welcome to the Accountant API! [{_settings.AppEnv}]");
    }
}