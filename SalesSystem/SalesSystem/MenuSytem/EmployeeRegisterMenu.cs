using SalesSystem.Database;
using SalesSystem.Entities;

namespace SalesSystem.MenuSytem
{
    internal class EmployeeRegisterMenu : MenuModel<Employee>
    {
        public override string ScreenText()
        {
            return "* EMPLOYEE REGISTER MENU *";
        }
        public override void Options(DAL<Employee> empDAL)
        {
            char loop = 'y';
            while (loop.ToString().ToLower() == 'y'.ToString())
            {
                Console.Write("Employee Name: ");
                try
                {
                    string empName = Console.ReadLine().TrimStart().TrimEnd();

                    // Verifies if Employee Name is null or have Digits
                    if (string.IsNullOrEmpty(empName) || empName.Length <= 0 || empName.Any(char.IsDigit))
                    {
                        throw new ArgumentOutOfRangeException("Invalid Name!");
                    }


                    Console.Write("Employee Email: ");
                    string empEmail = Console.ReadLine().TrimStart().TrimEnd();

                    //Verifies if email is null or if is valid
                    if (string.IsNullOrEmpty(empEmail) || empEmail.Length <= 0 || !empEmail.Contains("@") || !empEmail.Contains(".com"))
                    {
                        throw new ArgumentOutOfRangeException("Invalid Email!");
                    }

                    Employee employee = new Employee(empName, empEmail);
                    empDAL.Create(employee);
                    Console.WriteLine("\nEmployee Registered with Success!");

                    Console.WriteLine("\nDo you want to register another Employee?\n[Y] Yes [N] No");
                    loop = char.Parse(Console.ReadLine());
                    Console.WriteLine();
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
            GetBackToMenu();
        }
    }
}
