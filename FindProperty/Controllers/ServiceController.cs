using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Management;
using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Threading;

namespace FindProperty.Controllers
{
    public class ServiceController : Controller
    {

        const string ServiceBusConnectionString = "Endpoint=sb://servicebustp046685.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=asWb/NHtFsrxzISfC0BUN6iDWSawBm2+xem5O9Bpl1k=";
        const string QueueName = "appointmentqueue";
        static IQueueClient queueClient;
        static List<string> appointments;

        //Part 1: Send Message to the Service Bus
        public async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            try
            {
                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    // Create a new message to send to the queue
                    string messageBody = $"Message {i}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    // Write the body of the message to the console
                    Console.WriteLine($"Sending message: {messageBody}");

                    // Send the message to the queue
                    await queueClient.SendAsync(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }

        public List<string> RegisterOnMessageHandlerAndReceiveMessages()
        {
            appointments = new List<string>();

            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            return appointments;
        }

        public async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            //var result = JsonConvert.DeserializeObject<Ostring>(Encoding.UTF8.GetString(message.Body));
            // Console.WriteLine($"Appointment ID: is {result.OrderId}, Order name is {result.OrderName} and quantity is {result.OrderQuantity}");
            //Console.WriteLine($"Date: {result.date}");
            //Console.WriteLine($"Time: {result.hour}");
            //Console.WriteLine($"User Name: {result.user.name}");
            appointments.Add(Encoding.UTF8.GetString(message.Body));
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        public Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
