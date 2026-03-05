namespace SalesSystem.Shared.Database.Entities
{
    public static class Roles 
    {
        public const string Admin = "Admin";
        public const string Employee = "Employee";
        public const string AdminOrEmployee = Admin + "," + Employee;
    }
}
