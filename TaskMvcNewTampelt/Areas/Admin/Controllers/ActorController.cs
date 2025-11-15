using Microsoft.AspNetCore.Mvc;

namespace TaskMvcNewTampelt.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        public IActionResult Index() => View();
    }
}
