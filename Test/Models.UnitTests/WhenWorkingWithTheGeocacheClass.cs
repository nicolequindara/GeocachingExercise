using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

using GeocachingExercise.Models;

namespace Test.Models
{
    public class WhenWorkingWithTheGeocacheClass : ModelValidator
    {
        protected Geocache cache;
        protected List<ValidationResult> validationResults;
    }

    [TestClass]
    public class WhenCreatingGeocacheObjectWithInvalidName : WhenWorkingWithTheGeocacheClass
    {
        [TestMethod]
        public void ThenAGeocacheWithNullNameShouldBeInvalid()
        {
            // Arrange
            cache = new Geocache { Coordinate = new Coordinate(-90, 90) };

            // Act
            validationResults = ValidateModel(cache);

            // Assert
            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("Name", validationResults.First().MemberNames.First());
            Assert.AreEqual("The Name field is required.", validationResults.First().ErrorMessage);
        }

        [TestMethod]
        public void ThenAGeocacheWithEmptyNameShouldBeInvalid()
        {
            // Arrange
            cache = new Geocache { Name = string.Empty, Coordinate = new Coordinate(-90, 90) };

            // Act
            validationResults = ValidateModel(cache);

            // Assert
            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("Name", validationResults.First().MemberNames.First());
            Assert.AreEqual("The Name field is required.", validationResults.First().ErrorMessage);
        }

        [TestMethod]
        public void ThenAGeocacheWithNameLongerThan50CharactersShouldBeInvalid()
        {
            // Arrange
            cache = new Geocache {
                Name = new string('a', 51),
                Coordinate = new Coordinate(-90, 90)
            };

            // Act
            validationResults = ValidateModel(cache);

            // Assert
            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("Name", validationResults.First().MemberNames.First());
            Assert.AreEqual("The field Name must be a string or array type with a maximum length of '50'.", validationResults.First().ErrorMessage);
        }

        [TestMethod]
        public void ThenAGeocacheWithNameContainingNonAlphanumericOrWhiteSpaceCharactersShouldBeInvalid()
        {
            // Arrange
            cache = new Geocache
            {
                Name = "Geo-cache?",
                Coordinate = new Coordinate(-90, 90)
            };

            // Act
            validationResults = ValidateModel(cache);

            // Assert
            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("Name", validationResults.First().MemberNames.First());
            Assert.AreEqual("The field Name can only contain alphanumeric characters or spaces.", validationResults.First().ErrorMessage);
        }
    }

    [TestClass]
    public class WhenCreatingGeocacheObjectWithInvalidCoordinates : WhenWorkingWithTheGeocacheClass
    {
        [TestMethod]
        public void ThenAGeocacheWithLatitudeOutsideof0to90OShouldBeInvalid()
        {
            // Arrange
            cache = new Geocache
            {
                Name = "Name",
                Coordinate = new Coordinate(-91.0, 0.0)
            };

            // Act
            validationResults = ValidateModel(cache);

            // Assert
            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("Latitude", validationResults.First().MemberNames.First());
            Assert.AreEqual("The field Latitude must range from -90.0 to 90.0.", validationResults.First().ErrorMessage);
        }

        [TestMethod]
        public void ThenAGeocacheWithLongitude0to180ShouldBeInvalid()
        {
            // Arrange
            cache = new Geocache
            {
                Name = "Name",
                Coordinate = new Coordinate(0.0, 181.0)
            };

            // Act
            validationResults = ValidateModel(cache);

            // Assert
            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("Longitude", validationResults.First().MemberNames.First());
            Assert.AreEqual("The field Longitude must range from -180.0 to 180.0.", validationResults.First().ErrorMessage);
        }

        [TestMethod]
        public void ThenAGeocacheWithNullCoordinatesShouldInvalid()
        {
            // Arrange
            cache = new Geocache
            {
                Name = "Name"
            };

            // Act
            validationResults = ValidateModel(cache);

            // Assert
            Assert.AreEqual(2, validationResults.Count);
            Assert.AreEqual("The field Longitude is required.", validationResults.Find(x => x.MemberNames.Contains("Longitude")).ErrorMessage);
            Assert.AreEqual("The field Latitude is required.", validationResults.Find(x => x.MemberNames.Contains("Latitude")).ErrorMessage);

        }
    }

    [TestClass]
    public class WhenCreatingGeocacheObjectWithValidNameAndCoordinates : WhenWorkingWithTheGeocacheClass
    {
        [TestMethod]
        public void ThenAGeocacheWithValidNameAndValidCoordinatesIsValid()
        {
            // Arrange
            cache = new Geocache { Name = "Valid Geocache Name", Coordinate = new Coordinate(-44, 44) };

            // Act
            validationResults = ValidateModel(cache);

            // Assert
            Assert.AreEqual(0, validationResults.Count);
        }
    }
}
