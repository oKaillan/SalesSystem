using SalesSystem.Database;
using SalesSystem.Entities;
using System.Globalization;

namespace SalesSystem.MenuSytem
{
    internal class SalesRecordsMenu : MenuModel<SalesLog>
    {
        private DAL<SalesLog> SalesLogDAL { get; set; } = new(new SalesSystemContext());
        private double TotalPeriodSales { get; set; }

        public override string ScreenText()
        {
            return "*   SALES RECORDS MENU   *";
        }

        public override void Options(DAL<SalesLog> salesLogDAL)
        {
            Console.WriteLine("How do you want to get the SalesLog?");
            Console.WriteLine("1 - By Period");
            Console.WriteLine("2 - By Employee");
            Console.WriteLine("3 - By Product");
            var actions = new Dictionary<int, Action>()
            {
                {1, GetSalesByPeriod },
                {2, GetSalesByEmployee },
                { 3, GetSalesByProduct }
            };

            if (int.TryParse(Console.ReadLine(), out int option) && actions.TryGetValue(option, out var selectedAction))
            {
                selectedAction();
            }
            else
            {
                throw new Exception("Invalid Option");
            }
        }

        private void GetSalesByPeriod()
        {
            try
            {
                Console.WriteLine("Write the period in years to get the Sales:");
                Console.WriteLine("Format: IntialYear-EndYear");
                var periodString = Console.ReadLine().Trim().Split("-");

                var initialYear = DateTime.Parse(periodString[0] + "-1-" + "1");
                var endYear = DateTime.Parse(periodString[1] + "-12-" + "31");
                //Gets the List filtered by the Year
                var salesLog = SalesLogDAL.GetAllBy(p => p.Time >= initialYear && p.Time <= endYear);

                if (salesLog is null)
                {
                    throw new ArgumentNullException("Invalid Period!");
                }

                Console.Clear();
                foreach (var sales in salesLog)
                {
                    TotalPeriodSales += sales.TotalPrice;
                    Console.WriteLine(sales);
                }

                Console.WriteLine($"Total Period Sales: ${TotalPeriodSales.ToString("F2", CultureInfo.InvariantCulture)}");

                Console.ReadLine();
                GetBackToMenu();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private void GetSalesByEmployee()
        {
            try
            {
                Console.Write("Write the Employee Name or iD: ");
                var input = Console.ReadLine().TrimStart().TrimEnd();
                //verifies if is Name or iD
                var verifier = int.TryParse(input, out var id);
                Console.Clear();
                //True for int
                //False for string
                if (verifier)
                {
                    var salesLog = SalesLogDAL.GetAllBy(e => e.EmployeeId == id);
                    if (salesLog is null)
                    {
                        throw new ArgumentNullException("Sale not Found!");
                    }

                    foreach (var sales in salesLog)
                    {
                        TotalPeriodSales += sales.TotalPrice;
                        Console.WriteLine(sales);
                    }
                }
                else
                {
                    var salesLog = SalesLogDAL.GetAllBy(e => e.Employee == input);
                    if (salesLog is null)
                    {
                        throw new ArgumentNullException("Employee not found!");
                    }
                    foreach (var sales in salesLog)
                    {
                        TotalPeriodSales += sales.TotalPrice;
                        Console.WriteLine(sales);
                    }
                }

                Console.WriteLine($"Total Employee Sales: ${TotalPeriodSales.ToString("F2", CultureInfo.InvariantCulture)}");

                Console.ReadLine();
                GetBackToMenu();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private void GetSalesByProduct()
        {
            try
            {
                Console.Write("Write the product iD: ");
                var piD = int.Parse(Console.ReadLine());
                var salesLog = SalesLogDAL.GetAllBy(p => p.ProductId == piD);
                if (salesLog is null)
                {
                    throw new ArgumentNullException("Sale not Found!");
                }

                Console.Clear();
                foreach (var sales in salesLog)
                {
                    TotalPeriodSales += sales.TotalPrice;
                    Console.WriteLine(sales);
                }

                Console.WriteLine($"Total Period Sales: {TotalPeriodSales.ToString("F2", CultureInfo
                    .InvariantCulture)}");

                Console.ReadLine();
                GetBackToMenu();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
