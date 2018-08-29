using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using CMK;

namespace AnimatedPngCreator.Tests
{
    [TestClass]
    public class ImageChangeAnalyserTests
    {
        [TestMethod]
        public void TestBlackoutImage()
        {
            // Arrange
            var expected = new Bitmap(1, 1);
            expected.SetPixel(0, 0, Color.Transparent);
            var image = new Bitmap(1, 1);
            image.SetPixel(0, 0, Color.Black);
            var sut = new ImageChangeAnalyser();

            // Act
            sut.BlackoutImage(image, out bool b1);
            var result = sut.BlackoutImage(image, out bool b2) as Bitmap;
            var pixelColor = result.GetPixel(0, 0);

            // Assert
            Assert.IsTrue(
                Color.Transparent.A == pixelColor.A &&
                Color.Transparent.R == pixelColor.R &&
                Color.Transparent.G == pixelColor.G &&
                Color.Transparent.B == pixelColor.B
                );
        }
    }
}
