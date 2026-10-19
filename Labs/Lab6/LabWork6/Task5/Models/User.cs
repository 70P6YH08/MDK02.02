namespace Task5.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string SecondName { get; set; } = null!;
        public string Login { get; set; } = null!;
        public int Age { get; set; }
    }
}
