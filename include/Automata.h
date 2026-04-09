#ifndef AUTOMATA_H
#define AUTOMATA_H

#include <string>
#include <vector>

enum class STATES {
    OFF,
    WAIT,
    ACCEPT,
    CHECK,
    COOK
};

class Automata {
private:
    double cash;
    std::vector<std::string> menu;
    std::vector<double> prices;
    STATES state;
    int selectedIdx; // НОВОЕ: индекс выбранного напитка (-1 еслиpublic:
    // Конструктор: задаём меню и цены "вручную" для простоты
    Automata();

    // Методы управления состояниями (соответствуют переходам на диаграмме)
    void on();
    void off();
    void coin(double amount);
    void etMenu();           // вывод меню (для пользователя — просто печать, не возвращает ничего)
    STATES getState() const; // возвращает текущее состояние
    void choice(int index);  // выбор напитка по индексу
    void check();            // проверка наличия необходимой суммы
    void cancel();           // отмена
    void cook();             // имитация процесса приготовления напитка
    void finish();           // завершение обслуживания пользователя

    // Вспомогательные методы (не обязательны, но удобны)
    double getCash() const { return cash; }
    int getMenuSize() const { return static_cast<int>(menu.size()); }
};
