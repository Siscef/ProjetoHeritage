using System;
using Heritage.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HeritageTeste.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange 
            HomeController Home = new HomeController();

            //Act 
            ViewResult result = Home.Index() as ViewResult;

            //Assert 
            Assert.IsNotNull(result);
        }
    }
}
