using System;
using System.Collections.Generic;

namespace fans
{
    public class State
    {
        public string Name;
        public Dictionary<char, State> Transitions;
        public bool IsAcceptState;
    }

    public class FA1
    {
        private State startState;
        private State zeroOnce;
        private State zeroOnceAndOne;
        private State errorState;
        private State rejectAfterTwoZeros;

        public FA1()
        {
            InitializeStates();
            SetTransitions();
        }

        private void InitializeStates()
        {
            startState = new State { Name = "S", IsAcceptState = false };
            zeroOnce = new State { Name = "Z", IsAcceptState = false };
            zeroOnceAndOne = new State { Name = "ZA", IsAcceptState = true };
            errorState = new State { Name = "E", IsAcceptState = false };
            rejectAfterTwoZeros = new State { Name = "R", IsAcceptState = false };

            // инициализируем словари переходов
            startState.Transitions = new Dictionary<char, State>();
            zeroOnce.Transitions = new Dictionary<char, State>();
            zeroOnceAndOne.Transitions = new Dictionary<char, State>();
            errorState.Transitions = new Dictionary<char, State>();
            rejectAfterTwoZeros.Transitions = new Dictionary<char, State>();
        }

        private void SetTransitions()
        {
            // из начального состояния
            startState.Transitions['0'] = zeroOnce;   // первый ноль
            startState.Transitions['1'] = errorState; // если сразу 1, то потом нужен 0

            // из состояния с одним нолем
            zeroOnce.Transitions['0'] = rejectAfterTwoZeros; // второй ноль - отвергаем
            zeroOnce.Transitions['1'] = zeroOnceAndOne;      // первая единица после ноля

            // из состояния с нолем и хотя бы одной единицей
            zeroOnceAndOne.Transitions['0'] = rejectAfterTwoZeros; // еще один ноль - отвергаем
            zeroOnceAndOne.Transitions['1'] = zeroOnceAndOne;      // дополнительные единицы - ок

            // из состояния ошибки (нет нолей)
            errorState.Transitions['0'] = zeroOnceAndOne;          // первый ноль
            errorState.Transitions['1'] = errorState;              // просто еще одна 1

            // из состояния с двумя нолями (ошибка)
            rejectAfterTwoZeros.Transitions['0'] = rejectAfterTwoZeros;
            rejectAfterTwoZeros.Transitions['1'] = rejectAfterTwoZeros;
        }

        public bool? Run(IEnumerable<char> input)
        {
            State currentState = errorState; // начинаем с состояния, где есть только 1

            foreach (char symbol in input)
            {
                if (!currentState.Transitions.ContainsKey(symbol))
                {
                    return null; // недопустимый символ
                }
                
                currentState = currentState.Transitions[symbol];
            }

            return currentState.IsAcceptState;
        }
    }

    public class FA2
    {
        private State q00; // чет 0, чет 1
        private State q01; // чет 0, нечет 1
        private State q10; // нечет 0, чет 1
        private State q11; // нечет 0, нечет 1

        public FA2()
        {
            InitializeStatesForFA2();
            SetupTransitionsForFA2();
        }

        private void InitializeStatesForFA2()
        {
            q00 = new State { Name = "q00", IsAcceptState = false };
            q01 = new State { Name = "q01", IsAcceptState = false };
            q10 = new State { Name = "q10", IsAcceptState = false };
            q11 = new State { Name = "q11", IsAcceptState = true };

            q00.Transitions = new Dictionary<char, State>();
            q01.Transitions = new Dictionary<char, State>();
            q10.Transitions = new Dictionary<char, State>();
            q11.Transitions = new Dictionary<char, State>();
        }

        private void SetupTransitionsForFA2()
        {
            // из q00
            q00.Transitions['0'] = q10; // чет 0 -> нечет 0, чет 1
            q00.Transitions['1'] = q01; // чет 1 -> нечет 1, чет 0

            // из q01
            q01.Transitions['0'] = q11; // чет 0 -> нечет 0, нечет 1 остается
            q01.Transitions['1'] = q00; // нечет 1 -> чет 1, нечет 0 остается

            // из q10
            q10.Transitions['0'] = q00; // нечет 0 -> чет 0, чет 1
            q10.Transitions['1'] = q11; // чет 1 -> нечет 1

            // из q11
            q11.Transitions['0'] = q01; // нечет 0 -> чет 0
            q11.Transitions['1'] = q10; // нечет 1 -> чет 1
        }

        public bool? Run(IEnumerable<char> input)
        {
            State currentState = q00; // начинаем с четного числа 0 и 1

            foreach (char symbol in input)
            {
                if (!currentState.Transitions.ContainsKey(symbol))
                {
                    return null;
                }
                
                currentState = currentState.Transitions[symbol];
            }

            return currentState.IsAcceptState;
        }
    }

    public class FA3
    {
        private State noOnesYet;
        private State oneOneFound;
        private State twoOnesFound;

        public FA3()
        {
            InitializeStatesForFA3();
            ConfigureTransitionsForFA3();
        }

        private void InitializeStatesForFA3()
        {
            noOnesYet = new State { Name = "N", IsAcceptState = false };
            oneOneFound = new State { Name = "O", IsAcceptState = false };
            twoOnesFound = new State { Name = "T", IsAcceptState = true };

            noOnesYet.Transitions = new Dictionary<char, State>();
            oneOneFound.Transitions = new Dictionary<char, State>();
            twoOnesFound.Transitions = new Dictionary<char, State>();
        }

        private void ConfigureTransitionsForFA3()
        {
            // из начального состояния (еще не встретили 1 или только одна 1 подряд)
            noOnesYet.Transitions['0'] = noOnesYet; // просто ноль
            noOnesYet.Transitions['1'] = oneOneFound; // первая 1

            // из состояния, где последний символ был 1
            oneOneFound.Transitions['0'] = noOnesYet; // 10 - снова ждем 11
            oneOneFound.Transitions['1'] = twoOnesFound; // 11 - нашли!

            // из состояния, где уже нашли 11
            twoOnesFound.Transitions['0'] = twoOnesFound; // все равно что дальше
            twoOnesFound.Transitions['1'] = twoOnesFound; // все равно что дальше
        }

        public bool? Run(IEnumerable<char> input)
        {
            State currentState = noOnesYet;

            foreach (char symbol in input)
            {
                if (!currentState.Transitions.ContainsKey(symbol))
                {
                    return null;
                }
                
                currentState = currentState.Transitions[symbol];
            }

            return currentState.IsAcceptState;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Проверка FA1
            Console.WriteLine("Тестирование FA1:");
            FA1 fa1 = new FA1();
            Console.WriteLine($"01: {fa1.Run("01")}");
            Console.WriteLine($"101: {fa1.Run("101")}");
            Console.WriteLine($"0: {fa1.Run("0")}");
            Console.WriteLine($"11: {fa1.Run("11")}");
            Console.WriteLine($"00: {fa1.Run("00")}");

            // Проверка FA2
            Console.WriteLine("\nТестирование FA2:");
            FA2 fa2 = new FA2();
            Console.WriteLine($"01: {fa2.Run("01")}");
            Console.WriteLine($"10: {fa2.Run("10")}");
            Console.WriteLine($"001: {fa2.Run("001")}");
            Console.WriteLine($"011: {fa2.Run("011")}");
            Console.WriteLine($"11: {fa2.Run("11")}");

            // Проверка FA3
            Console.WriteLine("\nТестирование FA3:");
            FA3 fa3 = new FA3();
            Console.WriteLine($"11: {fa3.Run("11")}");
            Console.WriteLine($"011: {fa3.Run("011")}");
            Console.WriteLine($"110: {fa3.Run("110")}");
            Console.WriteLine($"101: {fa3.Run("101")}");
            Console.WriteLine($"111: {fa3.Run("111")}");
        }
    }
}
