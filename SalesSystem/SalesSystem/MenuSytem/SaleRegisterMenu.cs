using SalesSystem.Database;
using SalesSystem.Entities;
using System.Globalization;

namespace SalesSystem.MenuSytem
{
    internal class SaleRegisterMenu : MenuModel<SalesLog>
    {
        public override string ScreenText()
        {
            return "*   SALE REGISTER MENU   *";
        }
        public override void Options(DAL<SalesLog> soldDAL)
        {
            #region DALs&Context
            var context = new SalesSystemContext();
            //DALs
            var empDAL = new DAL<Employee>(context);
            var prodDAL = new DAL<Product>(context);
            #endregion

            char loop = 'y';
            while (loop.ToString().ToLower() == 'y'.ToString())
            {
                try
                {
                    Console.Write("Write the Product iD or Name: ");
                    var pIdentifier = Console.ReadLine();
                    int pId;

                    //Verifies if searching Product is by Name or iD
                    //If True, is int(iD)
                    //False, is String(Name)
                    var verifier = int.TryParse(pIdentifier, out pId);

                    Product product;
                    //Gets the Product and show in the screen
                    if (verifier)
                    {
                        product = prodDAL.GetBy(p => p.Id == pId);
                        Console.WriteLine(product);
                    }
                    else
                    {
                        product = prodDAL.GetBy(p => p.Name.ToUpper() == pIdentifier.ToUpper());
                        Console.WriteLine(product);
                    }


                    Console.Write("Enter the Quantity to sell: ");
                    int pQuantity = int.Parse(Console.ReadLine());
                    if (pQuantity > product.Quantity)
                    {
                        throw new Exception("The quantity choosen is bigger than this Product Stock Quantity");
                    }


                    Console.Write("The total price is: $");
                    var totalPrice = product.GetTotalPrice(pQuantity);
                    Console.WriteLine(totalPrice.ToString("F2", CultureInfo.InvariantCulture));

                    Console.Write("\nEnter the Seller iD: ");

                    int selleriD = int.Parse(Console.ReadLine());

                    //Gets the employee
                    var seller = empDAL.GetBy(e => e.Id == selleriD);
                    if (seller is null)
                    {
                        throw new ArgumentNullException("Seller not found");
                    }
                    Console.WriteLine(seller);

                    Console.WriteLine("Do you want to proceed with this sale?");
                    Console.WriteLine("[Y] Yes   [N] No");
                    char saleOption = char.Parse(Console.ReadLine().TrimStart().TrimEnd());
                    if (saleOption.ToString().ToLower() == 'y'.ToString())
                    {
                        var fileLog = new SalesLog();
                        var salesLog = new SalesLog(seller, product, pQuantity, totalPrice);

                        soldDAL.Create(salesLog);
                        fileLog.LogFile(salesLog);
                        Console.WriteLine("\nSale Registered with Success!");
                        product.RemoveStock(pQuantity);
                        prodDAL.Update(product);
                    }
                    else if (saleOption.ToString().ToLower() == 'n'.ToString())
                        GetBackToMenu();
                    else
                    {
                        throw new NotImplementedException();
                    }

                    Console.WriteLine("Do you want to register another Sale?\n[Y] Yes [N] No");
                    loop = char.Parse(Console.ReadLine());
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }

            GetBackToMenu();
        }
    }
}
