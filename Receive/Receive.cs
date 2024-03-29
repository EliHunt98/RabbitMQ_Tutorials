﻿using Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

class Receive
{
    static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        using (var connection = factory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {

                channel.QueueDeclare(queue: "Orders",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    OrderAdded OrderAdded = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderAdded>(message);
                    Console.WriteLine(" [x] Received {0}", message);

                };
                channel.BasicConsume(queue: "Orders",
                                    autoAck: true,
                                    consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }


        }
    }
}

