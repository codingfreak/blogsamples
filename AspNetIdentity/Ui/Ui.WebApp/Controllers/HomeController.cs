namespace codingfreaks.AspNetIdentity.Ui.WebApp.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Controller for all views inside the /Home path.
    /// </summary>
    public class HomeController : Controller
    {
        #region methods

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        #endregion
    }
}