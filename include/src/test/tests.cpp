#include "Automata.h"
#include <gtest/gtest.h>

TEST(AutomataTest, InitialStateIsOff) {
    Automata a;
    EXPECT_EQ(a.getState(), STATES::OFF);
}

TEST(AutomataTest, OnTurnsOn) {
    Automata a;
    a.on();
    EXPECT_EQ(a.getState(), STATES::WAIT);
}

TEST(AutomataTest, CoinInWaitEntersAccept) {
    Automata a;
    a.on();
    a.coin(10.0);
    EXPECT_EQ(a.getState(), STATES::ACCEPT);
    EXPECT_EQ(a.getCash(), 10.0);
}

TEST(AutomataTest, ChoiceStoresSelectedIndex) {
    Automata a;
    a.on();
    a.coin(20.0);
    a.choice(2); // выбираем кофе
    EXPECT_EQ(a.getState(), STATES::CHECK);

}

TEST(AutomataTest, CheckWithEnoughMoneyGoesToCook) {
    Automata a;
    a.on();
    a.coin(25.0);
    a.choice(3); // какао = 25
    a.check(); // теперь проверяет конкретно 3-й напиток
    EXPECT_EQ(a.getState(), STATES::COOK);
}

TEST(AutomataTest, CheckWithNotEnoughStaysInAccept) {
    Automata a;
    a.on();
    a.coin(10.0);
    a.choice(3); // какао = 25
    a.check();
    EXPECT_EQ(a.getState(), STATES::ACCEPT);
}

TEST(AutomataTest, CancelResetsCashAndSelection) {
    Automata a;
    a.on();
    a.coin(20.0);
    a.choice(1);
    a.cancel();
    EXPECT_EQ(a.getState(), STATES::WAIT);
    EXPECT_EQ(a.getCash(), 0.0);
}

TEST(AutomataTest, CookPreparesCorrectDrink) {
    Automata a;
    a.on();
    a.coin(20.0);
    a.choice(2); // кофе = 20
    a.check();
    a.cook();
    EXPECT_EQ(a.getState(), STATES::WAIT);
    EXPECT_EQ(a.getCash(), 0.0); // сдача в cook() обнуляет
}

TEST(AutomataTest, CookDeductsPriceCorrectly) {
    Automata a;
    a.on();
    a.coin(25.0);
    a.choice(2); // кофе = 20
    a.check();
    a.cook();
    EXPECT_EQ(a.getCash(), 0.0); // 25 - 20 = 5 -> сдача = 5
}

TEST(AutomataTest, OffClearsAllData) {
    Automata a;
    a.on();
    a.coin(10.0);
    a.choice(1);
    a.off();
    EXPECT_EQ(a.getState(), STATES::OFF);
    EXPECT_EQ(a.getCash(), 0.0);
}

TEST(AutomataTest, CheckWithoutChoiceFails) {
    Automata a;
    a.on();
    a.coin(30.0);
    a.state = STATES::CHECK; 
    a.check(); 
    EXPECT_EQ(a.getState(), STATES::ACCEPT); 
}

TEST(AutomataTest, CookWithoutChoiceShowsError) {
    Automata a;
    a.on();
    a.state = STATES::COOK; 
    a.cook(); 
    EXPECT_EQ(a.getState(), STATES::WAIT); 
}
