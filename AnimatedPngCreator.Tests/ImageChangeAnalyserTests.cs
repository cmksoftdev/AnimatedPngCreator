using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using CMK;
using CMK.ExtendedBitmap;

namespace AnimatedPngCreator.Tests
{
    [TestClass]
    public class ImageChangeAnalyserTests
    {
        [TestMethod]
        public void TestBlackoutImage_SameImage_TransparentPixel()
        {
            // Arrange
            var expected = new ExtendedBitmap(1, 1);
            expected.SetPixel(0, 0, Color.Transparent);
            var image = new ExtendedBitmap(1, 1);
            image.SetPixel(0, 0, Color.Black);
            var sut = new ImageChangeAnalyser();

            // Act
            sut.BlackoutImage(image, out bool b1);
            var result = sut.BlackoutImage(image, out bool b2) as ExtendedBitmap;
            var pixelColor = result.GetPixel(0, 0);

            // Assert
            Assert.IsTrue(
                Color.Transparent.A == pixelColor.A &&
                Color.Transparent.R == pixelColor.R &&
                Color.Transparent.G == pixelColor.G &&
                Color.Transparent.B == pixelColor.B
                );
        }

        [TestMethod]
        public void TestBlackoutImage_DifferentImage_BlackPixel()
        {
            // Arrange
            var expected = new ExtendedBitmap(1, 1);
            expected.SetPixel(0, 0, Color.Transparent);
            var image1 = new ExtendedBitmap(1, 1);
            image1.SetPixel(0, 0, Color.White);
            var image2 = new ExtendedBitmap(1, 1);
            image2.SetPixel(0, 0, Color.Black);
            var sut = new ImageChangeAnalyser();

            // Act
            sut.BlackoutImage(image1, out bool b1);
            var result = sut.BlackoutImage(image2, out bool b2) as ExtendedBitmap;
            var pixelColor = result.GetPixel(0, 0);

            // Assert
            Assert.IsTrue(
                Color.Black.A == pixelColor.A &&
                Color.Black.R == pixelColor.R &&
                Color.Black.G == pixelColor.G &&
                Color.Black.B == pixelColor.B
                );
        }
    }
}
