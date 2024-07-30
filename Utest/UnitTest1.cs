using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using sait.domein.Abstract;
using sait.domein.Entities;
using WebUI.Controllers;
using WebUI.Models;
using WebUI.HtmlHelpers;


namespace Utest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<ClothesRepository> mock = new Mock<ClothesRepository>();
            mock.Setup(m => m.Clothes).Returns(new List<Clothe>
    {
        new Clothe { Id = 1, Name = "Вещь1"},
        new Clothe { Id = 2, Name = "Вещь2"},
        new Clothe { Id = 3, Name = "Вещь3"},
        new Clothe { Id = 4, Name = "Вещь4"},
        new Clothe { Id = 5, Name = "Вещь5"},
        new Clothe { Id = 5, Name = "Вещь5"},
        new Clothe { Id = 6, Name = "Вещь6"},

    });
            ClothesController controller = new ClothesController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            ClotheListViewModel result = (ClotheListViewModel)controller.List(null, 2).Model;

            // Утверждение
            List<Clothe> clothes = result.Clothes.ToList();
            Assert.IsTrue(clothes.Count == 2);
            Assert.AreEqual(clothes[0].Name, "Вещь4");
            Assert.AreEqual(clothes[1].Name, "Вщь5");
        }
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());

        }
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
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
            ClothesController controller = new ClothesController(mock.Object);
            controller.pageSize = 3;

            // Act
            ClotheListViewModel result
                = (ClotheListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }
        public void Can_Filter_Clothes()
        {
            // Организация (arrange)
            Mock<ClothesRepository> mock = new Mock<ClothesRepository>();
            mock.Setup(m => m.Clothes).Returns(new List<Clothe>
    {
        new Clothe { Id = 1, Name = "Вещь1", Category="Платья"},
        new Clothe { Id = 2, Name = "Вещь2", Category="Худи"},
        new Clothe { Id = 3, Name = "Вещь3", Category="Свитера"},
        new Clothe { Id = 4, Name = "Вещь4", Category="Футболки"},
        new Clothe { Id = 5, Name = "Вещь5", Category="Джинсы"},
        new Clothe { Id = 6, Name = "Вещь6", Category="Куртки" },

    });
            ClothesController controller = new ClothesController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Clothe> result = ((ClotheListViewModel)controller.List("Cat2", 1).Model)
                .Clothes.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Вещь2" && result[0].Category == "Худи");
            Assert.IsTrue(result[1].Name == "Вещь4" && result[1].Category == "Футболки");

        }
        public void Can_Create_Categories()
        {
            // Организация - создание имитированного хранилища
            Mock<ClothesRepository> mock = new Mock<ClothesRepository>();
            mock.Setup(m => m.Clothes).Returns(new List<Clothe> {
        new Clothe { Id = 1, Name = "Вещь1", Category="Платья"},
        new Clothe { Id = 2, Name = "Вещь2", Category="Худи"},
        new Clothe { Id = 3, Name = "Вещь3", Category="Свитера"},
        new Clothe { Id = 4, Name = "Вещь4", Category="Футболки"},
        new Clothe { Id = 5, Name = "Вещь5", Category="Джинсы"},
        new Clothe { Id = 6, Name = "Вещь6", Category="Куртки" },

    });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "Джинсы");
            Assert.AreEqual(results[1], "Куртки");
            Assert.AreEqual(results[2], "Платья");
            Assert.AreEqual(results[3], "Свитера");
            Assert.AreEqual(results[4], "Футболки");
            Assert.AreEqual(results[5], "Худи");
        }
        public void Generate_Category_Specific_Clothes_Count()
        {
            /// Организация (arrange)
            Mock<ClothesRepository> mock = new Mock<ClothesRepository>();
            mock.Setup(m => m.Clothes).Returns(new List<Clothe>
    {
        new Clothe { Id = 1, Name = "Вещь1", Category="Платья"},
        new Clothe { Id = 2, Name = "Вещь2", Category="Худи"},
        new Clothe { Id = 3, Name = "Вещь3", Category="Свитера"},
        new Clothe { Id = 4, Name = "Вещь4", Category="Футболки"},
        new Clothe { Id = 5, Name = "Вещь5", Category="Джинсы"},
        new Clothe { Id = 6, Name = "Вещь6", Category="Куртки" }
    });
            ClothesController controller = new ClothesController(mock.Object);
            controller.pageSize = 3;

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((ClotheListViewModel)controller.List("Платья").Model).PagingInfo.TotalItems;
            int res2 = ((ClotheListViewModel)controller.List("Худи").Model).PagingInfo.TotalItems;
            int res3 = ((ClotheListViewModel)controller.List("Свитера").Model).PagingInfo.TotalItems;
            int res4 = ((ClotheListViewModel)controller.List("Футболки").Model).PagingInfo.TotalItems;
            int res5 = ((ClotheListViewModel)controller.List("Джинсы").Model).PagingInfo.TotalItems;
            int res6 = ((ClotheListViewModel)controller.List("Куртки").Model).PagingInfo.TotalItems;
            int resAll = ((ClotheListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
        namespace UnitTests1
    {
        [TestClass]
        public class ImageTests
        {
            [TestMethod]
            public void Can_Retrieve_Image_Data()
            {
                // Организация - создание объекта Game с данными изображения
                Clothe clothe = new Clothe
                {
                    Id = 2,
                    Name = "вещь2",
                    ImageData = new byte[] { },
                    ImageMimeType = "image/png"
                };

                // Организация - создание имитированного хранилища
                Mock<ClothesRepository> mock = new Mock<ClothesRepository>();
                mock.Setup(m => m.Clothes).Returns(new List<Clothe> {
                new Clothe {Id = 1, Name = "вещь1"},
                clothe,
                new Clothe {Id = 3, Name = "вещь3"}
            }.AsQueryable());

                // Организация - создание контроллера
                ClothesController controller = new ClothesController(mock.Object);

                // Действие - вызов метода действия GetImage()
                ActionResult result = controller.GetImage(2);

                // Утверждение
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(FileResult));
                Assert.AreEqual(clothe.ImageMimeType, ((FileResult)result).ContentType);
            }

            [TestMethod]
            public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
            {
                // Организация - создание имитированного хранилища
                Mock<ClothesRepository> mock = new Mock<ClothesRepository>();
                mock.Setup(m => m.Clothes).Returns(new List<Clothe> {
                new Clothe {Id = 1, Name = "вещь1"},
                new Clothe {Id = 2, Name = "вещь2"}
            }.AsQueryable());

                // Организация - создание контроллера
                ClothesController controller = new ClothesController(mock.Object);

                // Действие - вызов метода действия GetImage()
                ActionResult result = controller.GetImage(10);

                // Утверждение
                Assert.IsNull(result);
            }
        }
    }

}

