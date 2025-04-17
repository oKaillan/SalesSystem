using SalesSystem.Database;
using SalesSystem.Entities;

namespace SalesSystem.MenuSytem
{
    internal class Menu : MenuModel<Menu>
    {
        public override string ScreenText()
        {
            return "*        Welcome!        *";
        }
        public override void Options(DAL<Menu> dal)
        {
            Console.WriteLine("1 - Employee Register");
            Console.WriteLine("2 - Product Register");
            Console.WriteLine("3 - Sale Register");
            Console.WriteLine("4 - Sales Records");
            Console.WriteLine("0 - Leave Menu");

            Dictionary<int, MenuPrintBase> menu = new()
            {
                {1, new EmployeeRegisterMenu() },
                {2, new ProductRegisterMenu() },
                {3, new SaleRegisterMenu() },
                {4, new SalesRecordsMenu() },
                {0, new ExitMenu() }
            };

            if (int.TryParse(Console.ReadLine(), out int option) && menu.TryGetValue(option, out MenuPrintBase selectedMenu))
            {
                selectedMenu.PrintMenu();
            }
            else
            {
                throw new Exception("Invalid Option!");
            }
        }
    }
}
