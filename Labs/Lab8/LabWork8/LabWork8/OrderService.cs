using Microsoft.EntityFrameworkCore;

namespace LabWork8
{
    // Сервис для работы с заказами
    public class OrderService
    {
        private double _tax = 0.2;
        private double _discountPercent = 0.1;
        private double _minDiscountPrice = 10000;
        private double _discount = 0;

        private readonly AppDbContext _dbContext;

        public OrderService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddOrder(Order order)
        {
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
        }

        public void PrintOrderDetails(int orderId)
        {
            var order = _dbContext.Orders
                .Include(o => o.Customer)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                Console.WriteLine("Такого заказа не существует!");
                return;
            }

            PrintOrderInfo(order);
        }

        public void PrintOrderInfo(Order order)
        {
            PrintOrderId(order);
            PrintOrderTotalPrice(order);
            PrintExpressOrder(order);
            order.Customer.PrintCustomerInfo();
        }

        public static void PrintOrderId(Order order) =>
            Console.WriteLine($"Order Id: {order.Id}");

        public static void PrintOrderTotalPrice(Order order) =>
            Console.WriteLine($"Order Id: {order.Total}");

        public static void PrintExpressOrder(Order order) =>
            Console.WriteLine($"Order Id: {order.IsExpress}");

        public double CalculateFinalPrice(Order order)
        {
            double tax = 0.2; // НДС
            // скидка 10% при заказе от 10000
            double discount = 0;
            double minDiscountPrice = 10000;
            double discountPercent = 0.1;

            discount = (order.Total > minDiscountPrice)
                ? order.Total * discountPercent
			    : 0;

            // итоговая цена
            return order.Total - discount + (order.Total * tax);
        }
    }
}
