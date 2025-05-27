using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;

namespace SalesSystem.Entities
{
    internal class SalesLog
    {
        [Key]
        public int SaleId { get; private set; }

        public int EmployeeId { get; private set; }
        public string Employee { get; private set; }

        public int ProductId { get; private set; }
        public string Product { get; private set; }

        public int Quantity { get; private set; }
        public double TotalPrice { get; private set; }
        public DateTime Time { get; private set; }

        public SalesLog() { }
        public SalesLog(Employee employee, Product product, int quantity, double totalPrice)
        {
            Employee = employee.Name;
            EmployeeId = employee.Id;

            Product = product.Name;
            ProductId = product.Id;

            Quantity = quantity;
            TotalPrice = totalPrice;
            Time = DateTime.Now;
        }

        //Generates/Open and Write the Log File
        public void LogFile(SalesLog log)
        {
            string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
            Directory.CreateDirectory(logFolder);
            string path = logFolder + "/salesLog.json";

            try
            {
                using (FileStream fs = new(path, FileMode.OpenOrCreate)) { }
                using (StreamWriter sw = File.AppendText(path))
                {
                    var jsonOptions = new JsonSerializerOptions()
                    {
                        WriteIndented = true
                    };
                    var logJson = JsonSerializer.Serialize(log, jsonOptions);
                    sw.WriteLine(logJson);
                    sw.WriteLine();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

        }

        public override string ToString()
        {
            return $"Sale iD: {SaleId}\n" +
                "Employee information:\n" +
                $"iD: {EmployeeId}\n" +
                $"Name: {Employee}\n" +
                "Product Information:\n" +
                $"iD: {ProductId}\n" +
                $"Name: {Product}\n" +
                $"Quantity: {Quantity}\n" +
                $"Total Price: ${TotalPrice.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"At: {Time.ToString("MM/dd/yyyy hh:mm:ss")}\n\n";
        }
    }
}
