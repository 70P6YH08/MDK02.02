// Исходный код приложения для рефакторинга.
// Рефакторинг зафиксировать в текстовом документе со столбцами 
// Задание | Исходный код | Код после рефакторинга
// Разнести типы данных по разным файлам.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

namespace OrderManagementApp
{
    // Контекст базы данных
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=OrderManagement.db");
        }
    }

    // Класс Customer (клиент)
    public class Customer
    {
        public int Id { get; set; }
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("Имя не может быть пустым");
                _name = value;
            }
        }
        public string Email;
        public List<Order> Orders { get; set; }

        public void PrintCustomerInfo() =>
            Console.WriteLine($"Customer name: {Name}\nCustomer email: {Email}");

    }

    // Класс Order (заказ)
    public class Order
    {
        public int Id { get; set; }
        public double Total { get; set; }
        public bool IsExpress { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }

    // Сервис для работы с клиентами
    public class CustomerService
    {
        private readonly AppDbContext _dbContext;

        public CustomerService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddCustomer(Customer customer)
        {
            _dbContext.Customers.Add(customer);
            _dbContext.SaveChanges();
        }

        public void PrintCustomerInfo(int customerId)
        {
            var customer = _dbContext.Customers.Include(c => c.Orders).FirstOrDefault(c => c.Id == customerId);

            if (customer == null)
                return;

            Console.WriteLine("Customer name: " + customer.Name);
            Console.WriteLine("Email: " + customer.Email);
        }
    }

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
            var order = _dbContext.Orders.Include(o => o.Customer).FirstOrDefault(o => o.Id == orderId);

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
            CalculateDiscount(order);
            double finalPrice = CountFinalPrice(order);
            // итоговая цена
            return finalPrice;
        }
        private double CountFinalPrice(Order order) =>
            order.Total - _discount + (order.Total * _tax);
        private void CalculateDiscount(Order order) =>
            _discount = (order.Total > _minDiscountPrice) ? order.Total * _discountPercent : _discount = 0;

    }

    class Program
    {
        static void Main(string[] args)
        {
            using var dbContext = new AppDbContext();
            dbContext.Database.EnsureCreated();

            var customerService = new CustomerService(dbContext);
            var orderService = new OrderService(dbContext);

            var customer = new Customer { Name = "Alice", Email = "alice@example.com" };
            customerService.AddCustomer(customer);

            var order = new Order { Total = 1200, IsExpress = true, Customer = customer };
            orderService.AddOrder(order);

            customerService.PrintCustomerInfo(customer.Id);
            orderService.PrintOrderDetails(order.Id);

            Console.WriteLine("Final Price: " + orderService.CalculateFinalPrice(order));
        }
    }
}
