// See https://aka.ms/new-console-template for more information

using System.Net;
using RestEase;
using Shop_ConsoleAdmin;
using Shop_ConsoleAdmin.Models;

var api = RestClient.For<IProductApi>("http://localhost:5258");

while (true)
{
    Console.WriteLine("WellCome to admin panel: \n-For create new product enter 1 \n-For get product inventory count enter 2 \n-For exit panel enter e");
    var command = Console.ReadLine();
    switch (command)
    {
        case "1":
            await CreateProduct();
            break;
        case "2":
            GetInventoryCount();
            break;
        case "e":
            Environment.Exit(0);
            break;
    }
}

async Task CreateProduct()
{
    var product = new Product();
    Console.WriteLine("Please enter product title:");
    product.Title = Console.ReadLine() ?? "";
    Console.WriteLine("Please enter product stock:"); 
    product.Stock = int.TryParse(Console.ReadLine(), out var stock) ? stock : 1;
    try
    {
        var response = await api.CreateProduct(product);
        Console.WriteLine("product created, id:{0}",response.Id);
    }
    catch (ApiException e)
    {
        var response = e.StatusCode switch
        {
            HttpStatusCode.NotFound => "Not found",
            HttpStatusCode.BadRequest => "Bad request",
            _ => "An unknown problem has occurred."
        };
        Console.WriteLine(response);
    }
}

void GetInventoryCount()
{
    
}
