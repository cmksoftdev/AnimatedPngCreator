using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using CMK;
using System.Collections.Generic;
using System.IO;

namespace AnimatedPngCreator.Tests
{
    [TestClass]
    public class EngineBaseTests
    {
        byte[] array1 = new byte[]
        {
                0x11,0x11,0x11,0x11,0x11,
                0x01,0x12,
                0x11,0x11,0x11,0x11,0x11,
                0x11,0x11,0x11,0x11,0x11,
                0x01,0x12,
                0x11,0x11,0x11,0x11,0x11,
                0x11,0x11,0x11,0x11,0x11,
                0x01,0x12,
                0x11,0x11,0x11,0x11,0x11
        };
        byte[] array2 = new byte[]
        {
                0x11,0x11,0x11,0x11,0x11,
                0x01,0x12,0x12,
                0x11,0x11,0x11,0x11,0x11,
                0x11,0x11,0x11,0x11,0x11,
                0x01,0x12,0x12,
                0x11,0x11,0x11,0x11,0x11,
                0x11,0x11,0x11,0x11,0x11,
                0x01,0x12,0x12,
                0x11,0x11,0x11,0x11,0x11
        };
        List<int> expected1 = new List<int> { 5, 17, 29 };
        List<int> expected2 = new List<int> { 5, 18, 31 };
        byte[] sequence1 = new byte[] { 0x01, 0x12 };
        byte[] sequence2 = new byte[] { 0x01, 0x12, 0x12 };

        [TestMethod]
        public void FindSequenceTest1()
        {
            // Arrange
            var stream = new MemoryStream(array1);
            var sut = new EngineBase();

            // Act
            var result = sut.FindSequence(sequence1, stream);

            // Assert
            CollectionAssert.AreEqual(expected1, result);
        }

        [TestMethod]
        public void FindSequenceTest2()
        {
            // Arrange
            var sut = new EngineBase();

            // Act
            var result = sut.FindSequence(sequence2, array2);

            // Assert
            CollectionAssert.AreEqual(expected2, result);
        }

        [TestMethod]
        public void FindSequenceTest3()
        {
            // Arrange
            var stream = new MemoryStream(array2);
            var sut = new EngineBase();

            // Act
            var result = sut.FindSequence(sequence2, stream);

            // Assert
            CollectionAssert.AreEqual(expected2, result);
        }

        [TestMethod]
        public void FindSequenceTest4()
        {
            // Arrange
            var sut = new EngineBase();

            // Act
            var result = sut.FindSequence(sequence1, array1);

            // Assert
            CollectionAssert.AreEqual(expected1, result);
        }

        [TestMethod]
        public void getSwappedArrayTest1()
        {
            // Arrange
            byte[] expected = new byte[] { 0x00, 0x00, 0x00, 0x00 };

            // Act
            var result = EngineBase.getSwappedArray(0);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void getSwappedArrayTest2()
        {
            // Arrange
            byte[] expected = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
            uint ui = 4294967295;

            // Act
            var result = EngineBase.getSwappedArray((int)ui);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }
    }
}

