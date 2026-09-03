using System.Diagnostics;

double CalculateDiscount(double price, double discountRate)
{
    Debug.Assert(price > 0);
    discountRate = discountRate / 100;
    Debug.Assert(discountRate > 0 && discountRate < 1);
    var totalPrice = price - price * discountRate;
    Debug.Assert(totalPrice <= price);
    return totalPrice;
}

CalculateDiscount(100, 15);
CalculateDiscount(0, 15);
CalculateDiscount(1000, 101);