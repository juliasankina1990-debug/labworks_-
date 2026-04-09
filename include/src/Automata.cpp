#include "Automata.h"
#include <iostream>
#include <iomanip>
#include <cmath> 


Automata::Automata()
    : cash(0.0), state(STATES::OFF), selectedIdx(-1) {
    menu = {"Чай", "Кофе", "Какао", "Молоко"};
    prices = {15.0, 20.0, 25.0, 10.0};
}

void Automata::on() {
    if (state == STATES::OFF) {
        state = STATES::WAIT;
        selectedIdx = -1; // сбросим индекс выбора при включении
        std::cout << "[ON] Автомат включён. Жду монет...\n";
    } else {
        std::cout << "[ON] Уже включён.\n";
    }
}

void Automata::off() {
    if (state != STATES::OFF) {
        state = STATES::OFF;
        cash = 0.0;
        selectedIdx = -1; // также сбросим
        std::cout << "[OFF] Автомат выключен.\n";
    } else {
        std::cout << "[OFF] Уже выключен.\n";
    }
}

void Automata::coin(double amount) {
    if (state == STATES::WAIT || state == STATES::ACCEPT) {
        if (amount > 0) {
            cash += amount;
            std::cout << "[COIN] Добавлено " << std::fixed << std::setprecision(2) << amount
                      << " руб. Всего: " << cash << "\n";
            if (state == STATES::WAIT) {
                state = STATES::ACCEPT;
                std::cout << "Перешёл в состояние ACCEPT.\n";
            }
        } else {
            std::cout << "[COIN] Некорректная сумма.\n";
        }
    } else {
        std::cout << "[COIN] Нельзя вносить деньги в текущем состоянии (" << static_cast<int>(state) << ").\n";
    }
}

void Automata::etMenu() {
    if (state == STATES::ACCEPT || state == STATES::WAIT) {
        std::cout << "\n--- МЕНЮ ---\n";
        for (size_t i = 0; i < menu.size(); ++i) {
            std::cout << i + 1 << ". " << menu[i] << " — " << std::fixed << std::setprecision(2) << prices[i] << " руб.\n";
        }
        std::cout << "----------------\n";
    } else {
        std::cout << "[ETMENU] Меню недоступно в текущем состоянии.\n";
    }
}

STATES Automata::getState() const {
    return state;
}

void Automata::choice(int index) {
    if (state == STATES::ACCEPT) {
        if (index >= 1 && static_cast<size_t>(index) <= menu.size()) {
            int idx = index - 1;
            std::cout << "[CHOICE] Выбрано: " << menu[idx] << " (" << prices[idx] << " руб.)\n";
            selectedIdx = idx; // Сохраняем индекс выбранного напитка
            state = STATES::CHECK;
        } else {
            std::cout << "[CHOICE] Неверный номер напитка.\n";
        }
    } else {
        std::cout << "[CHOICE] Выбор недоступен в текущем состоянии (" << static_cast<int>(state) << ").\n";
    }
}

void Automata::check() {
    if (state == STATES::CHECK) {
        if (selectedIdx == -1) {
            std::cout << "[CHECK] Ошибка: напиток не выбран.\n";
            state = STATES::ACCEPT;
            return;
        }

        double needed = prices[selectedIdx];
        if (cash >= needed) {
            std::cout << "[CHECK] Денег достаточно (" << cash << " >= " << needed << "). Готов к приготовлению.\n";
            state = STATES::COOK;
        } else {
            std::cout << "[CHECK] Недостаточно денег (" << cash << " < " << needed << "). Ожидайте.\n";
            state = STATES::ACCEPT;
        }
    } else {
        std::cout << "[CHECK] Проверка недоступна в текущем состоянии (" << static_cast<int>(state) << ").\n";
    }
}

void Automata::cancel() {
    if (state == STATES::ACCEPT || state == STATES::CHECK) {
        std::cout << "[CANCEL] Сеанс отменён. Возврат денег: " << cash << " руб.\n";
        cash = 0.0;
        selectedIdx = -1; // сбросим выбор
        state = STATES::WAIT;
    } else {
        std::cout << "[CANCEL] Отмена недоступна в текущем состоянии (" << static_cast<int>(state) << ").\n";
    }
}

void Automata::cook() {
    if (state == STATES::COOK) {
        if (selectedIdx != -1) {
            std::cout << "[COOK] Приготовление напитка: " << menu[selectedIdx] << "...\n";
            std::cout << "...Готово!\n";
            cash -= prices[selectedIdx]; // списываем цену
            if (cash > 0) {
                std::cout << "Сдача: " << cash << " руб.\n";
                cash = 0.0;
            }
        } else {
            std::cout << "[COOK] Ошибка: напиток не выбран.\n";
        }
        selectedIdx = -1; // сбросим выбор
        state = STATES::WAIT;
    } else {
        std::cout << "[COOK] Приготовление недоступно в текущем состоянии (" << static_cast<int>(state) << ").\n";
    }
}

void Automata::finish() {
    if (state == STATES::WAIT) {
        std::cout << "[FINISH] Сеанс завершён.\n";
    } else {
        std::cout << "[FINISH] Завершение недоступно в текущем состоянии (" << static_cast<int>(state) << ").\n";
    }
}
