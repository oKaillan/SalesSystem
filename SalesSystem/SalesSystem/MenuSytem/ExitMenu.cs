
namespace SalesSystem.MenuSytem
{
    internal class ExitMenu : MenuPrintBase
    {
        public override void PrintMenu()
        {
            Console.Clear();
            Console.Write("Leaving SalesSystem!");
            PointPrint();
            PointDelete();
            PointPrint();
            Thread.Sleep(100);
            Environment.Exit(0);
        }

        private void PointPrint()
        {
            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(500);
                Console.Write(".");
            }
        }

        private void PointDelete()
        {
            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(400);
                Console.Write("\b \b");
            }
        }
    }
}
