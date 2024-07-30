using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sait.domein.Abstract;
using sait.domein.Entities;


namespace WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        ClothesRepository repository;


        public AdminController(ClothesRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Clothes);
        }
        public ViewResult Edit(int id)
        {
            Clothe clothes = repository.Clothes
                 .FirstOrDefault(g => g.Id == id);
            return View(clothes);

        }
        // Перегруженная версия Edit() для сохранения изменений
        [HttpPost]
        public ActionResult Edit(Clothe clothe, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    clothe.ImageMimeType = image.ContentType;
                    clothe.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(clothe.ImageData, 0, image.ContentLength);
                }
                repository.SaveClothes(clothe);
                TempData["message"] = string.Format("Изменения в одежде \"{0}\" были сохранены", clothe.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(clothe);
            }
        }
        public ViewResult Create()
        {
            return View("Edit", new Clothe());
        }
        public ActionResult Delete(int id)
        {
            Clothe deletedClothe = repository.DeleteClothe(id);
            if (deletedClothe != null)
            {
                TempData["message"] = string.Format("Вещь \"{0}\" была удалена",
                    deletedClothe.Name);
            }
            return RedirectToAction("Index");
        }
    } }





