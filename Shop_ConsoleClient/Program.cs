// See https://aka.ms/new-console-template for more information

using System.Net;
using RestEase;
using Shop_ConsoleClient;
using Shop_ConsoleClient.Models;

Console.WriteLine("Hello Word!");
var api = RestClient.For<IOrderApi>("http://localhost:5285");

while (true)
{
    Console.WriteLine("WellCome to admin panel: \n-For create new order enter 1 \n-For exit panel enter e");
    var command = Console.ReadLine();
    switch (command)
    {
        case "1":
            await CreateOrder();
            break;
        case "e":
            Environment.Exit(0);
            break;
    }
}

async Task CreateOrder()
{
    var order = new Order();
    Console.WriteLine("Please enter order product id:");
    order.ProductId = Guid.TryParse(Console.ReadLine(),out var productId) ? productId:Guid.Empty;
    Console.WriteLine("Please enter order product count:");
    order.ProductCount = int.TryParse(Console.ReadLine(), out var productCount) ? productCount : 1;
    Console.WriteLine("Please enter order state:");
    order.State = Console.ReadLine() ?? "unknown";
    try
    {
        var response = await api.CreateOrder(order);
        Console.WriteLine("order created, id:{0}",response);
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
