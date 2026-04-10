using Stateless;

namespace BugPro;


public class Bug
{

    public enum State
    {
        NewDefect,          // НОВЫЙ ДЕФЕКТ
        Analysis,           // РАЗБОР ДЕФЕКТОВ
        Fixed,              // ИСПРАВЛЕНИЕ
        Closed,             // ЗАКРЫТИЕ
        Reopened,           // ПЕРЕОТКРЫТИЕ
        Returned            // ВОЗВРАТ
    }

    // Триггеры (действия)
    public enum Trigger
    {
        AssignToTeam,       // ДЕЙСТВИЕ: ПРОДУКТОВАЯ КОМАНДА
        Investigate,        // ДЕЙСТВИЕ: ТЕСТИРОВЩИК
        Fix,                // ДЕЙСТВИЕ: РАЗРАБОТЧИК
        Verify,             // Проверка исправления (тестировщик)
        Reject,             // Отклонить как не дефект / дубль / не воспроизводимо
        Close,              // Закрыть (если проблема решена)
        Reopen,             // Переоткрыть (если не решена)
        Return              // Вернуть на разбор (например, не хватает информации)
    }

    private readonly StateMachine<State, Trigger> _machine;

    public Bug()
    {
        _machine = new StateMachine<State, Trigger>(State.NewDefect);

        // Настройка переходов по диаграмме

        // НОВЫЙ ДЕФЕКТ → РАЗБОР ДЕФЕКТОВ
        _machine.Configure(State.NewDefect)
            .Permit(Trigger.AssignToTeam, State.Analysis);

        // РАЗБОР ДЕФЕКТОВ → 
        //   → ИСПРАВЛЕНИЕ (если нужно исправлять)
        _machine.Configure(State.Analysis)
            .Permit(Trigger.Investigate, State.Fixed)     // тестировщик начал проверку → переходим к исправлению?
            .Permit(Trigger.Return, State.Analysis)       // возврат в разбор (если не хватает инфы)
            .Permit(Trigger.Reject, State.Returned);      // отклонение: не дефект / дубль / не воспроизводимо

        // Но по диаграмме: из "РАЗБОР" можно сразу в "ВОЗВРАТ", "НЕ ДЕФЕКТ", "ДУБЛЬ", "НЕ ВОСПРОИЗВОДИМО"
        // → сделаем отдельные триггеры для каждого случая:
        // Уточним: в диаграмме "НЕ ДЕФЕКТ", "НЕ ИСПРАВЛЯТЬ", "ДУБЛИРОВАНИЕ", "НЕ ВОСПРОИЗВОДИМО" — всё это ведёт в состояние "ВОЗВРАТ"
        // Поэтому объединим их в один триггер `Reject`, который всегда ведёт в `Returned`.

        // ИСПРАВЛЕНИЕ → ПРОВЕРКА (через Verify) → затем решение
        _machine.Configure(State.Fixed)
            .Permit(Trigger.Verify, State.Analysis)  // после исправления — тестировщик проверяет, возвращает на анализ
            .Permit(Trigger.Fix, State.Fixed);        // разработчик может продолжать править (само-переход)

        // После проверки — вопрос: "ПРОБЛЕМА РЕШЕНА?"
        // В диаграмме: из "ИСПРАВЛЕНИЕ" есть стрелка на "ПРОБЛЕМА РЕШЕНА?" → ДА/НЕТ
        // Реализуем это через два триггера:
        //   - `Close` — если да → Closed
        //   - `Reopen` — если нет → Reopened
        // Но по логике: `Verify` возвращает в Analysis, а уже там решают — закрыть или переоткрыть.
        // Чтобы упростить и соответствовать диаграмме напрямую, сделаем так:

        // Из Analysis после Verify можно:
        _machine.Configure(State.Analysis)
            .Permit(Trigger.Close, State.Closed)      // проблема решена
            .Permit(Trigger.Reopen, State.Reopened)   // проблема не решена
            .Permit(Trigger.Return, State.Analysis);  // вернуть на уточнение

        // Переоткрытый баг → снова в анализ
        _machine.Configure(State.Reopened)
            .Permit(Trigger.Investigate, State.Fixed)
            .Permit(Trigger.Return, State.Analysis);

        // Закрытый баг можно переоткрыть
        _machine.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);

        // Возвращённый (Rejected) баг можно вернуть в анализ (например, если передумали)
        _machine.Configure(State.Returned)
            .Permit(Trigger.AssignToTeam, State.Analysis);
    }

    // Методы-обёртки для удобства
    public void AssignToTeam() => _machine.Fire(Trigger.AssignToTeam);
    public void Investigate() => _machine.Fire(Trigger.Investigate);
    public void Fix() => _machine.Fire(Trigger.Fix);
    public void Verify() => _machine.Fire(Trigger.Verify);
    public void Reject() => _machine.Fire(Trigger.Reject);
    public void Close() => _machine.Fire(Trigger.Close);
    public void Reopen() => _machine.Fire(Trigger.Reopen);
    public void Return() => _machine.Fire(Trigger.Return);

    public State GetCurrentState() => _machine.State;

    // Для вывода в консоль
    public override string ToString()
    {
        return $"Bug state: {GetCurrentState()}";
    }
}


class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Демонстрация жизненного цикла бага ===\n");

        var bug = new Bug();
        Console.WriteLine(bug);

        // Сценарий 1: стандартный путь
        bug.AssignToTeam();   // New → Analysis
        Console.WriteLine(bug);

        bug.Investigate();    // Analysis → Fixed
        Console.WriteLine(bug);

        bug.Fix();            // Fixed → Fixed (самопетля)
        Console.WriteLine(bug);

        bug.Verify();         // Fixed → Analysis
        Console.WriteLine(bug);

        bug.Close();          // Analysis → Closed
        Console.WriteLine(bug);

        // Сценарий 2: переоткрытие
        bug.Reopen();         // Closed → Reopened
        Console.WriteLine(bug);

        bug.Investigate();    // Reopened → Fixed
        Console.WriteLine(bug);

        // Сценарий 3: отклонение
        var bug2 = new Bug();
        bug2.AssignToTeam();
        bug2.Reject();        // Analysis → Returned
        Console.WriteLine($"Отклонённый баг: {bug2}");

        Console.WriteLine("\n--- Готово ---");
    }
}
