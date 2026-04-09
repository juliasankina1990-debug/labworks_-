using Xunit;
using fans;

namespace fa.Tests
{
    public class FA1Tests
    {
        private readonly FA1 _fa1;

        public FA1Tests()
        {
            _fa1 = new FA1();
        }

        [Fact]
        public void Run_OneZeroAndOneOne_Accepts()
        {
            // Строка "10" - один '0' и одна '1'
            var result = _fa1.Run("10");
            Assert.True(result);
        }

        [Fact]
        public void Run_ZeroThenOne_Accepts()
        {
            // Строка "01" - один '0' и одна '1'
            var result = _fa1.Run("01");
            Assert.True(result);
        }

        [Fact]
        public void Run_MultipleOnesAndOneZero_Accepts()
        {
            // Строка "1110" - один '0' и три '1'
            var result = _fa1.Run("1110");
            Assert.True(result);
        }

        [Fact]
        public void Run_OneZeroMultipleOnes_Accepts()
        {
            // Строка "11011" - один '0' и четыре '1'
            var result = _fa1.Run("11011");
            Assert.True(result);
        }

        [Fact]
        public void Run_OnlyOneZero_Rejects()
        {
            // Строка "0" - один '0', но нет '1'
            var result = _fa1.Run("0");
            Assert.False(result);
        }

        [Fact]
        public void Run_OnlyOnes_Rejects()
        {
            // Строка "111" - нет '0'
            var result = _fa1.Run("111");
            Assert.False(result);
        }

        [Fact]
        public void Run_TwoZeros_Rejects()
        {
            // Строка "00" - два '0'
            var result = _fa1.Run("00");
            Assert.False(result);
        }

        [Fact]
        public void Run_ThreeZeros_Rejects()
        {
            // Строка "10001" - три '0'
            var result = _fa1.Run("10001");
            Assert.False(result);
        }

        [Fact]
        public void Run_EmptyString_Rejects()
        {
            // Пустая строка
            var result = _fa1.Run("");
            Assert.False(result);
        }
    }

    public class FA2Tests
    {
        private readonly FA2 _fa2;

        public FA2Tests()
        {
            _fa2 = new FA2();
        }

        [Fact]
        public void Run_OneZeroOneOne_Accepts()
        {
            // Строка "01" - 1 '0' (нечет), 1 '1' (нечет)
            var result = _fa2.Run("01");
            Assert.True(result);
        }

        [Fact]
        public void Run_OneOneOneZero_Accepts()
        {
            // Строка "10" - 1 '0' (нечет), 1 '1' (нечет)
            var result = _fa2.Run("10");
            Assert.True(result);
        }

        [Fact]
        public void Run_ThreeZerosThreeOnes_Accepts()
        {
            // Строка "000111" - 3 '0' (нечет), 3 '1' (нечет)
            var result = _fa2.Run("000111");
            Assert.True(result);
        }

        [Fact]
        public void Run_FiveZerosOneOne_Accepts()
        {
            // Строка "000001" - 5 '0' (нечет), 1 '1' (нечет)
            var result = _fa2.Run("000001");
            Assert.True(result);
        }

        [Fact]
        public void Run_OneZeroThreeOnes_Accepts()
        {
            // Строка "0111" - 1 '0' (нечет), 3 '1' (нечет)
            var result = _fa2.Run("0111");
            Assert.True(result);
        }

        [Fact]
        public void Run_TwoZerosTwoOnes_Rejects()
        {
            // Строка "0011" - 2 '0' (чет), 2 '1' (чет)
            var result = _fa2.Run("0011");
            Assert.False(result);
        }

        [Fact]
        public void Run_OnlyZeros_Rejects()
        {
            // Строка "000" - 3 '0' (нечет), 0 '1' (чет)
            var result = _fa2.Run("000");
            Assert.False(result);
        }

        [Fact]
        public void Run_OnlyOnes_Rejects()
        {
            // Строка "111" - 0 '0' (чет), 3 '1' (нечет)
            var result = _fa2.Run("111");
            Assert.False(result);
        }

        [Fact]
        public void Run_EmptyString_Rejects()
        {
            // Пустая строка - 0 '0' (чет), 0 '1' (чет)
            var result = _fa2.Run("");
            Assert.False(result);
        }

        [Fact]
        public void Run_SingleZero_Rejects()
        {
            // Строка "0" - 1 '0' (нечет), 0 '1' (чет)
            var result = _fa2.Run("0");
            Assert.False(result);
        }

        [Fact]
        public void Run_SingleOne_Rejects()
        {
            // Строка "1" - 0 '0' (чет), 1 '1' (нечет)
            var result = _fa2.Run("1");
            Assert.False(result);
        }
    }

    public class FA3Tests
    {
        private readonly FA3 _fa3;

        public FA3Tests()
        {
            _fa3 = new FA3();
        }

        [Fact]
        public void Run_DoubleOne_Accepts()
        {
            // Строка "11"
            var result = _fa3.Run("11");
            Assert.True(result);
        }

        [Fact]
        public void Run_ZeroDoubleOneZero_Accepts()
        {
            // Строка "0110"
            var result = _fa3.Run("0110");
            Assert.True(result);
        }

        [Fact]
        public void Run_TripleOne_Accepts()
        {
            // Строка "111"
            var result = _fa3.Run("111");
            Assert.True(result);
        }

        [Fact]
        public void Run_OneOneWithZeros_Accepts()
        {
            // Строка "001100"
            var result = _fa3.Run("001100");
            Assert.True(result);
        }

        [Fact]
        public void Run_OneAtEnd_Accepts()
        {
            // Строка "011"
            var result = _fa3.Run("011");
            Assert.True(result);
        }

        [Fact]
        public void Run_OneAtStart_Accepts()
        {
            // Строка "110"
            var result = _fa3.Run("110");
            Assert.True(result);
        }

        [Fact]
        public void Run_NoDoubleOne_Rejects()
        {
            // Строка "01" - нет "11"
            var result = _fa3.Run("01");
            Assert.False(result);
        }

        [Fact]
        public void Run_SingleOne_Rejects()
        {
            // Строка "1" - нет "11"
            var result = _fa3.Run("1");
            Assert.False(result);
        }

        [Fact]
        public void Run_OnlyZeros_Rejects()
        {
            // Строка "000" - нет "11"
            var result = _fa3.Run("000");
            Assert.False(result);
        }

        [Fact]
        public void Run_EmptyString_Rejects()
        {
            // Пустая строка
            var result = _fa3.Run("");
            Assert.False(result);
        }

        [Fact]
        public void Run_AlternatingOnesZeros_Rejects()
        {
            // Строка "10101" - нет "11"
            var result = _fa3.Run("10101");
            Assert.False(result);
        }
    }
}
