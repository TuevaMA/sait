using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sait.domein.Abstract;
using sait.domein.Entities;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class ClothesController : Controller
    {
        // GET: Clothes
        private ClothesRepository repository;
        public int pageSize = 4;
        public ClothesController(ClothesRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string category, int page = 1)
        {
            ClotheListViewModel model = new ClotheListViewModel
            {
                Clothes = repository.Clothes
                 .Where(p => category == null || p.Category == category)
                    .OrderBy(clothe => clothe.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                repository.Clothes.Count() :
                repository.Clothes.Where(clothes => clothes.Category == category).Count()
                },
                CurrentCategory = category

            };
            return View(model);
        }
        public FileContentResult GetImage(int Id)
        {
            Clothe clothe = repository.Clothes
                .FirstOrDefault(g => g.Id == Id);

            if (clothe != null)
            {
                return File(clothe.ImageData, clothe.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}