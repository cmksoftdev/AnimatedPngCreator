using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using CMK.ExtendedBitmap;
using CMK;

namespace AnimatedPngCreator.Tests
{
    /// <summary>
    /// AnimatedPngCreator integration tests
    /// </summary>
    [TestClass]
    public class AnimatedPngCreatorTests
    {
        [TestMethod]
        public void Create1()
        {
            // Arrange
            var expectedBytes = File.ReadAllBytes("TestFiles\\expected.png");
            var filePaths = new List<string>
            {
                "TestFiles\\1.bmp", "TestFiles\\2.bmp", "TestFiles\\3.bmp"
            };

            // Act
            CMK.AnimatedPngCreator.Create("actual1.png", filePaths, 1000);
            var actual = File.ReadAllBytes("actual1.png");

            // Assert
            Assert.IsTrue(expectedBytes.SequenceEqual(actual));
        }

        [TestMethod]
        public void Create2()
        {
            // Arrange
            var expectedBytes = File.ReadAllBytes("TestFiles\\expected.png");
            var images = new List<IExtendedBitmap>
            {
                BitmapFactory.FromFile("TestFiles\\1.bmp"),
                BitmapFactory.FromFile("TestFiles\\2.bmp"),
                BitmapFactory.FromFile("TestFiles\\3.bmp")
            };

            // Act
            CMK.AnimatedPngCreator.Create("actual2.png", images, 1000);
            var actual = File.ReadAllBytes("actual2.png");

            // Assert
            Assert.IsTrue(expectedBytes.SequenceEqual(actual));
        }

        [TestMethod]
        public void Create3()
        {
            // Arrange
            var expectedBytes = File.ReadAllBytes("TestFiles\\expected.png");
            var images = new List<AnimatedPng.Frame>
            {
                CMK.AnimatedPngCreator.Frame(BitmapFactory.FromFile("TestFiles\\1.bmp"),1000),
                CMK.AnimatedPngCreator.Frame(BitmapFactory.FromFile("TestFiles\\2.bmp"),1000),
                CMK.AnimatedPngCreator.Frame(BitmapFactory.FromFile("TestFiles\\3.bmp"),1000)
            };

            // Act
            CMK.AnimatedPngCreator.Create("actual3.png", images);
            var actual = File.ReadAllBytes("actual3.png");

            // Assert
            Assert.IsTrue(expectedBytes.SequenceEqual(actual));
        }

        [TestCleanup]
        public void CleanUp()
        {
            File.Delete("actual1.png");
            File.Delete("actual2.png");
            File.Delete("actual3.png");
        }
    }
}
