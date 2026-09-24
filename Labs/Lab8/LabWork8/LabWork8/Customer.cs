namespace LabWork8
{
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
}
