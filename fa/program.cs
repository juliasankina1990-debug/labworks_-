using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fans
{
    public class State
    {
        public string Name;
        public Dictionary<char, State> Transitions;
        public bool IsAcceptState;
    }

    // FA1: ДКА, который допускает бинарную строку, содержащую ровно один '0' и хотя бы одну '1'
    public class FA1
    {
        private State q0; // начальное состояние: еще не видели ни '0', ни '1'
        private State q1; // видели только '1' (ноль пока не встречался)
        private State q2; // видели ровно один '0' и хотя бы одну '1' (принимающее)
        private State q3; // видели более одного '0' (не принимающее)
        private State q4; // видели ровно один '0' но ни одной '1' (не принимающее)
        private State q5; // тупиковое состояние

        private State InitialState;

        public FA1()
        {
            q0 = new State { Name = "q0", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
            q1 = new State { Name = "q1", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
            q2 = new State { Name = "q2", IsAcceptState = true, Transitions = new Dictionary<char, State>() };
            q3 = new State { Name = "q3", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
            q4 = new State { Name = "q4", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
            q5 = new State { Name = "q5", IsAcceptState = false, Transitions = new Dictionary<char, State>() };

            InitialState = q0;

            // Переходы из q0 (ничего не видели)
            q0.Transitions['0'] = q4; // первый '0', пока нет '1'
            q0.Transitions['1'] = q1; // первая '1', пока нет '0'

            // Переходы из q1 (видели только '1')
            q1.Transitions['0'] = q2; // теперь есть один '0' и хотя бы одна '1' -> принимающее
            q1.Transitions['1'] = q1; // еще '1'

            // Переходы из q2 (ровно один '0' и хотя бы одна '1') - принимающее
            q2.Transitions['0'] = q3; // второй '0' -> не принимающее
            q2.Transitions['1'] = q2; // еще '1', все еще принимающее

            // Переходы из q3 (более одного '0')
            q3.Transitions['0'] = q3; // еще '0'
            q3.Transitions['1'] = q3; // '1' не поможет

            // Переходы из q4 (ровно один '0', но нет '1')
            q4.Transitions['0'] = q3; // второй '0'
            q4.Transitions['1'] = q2; // теперь есть '1' -> принимающее

            // Тупиковое состояние
            q5.Transitions['0'] = q5;
            q5.Transitions['1'] = q5;
        }

        public bool? Run(IEnumerable<char> s)
        {
            State current = InitialState;
            foreach (var c in s)
            {
                if (!current.Transitions.ContainsKey(c))
                    return null;
                current = current.Transitions[c];
                if (current == null)
                    return null;
            }
            return current.IsAcceptState;
        }
    }

    // FA2: ДКА, который допускает бинарную строку с нечетным количеством '0' и нечетным количеством '1'
    public class FA2
    {
        // Состояния кодируют (четность_0, четность_1):
        // 0 = четное, 1 = нечетное
        // q00: (чет, чет) - начальное
        // q01: (чет, нечет)
        // q10: (нечет, чет)
        // q11: (нечет, нечет) - принимающее
        private State q00;
        private State q01;
        private State q10;
        private State q11;

        private State InitialState;

        public FA2()
        {
            q00 = new State { Name = "q00", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
            q01 = new State { Name = "q01", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
            q10 = new State { Name = "q10", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
            q11 = new State { Name = "q11", IsAcceptState = true, Transitions = new Dictionary<char, State>() };

            InitialState = q00;

            // Из q00 (чет, чет)
            q00.Transitions['0'] = q10; // '0' меняет четность нулей
            q00.Transitions['1'] = q01; // '1' меняет четность единиц

            // Из q01 (чет, нечет)
            q01.Transitions['0'] = q11; // '0' -> (нечет, нечет) = принимающее
            q01.Transitions['1'] = q00; // '1' -> (чет, чет)

            // Из q10 (нечет, чет)
            q10.Transitions['0'] = q00; // '0' -> (чет, чет)
            q10.Transitions['1'] = q11; // '1' -> (нечет, нечет) = принимающее

            // Из q11 (нечет, нечет) - принимающее
            q11.Transitions['0'] = q01; // '0' -> (чет, нечет)
            q11.Transitions['1'] = q10; // '1' -> (нечет, чет)
        }

        public bool? Run(IEnumerable<char> s)
        {
            State current = InitialState;
            foreach (var c in s)
            {
                if (!current.Transitions.ContainsKey(c))
                    return null;
                current = current.Transitions[c];
                if (current == null)
                    return null;
            }
            return current.IsAcceptState;
        }
    }

    // FA3: ДКА, который допускает бинарную строку, содержащую '11'
    public class FA3
    {
        private State q0; // начальное: еще не видели '1' или последний символ был '0'
        private State q1; // видели одну '1' подряд
        private State q2; // видели '11' (принимающее, поглощающее)

        private State InitialState;

        public FA3()
        {
            q0 = new State { Name = "q0", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
            q1 = new State { Name = "q1", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
            q2 = new State { Name = "q2", IsAcceptState = true, Transitions = new Dictionary<char, State>() };

            InitialState = q0;

            // Из q0
            q0.Transitions['0'] = q0; // '0' сбрасывает
            q0.Transitions['1'] = q1; // первая '1'

            // Из q1
            q1.Transitions['0'] = q0; // '0' сбрасывает
            q1.Transitions['1'] = q2; // вторая '1' подряд -> '11' найдено

            // Из q2 (уже нашли '11')
            q2.Transitions['0'] = q2; // любой символ остается в принимающем
            q2.Transitions['1'] = q2;
        }

        public bool? Run(IEnumerable<char> s)
        {
            State current = InitialState;
            foreach (var c in s)
            {
                if (!current.Transitions.ContainsKey(c))
                    return null;
                current = current.Transitions[c];
                if (current == null)
                    return null;
            }
            return current.IsAcceptState;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Тест FA1
            FA1 fa1 = new FA1();
            Console.WriteLine("FA1 Tests:");
            Console.WriteLine($"\"10\" -> {fa1.Run("10")}");       // True: один '0', есть '1'
            Console.WriteLine($"\"01\" -> {fa1.Run("01")}");       // True: один '0', есть '1'
            Console.WriteLine($"\"110\" -> {fa1.Run("110")}");     // True: один '0', есть '1'
            Console.WriteLine($"\"0\" -> {fa1.Run("0")}");         // False: нет '1'
            Console.WriteLine($"\"1\" -> {fa1.Run("1")}");         // False: нет '0'
            Console.WriteLine($"\"00\" -> {fa1.Run("00")}");       // False: два '0'
            Console.WriteLine($"\"1010\" -> {fa1.Run("1010")}");   // False: два '0'

            // Тест FA2
            FA2 fa2 = new FA2();
            Console.WriteLine("\nFA2 Tests:");
            Console.WriteLine($"\"01\" -> {fa2.Run("01")}");       // True: 1 '0', 1 '1' (оба нечетные)
            Console.WriteLine($"\"10\" -> {fa2.Run("10")}");       // True
            Console.WriteLine($"\"000111\" -> {fa2.Run("000111")}"); // True: 3 '0', 3 '1'
            Console.WriteLine($"\"00\" -> {fa2.Run("00")}");       // False: 2 '0' (чет), 0 '1' (чет)
            Console.WriteLine($"\"11\" -> {fa2.Run("11")}");       // False: 0 '0' (чет), 2 '1' (чет)
            Console.WriteLine($"\"0\" -> {fa2.Run("0")}");         // False: 1 '0', 0 '1'
            Console.WriteLine($"\"1\" -> {fa2.Run("1")}");         // False: 0 '0', 1 '1'

            // Тест FA3
            FA3 fa3 = new FA3();
            Console.WriteLine("\nFA3 Tests:");
            Console.WriteLine($"\"11\" -> {fa3.Run("11")}");       // True
            Console.WriteLine($"\"0110\" -> {fa3.Run("0110")}");   // True
            Console.WriteLine($"\"111\" -> {fa3.Run("111")}");     // True
            Console.WriteLine($"\"01\" -> {fa3.Run("01")}");       // False
            Console.WriteLine($"\"10\" -> {fa3.Run("10")}");       // False
            Console.WriteLine($"\"000\" -> {fa3.Run("000")}");     // False
            Console.WriteLine($"\"\" -> {fa3.Run("")}");           // False
        }
    }
}
