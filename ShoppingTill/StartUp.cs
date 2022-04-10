using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingTill
{
    public class StartUp
    {
        static void Main()
        {
            //List that keeps all the products that the store is offering.
            List<Product> storeInventory = new List<Product>();

            while (true)
            {
                Console.WriteLine("Write END in order to finish adding products");

                Console.WriteLine("Write new unique product name: ");

                string name = Console.ReadLine();

                if (name == "END")
                {
                    break;
                }

                Console.WriteLine("Write a price for the new product in clouds: ");

                string value = Console.ReadLine();

                if (value == "END")
                {
                    break;
                }

                decimal price;

                bool result = decimal.TryParse(value, out price);

                if (result == true)
                {
                    storeInventory.Add(new Product(name, price));
                }
            }

            Console.WriteLine("Write up to 3 products you want to buy separated with ' ':");

            List<string> productsToBuy = Console.ReadLine().Split(" ").ToList();

            string currentproduct = productsToBuy[0].TrimStart('"').TrimEnd('"');

            if (currentproduct == "")
            {
                Console.WriteLine("Try again");
            }
            else
            {
                List<Product> products = FillProducts(productsToBuy, storeInventory);

                decimal totalPrice = 0;

                bool deal2For3 = Deal2For3(products, ref totalPrice);

                bool dealbuy1Get1HalfPrice = DealBuy1Get1HalfPrice(products, ref totalPrice);

                if (deal2For3)
                {
                    Console.WriteLine("Deal 2 for 3 is ON");
                }

                if (dealbuy1Get1HalfPrice)
                {
                    Console.WriteLine("Deal buy 1 get 1 half price is ON");
                }

                if (totalPrice > 99)
                {
                    Console.WriteLine($"Total price is {(int)totalPrice / 100} aws and {totalPrice % 100} clouds");
                }
                else
                {
                    Console.WriteLine($"Total price is {totalPrice % 100} clouds");
                }
            }
        }

        //In case the client wants to buy two pieces of the same product, he gets one of them at half price.
        private static bool DealBuy1Get1HalfPrice(List<Product> products, ref decimal totalPrice)
        {
            bool dealbuy1Get1HalfPrice = false;

            if (products.Count == 1)
            {
                totalPrice = products[0].Price;
            }
            else if (products.Count == 2)
            {
                if (products[0].Name == products[1].Name)
                {
                    dealbuy1Get1HalfPrice = true;
                    totalPrice = products[0].Price + products[1].Price / 2;
                }
            }

            return dealbuy1Get1HalfPrice;
        }

        //In case the client wants to buy three products, he gets the cheapest one for free.
        private static bool Deal2For3(List<Product> products, ref decimal totalPrice)
        {
            bool deal2For3 = false;

            if (products.Count == 3)
            {
                if (products[0].Price <= products[1].Price && products[0].Price <= products[2].Price)
                {
                    deal2For3 = true;

                    totalPrice += products[1].Price;
                    totalPrice += products[2].Price;
                }
                else if (products[1].Price <= products[0].Price && products[1].Price <= products[2].Price)
                {
                    deal2For3 = true;

                    totalPrice += products[0].Price;
                    totalPrice += products[2].Price;
                }
                else if (products[2].Price <= products[0].Price && products[2].Price <= products[1].Price)
                {
                    deal2For3 = true;

                    totalPrice += products[0].Price;
                    totalPrice += products[1].Price;
                }
            }
            else
            {
                totalPrice = products.Select(x => x.Price).Sum();
            }

            return deal2For3;
        }

        //Check if the products that the client wants to buy are available in the store inventory.
        private static List<Product> FillProducts(List<string> productsToBuy, List<Product> storeInventory)
        {
            List<Product> products = new List<Product>();

            for (int i = 0; i < productsToBuy.Count; i++)
            {
                string currentProduct = productsToBuy[i].TrimStart('"').TrimEnd('"');

                bool containsItem = storeInventory.Any(item => item.Name == currentProduct);

                if (containsItem == true)
                {
                    int index = storeInventory.FindIndex(a => a.Name == currentProduct);

                    products.Add(new Product(currentProduct, storeInventory[index].Price));
                }
                else
                {
                    Console.WriteLine($"We do not sell {currentProduct}. Please try with a different product.");
                }
            }

            return products;
        }
    }
}
