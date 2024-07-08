using System.Web.Mvc;

namespace HyperConvert.Controllers
{
    public class APIlogController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "API Log info";

            return View();
        }
    }
}
