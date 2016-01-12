using GeocachingExercise.Controllers;
using GeocachingExercise.Models;
using GeocachingExercise.Persistence.EF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Test.Controllers.UnitTests
{
    public class WhenWorkingWithTheGeocachesController
    {
        protected Mock<IGeocacheRepository> geocacheRepository;
        protected GeocachesController controller;
        protected ActionResult result;

        public virtual void Arrange()
        {
            geocacheRepository = new Mock<IGeocacheRepository>();

            controller = new GeocachesController(geocacheRepository.Object);
            
        }

        /// <summary>
        /// Manually sets Model State
        /// Calling controller directly bypasses model binder
        /// Without calling this method, ModelState.IsValid will pass for invalid models
        /// </summary>
        /// <param name="controller">GeocachesController to change model state</param>
        /// <param name="model">Geocache object to validate</param>
        public void ValidateModel(ref GeocachesController controller, object model)
        {
            controller.ModelState.Clear();

            List<ValidationResult> validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(model, new ValidationContext(model, null, null), validationResults, true);

            foreach (ValidationResult result in validationResults)
            {
                foreach (string name in result.MemberNames)
                {
                    // No model binder when calling controller directly, must manually set invalid Model State
                    controller.ModelState.AddModelError(name, result.ErrorMessage);
                }
            }
        }
    }

    #region Index
    [TestClass]
    public class WhenCallingIndexWithGeocachesInRepository : WhenWorkingWithTheGeocachesController
    {
        public override void Arrange()
        {
            base.Arrange();

            // Get all geocaches:
            geocacheRepository.Setup(x => x.All()).Returns(new List<Geocache> {
                new Geocache { Name = "Cache 1", Coordinate = new Coordinate(0.0, 0.0) },
                new Geocache { Name = "Cache 2", Coordinate = new Coordinate(1.0, 0.0) }
            });
        }
        [TestMethod]
        public void ThenTheIndexMethodShouldReturnListOfGeocachesAsTheViewModel()
        {
            // Arrange
            Arrange();

            // Act 
            result = controller.Index();

            // Assert
            var viewResult = result as ViewResult;
            var viewModel = viewResult.Model;

            Assert.AreEqual(typeof(List<Geocache>),viewModel.GetType());
            Assert.AreEqual(2, (viewModel as List<Geocache>).Count);
        }
    }
    #endregion

    #region Details
    [TestClass]
    public class WhenCallingDetailsWithValidId : WhenWorkingWithTheGeocachesController
    {
        public override void Arrange()
        {
            base.Arrange();

            // Find geocache by Id:
            geocacheRepository.Setup(x => x.Find(1)).Returns(new Geocache { Name = "Name", Coordinate = new Coordinate(1.0, 0.0) });
        }

        [TestMethod]
        public void ThenTheDetailsMethodShouldReturnASingleNonNullGeocacheAsTheViewModel()
        {
            // Arrange
            Arrange();

            // Act
            result = controller.Details(1);

            // Assert
            var viewResult = result as ViewResult;
            var viewModel = viewResult.Model;

            Assert.AreEqual(typeof(Geocache), viewModel.GetType());
            Assert.IsNotNull(viewModel);
            Assert.AreEqual("Name", (viewModel as Geocache).Name);
            Assert.AreEqual(1.0, (viewModel as Geocache).Coordinate.Latitude);
            Assert.AreEqual(0.0, (viewModel as Geocache).Coordinate.Longitude);
        }
    }

    [TestClass]
    public class WhenCallingDetailsWithInvalidId : WhenWorkingWithTheGeocachesController
    {
        public override void Arrange()
        {
            base.Arrange();

            // Geocache does not exist:
            geocacheRepository.Setup(x => x.Find(It.IsAny<int>())).Returns((Geocache)null);
        }

        [TestMethod]
        public void ThenTheDetailsMethodShouldReturnANotFoundStatusCode()
        {
            // Arrange
            Arrange();

            // Act
            result = controller.Details(1);

            // Assert
            var httpCode = result as HttpStatusCodeResult;

            Assert.AreEqual(404, httpCode.StatusCode);
        }
    }

    [TestClass]
    public class WhenCallingDetailsWithNullId : WhenWorkingWithTheGeocachesController
    {
        [TestMethod]
        public void ThenTheDetailsMethodhouldReturnABadRequestStatusCode()
        {
            // Arrange
            Arrange();

            // Act
            result = controller.Details(null);

            // Assert
            var httpCode = result as HttpStatusCodeResult;

            Assert.AreEqual(400, httpCode.StatusCode);
        }
    }
    #endregion

    #region Create
    [TestClass]
    public class WhenCallingCreateWithInvalidGeocacheObject : WhenWorkingWithTheGeocachesController
    {
        protected Geocache cache;

        public override void Arrange()
        {
            base.Arrange();
            cache = new Geocache { Name = string.Empty, Coordinate = new Coordinate(1000.0, -999) }; 
        }

        [TestMethod]
        public void ThenTheCreateMethodShouldNotCreateNorSaveANullGeocacheModel()
        {
            // Arrange
            Arrange();

            // Act
            ValidateModel(ref controller, cache);
            result = controller.Create(cache);

            // Assert
            geocacheRepository.Verify(x => x.Create(It.IsAny<Geocache>()), Times.Never());
            geocacheRepository.Verify(x => x.Save(), Times.Never());

        }
    }

    [TestClass]
    public class WhenCallingCreateWithValidGeocacheObject : WhenWorkingWithTheGeocachesController
    {
        protected Geocache cache;

        public override void Arrange()
        {
            base.Arrange();
            cache = new Geocache { Name = "Geocache 500", Coordinate = new Coordinate(-89.0, 112.0) };
        }

        [TestMethod]
        public void ThenTheCreateMethodShouldCreateAndSaveAValidGeocache()
        {
            // Arrange
            Arrange();

            // Act
            ValidateModel(ref controller, cache);
            result = controller.Create(cache);

            // Assert
            var viewResult = result as ViewResult;

            geocacheRepository.Verify(x => x.Create(It.IsAny<Geocache>()), Times.Once());
            geocacheRepository.Verify(x => x.Save(), Times.Once());          

        }
    }
    #endregion


}
