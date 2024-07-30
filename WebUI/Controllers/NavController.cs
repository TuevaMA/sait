using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sait.domein.Abstract;

namespace WebUI.Controllers
{
    public class NavController : Controller
    {
        private ClothesRepository repository;

        public NavController(ClothesRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.Clothes

                .Select(Clothes => Clothes.Category)
                .Distinct()
                .OrderBy(x => x);
            return PartialView("FlexMenu", categories);

        }

    }
}
