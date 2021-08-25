using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChallengeWebApi.Controllers;
using ChallengeWebApi.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net.Http;
using System.Web.Http;
using ChallengeWebApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Web.Http.Results;

namespace ChallengeWebApi.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void GetOnePerson_ShouldReturnOnePerson()
        {
            var mock = new Mock<ILogger<HomeController>>();
            ILogger<HomeController> logger = mock.Object;
            var controller = new HomeController(logger);
            var dbOptions = new DbContextOptions<DataContext>();
            var context = new DataContext(dbOptions);
            Person person = new Person();
            person.BirthDate = "2020-08-02";
            person.Cpf = "09765432180";
            person.Email = "email@email.com";
            person.Gender = "Male";
            person.Id = 23;
            person.Name = "EMail";
            person.StartDate = "22/9999";
            context.Persons.Add(person);
            // Act
            var result = controller.Index(context);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
        }

        [TestMethod]
        public void GetAllPersons_ShouldReturnAllPersons()
        {
            var mock = new Mock<ILogger<HomeController>>();
            ILogger<HomeController> logger = mock.Object;
            var controller = new HomeController(logger);
            var dbOptions = new DbContextOptions<DataContext>();
            var context = new DataContext(dbOptions);
            Person person = new Person();
            person.BirthDate = "2020-08-02";
            person.Cpf = "09765432180";
            person.Email = "email@email.com";
            person.Gender = "Male";
            person.Id = 23;
            person.Name = "EMail";
            person.StartDate = "22/9999";
            Person personB = new Person();
            person.BirthDate = "2020-08-02";
            person.Cpf = "09765432180";
            person.Email = "email@email.com";
            person.Gender = "Male";
            person.Id = 2;
            person.Name = "EMail";
            person.StartDate = "22/9999";
            Person personC = new Person();
            person.BirthDate = "2020-08-02";
            person.Cpf = "09765432180";
            person.Email = "email@email.com";
            person.Gender = "Male";
            person.Id = 211;
            person.Name = "EMail";
            person.StartDate = "22/9999";
            context.Persons.Add(person);
            context.Persons.Add(personB);
            context.Persons.Add(personC);
            // Act
            var result = controller.Index(context);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
        }

        [TestMethod]
        public void GetNoPerson_ShouldReturnNoPerson()
        {
            var mock = new Mock<ILogger<HomeController>>();
            ILogger<HomeController> logger = mock.Object;
            var controller = new HomeController(logger);
            var dbOptions = new DbContextOptions<DataContext>();
            var context = new DataContext(dbOptions);
            // Act
            var result = controller.Index(context);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
        }
    }
}