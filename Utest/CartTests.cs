using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using sait.domein.Abstract;
using sait.domein.Entities;
using WebUI.Controllers;
using WebUI.Models;

namespace Utest
{
    public class CartTests
    {
        public void Can_Add_New_Lines()
        {
            // Организация - создание нескольких тестовых игр
            Clothe clothes1 = new Clothe { Id = 1, Name = "Вещь1" };
            Clothe clothes2 = new Clothe { Id = 2, Name = "Вещь2" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(clothes1, 1);
            cart.AddItem(clothes2, 1);
            List<CartLine> results = cart.Lines.ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Clothe, clothes1);
            Assert.AreEqual(results[1].Clothe, clothes2);
        }
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Организация - создание нескольких тестовых игр
            Clothe clothes1 = new Clothe { Id = 1, Name = "Вещь1" };
            Clothe clothes2 = new Clothe { Id = 2, Name = "Вещь2" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(clothes1, 1);
            cart.AddItem(clothes2, 1);
            cart.AddItem(clothes1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Clothe.Id).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);    // 6 экземпляров добавлено в корзину
            Assert.AreEqual(results[1].Quantity, 1);
        }
        public void Can_Remove_Line()
        {
            // Организация - создание нескольких тестовых игр
            Clothe clothes1 = new Clothe { Id = 1, Name = "Вещь1" };
            Clothe clothes2 = new Clothe { Id = 2, Name = "Вещь2" };
            Clothe clothes3 = new Clothe { Id = 3, Name = "Вещь3" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - добавление нескольких игр в корзину
            cart.AddItem(clothes1, 1);
            cart.AddItem(clothes2, 4);
            cart.AddItem(clothes3, 2);
            cart.AddItem(clothes2, 1);

            // Действие
            cart.RemoveLine(clothes2);

            // Утверждение
            Assert.AreEqual(cart.Lines.Where(c => c.Clothe == clothes2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }
        public void Calculate_Cart_Total()
        {
            // Организация - создание нескольких тестовых игр
            Clothe clothes1 = new Clothe { Id = 1, Name = "Вещь1", Price = 100 };
            Clothe clothes2 = new Clothe { Id = 2, Name = "Вещь2", Price = 55 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(clothes1, 1);
            cart.AddItem(clothes2, 1);
            cart.AddItem(clothes1, 5);
            decimal result = cart.ComputeTotalValue();

            // Утверждение
            Assert.AreEqual(result, 655);
        }
        public void Can_Clear_Contents()
        {
            // Организация - создание нескольких тестовых игр
            Clothe clothes1 = new Clothe { Id = 1, Name = "Вещь1", Price = 100 };
            Clothe clothes2 = new Clothe { Id = 2, Name = "Вещь2", Price = 55 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(clothes1, 1);
            cart.AddItem(clothes2, 1);
            cart.AddItem(clothes1, 5);
            cart.Clear();

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 0);
        }
        public void Can_Add_To_Cart()
        {
            // Организация - создание имитированного хранилища
            Mock<ClothesRepository> mock = new Mock<ClothesRepository>();
            mock.Setup(m => m.Clothes).Returns(new List<Clothe> {
             new Clothe {Id = 1, Name = "Вещь1", Category = "Платья"},
        }.AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object);

            // Действие - добавить игру в корзину
            controller.AddToCart(cart, 1, null);

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Clothe.Id, 1);
        }

        /// <summary>
        /// После добавления игры в корзину, должно быть перенаправление на страницу корзины
        /// </summary>
        [TestMethod]
        public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
        {
            // Организация - создание имитированного хранилища
            Mock<ClothesRepository> mock = new Mock<ClothesRepository>();
            mock.Setup(m => m.Clothes).Returns(new List<Clothe> {
            new Clothe {Id = 1, Name = "Вещь1", Category = "Платья"},
         }.AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object);

            // Действие - добавить игру в корзину
            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            // Утверждение
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        // Проверяем URL
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController target = new CartController(null);

            // Действие - вызов метода действия Index()
            CartIndexViewModel result
                = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Утверждение
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
        public void Cannot_Checkout_Empty_Cart()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация - создание пустой корзины
            Cart cart = new Cart();

            // Организация - создание деталей о доставке
            ShippingDetails shippingDetails = new ShippingDetails();

            // Организация - создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие
            ViewResult result = controller.Checkout(cart, shippingDetails);

            // Утверждение — проверка, что заказ не был передан обработчику 
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение — проверка, что метод вернул стандартное представление 
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Clothe(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Организация — добавление ошибки в модель
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ не передается обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение - проверка, что метод вернул стандартное представление
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }
        public void Can_Checkout_And_Submit_Order()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Clothe(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ передан обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());

            // Утверждение - проверка, что метод возвращает представление 
            Assert.AreEqual("Completed", result.ViewName);

            // Утверждение - проверка, что представлению передается допустимая модель
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }


    }
}
