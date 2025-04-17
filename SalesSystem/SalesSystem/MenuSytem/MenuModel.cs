using SalesSystem.Database;

namespace SalesSystem.MenuSytem
{
    internal abstract class MenuModel<T> : MenuPrintBase where T : class
    {
        private readonly SalesSystemContext context = new();
        private DAL<T> dal;

        public override void PrintMenu()
        {
            dal = new DAL<T>(context);
            Console.Clear();
            Console.WriteLine("**************************");
            for (int i = 0; i < 3; i++)
                Console.WriteLine("*                        *");
            Console.WriteLine(ScreenText());
            for (int i = 0; i < 3; i++)
                Console.WriteLine("*                        *");
            Console.WriteLine("**************************");

            Console.WriteLine("\n\nWrite the wanted option:\n");
            Options(dal);
        }
        public abstract string ScreenText();
        public abstract void Options(Database.DAL<T> selectedDAL);
    }
}
