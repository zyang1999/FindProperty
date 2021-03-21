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
using FindProperty.Models;
using Microsoft.Azure.ServiceBus.Management;

namespace FindProperty.Controllers
{
    public class ServiceController : Controller
    {

        const string ServiceBusConnectionString = "Endpoint=sb://servicebustp046685.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=asWb/NHtFsrxzISfC0BUN6iDWSawBm2+xem5O9Bpl1k=";
        const string QueueName = "appointmentqueue";
        public static List<string> appointments = new List<string>();
        public IQueueClient queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
         private static async Task CreateQueueFunctionAsync()
        {
            var managementClient = new ManagementClient(ServiceBusConnectionString);
            bool queueExists = await managementClient.QueueExistsAsync(QueueName);
            if (!queueExists)
            {
                QueueDescription qd = new QueueDescription(QueueName);
                qd.MaxSizeInMB = 1024;
                qd.MaxDeliveryCount = 10;
                await managementClient.CreateQueueAsync(qd);
            }
        }

        //Part 1: Send Message to the Service Bus
        public void Index(string Message)
        {      
            try
            {
                CreateQueueFunctionAsync().Wait();
                string messageBody = Message;
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                // Write the body of the message to the console.
                Console.WriteLine($"Sending message: {messageBody}");

                // Send the message to the queue.
                queueClient.SendAsync(message);
                ViewBag.msg = "success";
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }

       
        public ActionResult RegisterMessageHandler(string searchString, string status, DateTime date)
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            System.Threading.Thread.Sleep(3000);
            queueClient.CloseAsync().Wait();
            return RedirectToAction("Index", "Appointments", new { searchString, status, date});
        }

        public async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"{Encoding.UTF8.GetString(message.Body)}");
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
            appointments.Add(Encoding.UTF8.GetString(message.Body));
        }

        public Task ExceptionHandler(ExceptionReceivedEventArgs exceptionMessage)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionMessage.Exception}.");
            var context = exceptionMessage.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
