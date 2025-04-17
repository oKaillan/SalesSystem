using Microsoft.EntityFrameworkCore;
using SalesSystem.Database;
using SalesSystem.Entities;

namespace SalesSystem.MenuSytem
{
    internal class ProductRegisterMenu : MenuModel<Product>
    {
        public override string ScreenText()
        {
            return "* PRODUCT REGISTER MENU  *";
        }
        public override void Options(DAL<Product> productDAL)
        {
            var context = new SalesSystemContext();
            var categoryDAL = new DAL<ProductCategory>(context);

            char loop = 'y';
            while (loop.ToString().ToLower() == 'y'.ToString())
            {
                try
                {
                    Console.Write("Product Name: ");
                    string pName = Console.ReadLine().TrimStart().TrimEnd();
                    var pNameVerifier = productDAL.GetBy(p => p.Name.ToLower().Equals(pName.ToLower()));
                    //Verifies if Product Name is valid and not empty
                    if (string.IsNullOrEmpty(pName) || pName.Length <= 0)
                    {
                        throw new ArgumentOutOfRangeException("Invalid Name!");
                    }
                    if (pNameVerifier is not null)
                    {
                        throw new Exception("This Product Name is already registered!");
                    }

                    Console.Write("Product Category: ");
                    var categoryInput = Console.ReadLine().TrimStart().TrimEnd();
                    var pCategory = categoryDAL.GetBy(c => c.Name.ToLower() == categoryInput.ToLower());

                    if (pCategory is null)
                    {
                        throw new ArgumentNullException("This category does not exist!");
                    }

                    Console.Write("Product Quantity: ");
                    int pQuantity = int.Parse(Console.ReadLine());

                    //Verifies if Product Quantity is not negative
                    if (pQuantity < 0)
                    {
                        throw new ArgumentOutOfRangeException("Product Quantity can't be less than 0");
                    }

                    Console.Write("Product Price: $");
                    double pPrice = double.Parse(Console.ReadLine());
                    //Verifies if Product Price is not negative or equal 0
                    if (pPrice <= 0)
                    {
                        throw new ArgumentOutOfRangeException("Product Price can't be negative or equal 0");
                    }

                    Product product = new(pName, pQuantity, pPrice, pCategory);
                    productDAL.Create(product);
                    Console.WriteLine("Product Registered with Success!");

                    Console.WriteLine("Write [Y] to register another Product or Write [N] to Main Menu");
                    loop = char.Parse(Console.ReadLine().ToLower());
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }

            }

            GetBackToMenu();
        }
    }
}
