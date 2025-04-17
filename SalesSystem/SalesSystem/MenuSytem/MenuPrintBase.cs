namespace SalesSystem.MenuSytem
{
    internal abstract class MenuPrintBase
    {
        public abstract void PrintMenu();
        public static void GetBackToMenu()
        {
            var menu = new Menu();
            menu.PrintMenu();
        }
    }
}
