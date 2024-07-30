using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using sait.domein.Abstract;
using sait.domein.Entities;
using WebUI.Controllers;
using System.Web.Mvc;

namespace Utest
{
    public class AdminTests
    {
        public void Index_Contains_All_Clothes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<ClothesRepository> mock = new Mock<ClothesRepository>();
            mock.Setup(m => m.Clothes).Returns(new List<Clothe>
            {
                new Clothe { Id = 1, Name = "Вещь1"},
                new Clothe { Id = 2, Name = "Вещь2"},
                new Clothe { Id = 3, Name = "Вещь3"},
                new Clothe { Id = 4, Name = "Вещь4"},
                new Clothe { Id = 5, Name = "Вещь5"},
                new Clothe { Id = 6, Name = "Вещь6"},
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            List<Clothe> result = ((IEnumerable<Clothe>)controller.Index().
                ViewData.Model).ToList();

            // Утверждение
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Вещь1", result[0].Name);
            Assert.AreEqual("Вещь2", result[1].Name);
            Assert.AreEqual("Вещь3", result[2].Name);
        }
        public void Can_Edit_Clothes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<ClothesRepository> mock = new Mock<ClothesRepository>();
            mock.Setup(m => m.Clothes).Returns(new List<Clothe>
    {
        new Clothe { Id = 1, Name = "Вещь1"},
        new Clothe { Id = 2, Name = "Вещь2"},
        new Clothe { Id = 3, Name = "Вещь3"},
        new Clothe { Id = 4, Name = "Вещь4"},
        new Clothe { Id = 5, Name = "Вещь5"},
        new Clothe { Id = 6, Name = "Вещь6"}
    });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Clothe clothes1 = controller.Edit(1).ViewData.Model as Clothe;
            Clothe clothes2 = controller.Edit(2).ViewData.Model as Clothe;
            Clothe clothes3 = controller.Edit(3).ViewData.Model as Clothe;

            // Assert
            Assert.AreEqual(1, clothes1.Id);
            Assert.AreEqual(2, clothes2.Id);
            Assert.AreEqual(3, clothes3.Id);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Clothes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<ClothesRepository> mock = new Mock<ClothesRepository>();
            mock.Setup(m => m.Clothes).Returns(new List<Clothe>
    {
        new Clothe { Id = 1, Name = "Вещь1"},
        new Clothe { Id = 2, Name = "Вещь2"},
        new Clothe { Id = 3, Name = "Вещь3"},
        new Clothe { Id = 4, Name = "Вещь4"},
        new Clothe { Id = 5, Name = "Вещь5"},
        new Clothe { Id = 6, Name = "Вещь6"}
    });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Clothe result = controller.Edit(6).ViewData.Model as Clothe;

            // Assert
        }
        public void Can_Save_Valid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<ClothesRepository> mock = new Mock<ClothesRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            Clothe clothe = new Clothe { Name = "Test" };

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(clothe);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SaveClothes(clothe));

            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<ClothesRepository> mock = new Mock<ClothesRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            Clothe clothe = new Clothe { Name = "Test" };

            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(clothe);

            // Утверждение - проверка того, что обращение к хранилищу НЕ производится 
            mock.Verify(m => m.SaveClothes(It.IsAny<Clothe>()), Times.Never());

            // Утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
    

