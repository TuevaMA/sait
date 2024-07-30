using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sait.domein.Entities;
using sait.domein.Abstract;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class CartController : Controller
    {
        private ClothesRepository repository;
        private IOrderProcessor orderProcessor;
        private ClothesRepository @object;

        public CartController(ClothesRepository repo, IOrderProcessor processor)
        {
            repository = repo;
            orderProcessor = processor;
        }

        public CartController(ClothesRepository @object)
        {
            this.@object = @object;
        }
        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

            [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }



        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int Id, string returnUrl)
        {
            Clothe clothe = repository.Clothes
                .FirstOrDefault(g => g.Id == Id);

            if (clothe != null)
            {
                cart.AddItem(clothe, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int Id, string returnUrl)
        {
            Clothe clothe = repository.Clothes
                .FirstOrDefault(g => g.Id == Id);

            if (clothe != null)
            {
                cart.RemoveLine(clothe);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
      
    }
    }

    