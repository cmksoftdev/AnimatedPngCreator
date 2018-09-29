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

        [TestMethod]
        public void FindSequenceTest1()
        {
            // Arrange

            var stream = new MemoryStream(array1);
            byte[] sequence = new byte[] { 0x01, 0x12 };
            var expected = new List<int>
            {
                5, 17, 29
            };
            var sut = new EngineBase();

            // Act
            var result = sut.FindSequence(sequence, stream);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FindSequenceTest2()
        {
            // Arrange

            byte[] sequence = new byte[] { 0x01, 0x12, 0x12 };
            var expected = new List<int>
            {
                5, 18, 31
            };
            var sut = new EngineBase();

            // Act
            var result = sut.FindSequence(sequence, array2);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FindSequenceTest3()
        {
            // Arrange
            var stream = new MemoryStream(array2);
            byte[] sequence = new byte[] { 0x01, 0x12, 0x12 };
            var expected = new List<int>
            {
                5, 18, 31
            };
            var sut = new EngineBase();

            // Act
            var result = sut.FindSequence(sequence, stream);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FindSequenceTest4()
        {
            // Arrange
            byte[] sequence = new byte[] { 0x01, 0x12 };
            var expected = new List<int>
            {
                5, 17, 29
            };
            var sut = new EngineBase();

            // Act
            var result = sut.FindSequence(sequence, array1);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }
    }
}

