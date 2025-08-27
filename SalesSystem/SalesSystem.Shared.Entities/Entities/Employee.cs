namespace SalesSystem.Entities
{
    public class Employee
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }

        public Employee(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public void ChangeEmployeeName(string name)
        {
            Name = name;
        }
        public void ChangeEmployeeEmail(string email)
        {
            Email = email;
        }

        public override string ToString()
        {
            return "\nEmployee Information:\n\n" +
                   $"iD: {Id}\n" +
                   $"Name: {Name}\n" +
                   $"Email: {Email}\n";
        }
    }
}
