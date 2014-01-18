using System.Web.Mvc;

namespace Example.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("Index");
        }
    }
}