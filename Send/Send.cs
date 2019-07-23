using System;
using RabbitMQ.Client;
using System.Text;
using Contracts;

class Send
{
    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost"};
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "Orders",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            OrderAdded OrderAdded = new OrderAdded
            {
                OrderID = 12345,
                Customer = new Customer { CustomerID = 678, FullName = "Johnathan" }
            };
            string OrderAddedString = Newtonsoft.Json.JsonConvert.SerializeObject(OrderAdded);
            var body = Encoding.UTF8.GetBytes(OrderAddedString);
            
            channel.BasicPublish(exchange: "",
                                 routingKey: "Orders",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine(" [x] Sent {0}", OrderAddedString);
        }

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}