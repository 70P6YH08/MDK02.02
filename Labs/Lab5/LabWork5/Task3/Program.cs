using System.Diagnostics;

double CalculateDiscount(double price, double discountRate)
{
    Debug.Assert(price > 0, "Цена не может быть меньше или равна нулю");
    Debug.Assert(discountRate > 0 && discountRate < 1, "Такой скидки не может быть");
    var totalPrice = price - price * discountRate;
    Debug.Assert(totalPrice <= price, "Итоговая цена не может быть больше исохдной цены");
    return totalPrice;
}

CalculateDiscount(100, 0.15);
CalculateDiscount(0, 0.15);
CalculateDiscount(1000, 1.01);
