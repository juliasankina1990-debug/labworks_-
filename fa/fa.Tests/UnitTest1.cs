using Microsoft.VisualStudio.TestTools.UnitTesting;
using fans;
using System;

namespace fa.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_FA1_ExactOneZeroAtLeastOneOne_Positive()
        {
            var fa1 = new FA1();
            Assert.IsTrue(fa1.Run("01"));
            Assert.IsTrue(fa1.Run("101"));
            Assert.IsTrue(fa1.Run("110"));
            Assert.IsTrue(fa1.Run("10"));
        }

        [TestMethod]
        public void Test_FA1_ExactOneZeroAtLeastOneOne_Negative()
        {
            var fa1 = new FA1();
            Assert.IsFalse(fa1.Run("0"));
            Assert.IsFalse(fa1.Run("1"));
            Assert.IsFalse(fa1.Run("00"));
            Assert.IsFalse(fa1.Run("000"));
            Assert.IsFalse(fa1.Run("11"));
            Assert.IsFalse(fa1.Run("100"));
            Assert.IsFalse(fa1.Run("010"));
        }

        [TestMethod]
        public void Test_FA1_LongerValidStrings()
        {
            var fa1 = new FA1();
            Assert.IsTrue(fa1.Run("1110"));
            Assert.IsTrue(fa1.Run("0111"));
            Assert.IsTrue(fa1.Run("1101"));
            Assert.IsTrue(fa1.Run("1011"));
        }

        [TestMethod]
        public void Test_FA2_OddZerosOddOnes_Positive()
        {
            var fa2 = new FA2();
            Assert.IsTrue(fa2.Run("01"));
            Assert.IsTrue(fa2.Run("10"));
            Assert.IsTrue(fa2.Run("0001"));
            Assert.IsTrue(fa2.Run("1000"));
            Assert.IsTrue(fa2.Run("0111"));
        }

        [TestMethod]
        public void Test_FA2_OddZerosOddOnes_Negative()
        {
            var fa2 = new FA2();
            Assert.IsFalse(fa2.Run(""));
            Assert.IsFalse(fa2.Run("0"));
            Assert.IsFalse(fa2.Run("1"));
            Assert.IsFalse(fa2.Run("00"));
            Assert.IsFalse(fa2.Run("11"));
            Assert.IsFalse(fa2.Run("0011"));
            Assert.IsFalse(fa2.Run("011"));
        }

        [TestMethod]
        public void Test_FA3_Contains11_Positive()
        {
            var fa3 = new FA3();
            Assert.IsTrue(fa3.Run("11"));
            Assert.IsTrue(fa3.Run("011"));
            Assert.IsTrue(fa3.Run("110"));
            Assert.IsTrue(fa3.Run("111"));
            Assert.IsTrue(fa3.Run("001100"));
        }

        [TestMethod]
        public void Test_FA3_Contains11_Negative()
        {
            var fa3 = new FA3();
            Assert.IsFalse(fa3.Run(""));
            Assert.IsFalse(fa3.Run("0"));
            Assert.IsFalse(fa3.Run("1"));
            Assert.IsFalse(fa3.Run("00"));
            Assert.IsFalse(fa3.Run("01"));
            Assert.IsFalse(fa3.Run("10"));
            Assert.IsFalse(fa3.Run("101"));
            Assert.IsFalse(fa3.Run("010"));
        }

        [TestMethod]
        public void Test_FA1_EmptyString()
        {
            var fa1 = new FA1();
            Assert.IsFalse(fa1.Run(""));
        }

        [TestMethod]
        public void Test_FA2_EmptyString()
        {
            var fa2 = new FA2();
            Assert.IsFalse(fa2.Run(""));
        }

        [TestMethod]
        public void Test_FA3_EmptyString()
        {
            var fa3 = new FA3();
            Assert.IsFalse(fa3.Run(""));
        }

        [TestMethod]
        public void Test_FA1_MultipleZeros()
        {
            var fa1 = new FA1();
            Assert.IsFalse(fa1.Run("001"));
            Assert.IsFalse(fa1.Run("1001"));
            Assert.IsFalse(fa1.Run("0101"));
        }

        [TestMethod]
        public void Test_FA2_EvenZerosEvenOnes()
        {
            var fa2 = new FA2();
            Assert.IsFalse(fa2.Run("00"));
            Assert.IsFalse(fa2.Run("11"));
            Assert.IsFalse(fa2.Run("0011"));
            Assert.IsFalse(fa2.Run("1100"));
        }

        [TestMethod]
        public void Test_FA3_NoConsecutiveOnes()
        {
            var fa3 = new FA3();
            Assert.IsFalse(fa3.Run("101"));
            Assert.IsFalse(fa3.Run("0101"));
            Assert.IsFalse(fa3.Run("1010"));
        }

        [TestMethod]
        public void Test_FA1_StartWithOne()
        {
            var fa1 = new FA1();
            Assert.IsTrue(fa1.Run("10"));
            Assert.IsTrue(fa1.Run("110"));
            Assert.IsTrue(fa1.Run("1110"));
        }

        [TestMethod]
        public void Test_FA1_EndWithOne()
        {
            var fa1 = new FA1();
            Assert.IsTrue(fa1.Run("01"));
            Assert.IsTrue(fa1.Run("011"));
            Assert.IsTrue(fa1.Run("0111"));
        }

        [TestMethod]
        public void Test_FA2_SingleZeroSingleOne()
        {
            var fa2 = new FA2();
            Assert.IsTrue(fa2.Run("01"));
            Assert.IsTrue(fa2.Run("10"));
        }

        [TestMethod]
        public void Test_FA3_LongSequenceWith11()
        {
            var fa3 = new FA3();
            Assert.IsTrue(fa3.Run("0001111000"));
            Assert.IsTrue(fa3.Run("1100000000"));
            Assert.IsTrue(fa3.Run("0000000011"));
        }

        [TestMethod]
        public void Test_FA1_ZeroAtBeginning()
        {
            var fa1 = new FA1();
            Assert.IsTrue(fa1.Run("01"));
            Assert.IsTrue(fa1.Run("011"));
            Assert.IsTrue(fa1.Run("0111"));
        }

        [TestMethod]
        public void Test_FA2_ComplexOddCount()
        {
            var fa2 = new FA2();
            Assert.IsTrue(fa2.Run("0111001")); // 3 нуля, 5 единиц - оба нечетные
        }

        [TestMethod]
        public void Test_FA3_Just11()
        {
            var fa3 = new FA3();
            Assert.IsTrue(fa3.Run("11"));
        }

        [TestMethod]
        public void Test_FA1_MoreThanOneZero()
        {
            var fa1 = new FA1();
            Assert.IsFalse(fa1.Run("001"));
            Assert.IsFalse(fa1.Run("010"));
            Assert.IsFalse(fa1.Run("100"));
            Assert.IsFalse(fa1.Run("000"));
        }
    }
}
