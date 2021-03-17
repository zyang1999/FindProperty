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
using FindProperty.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace FindProperty.Controllers
{
    public class ServiceController : Controller
    {
      
        const string ServiceBusConnectionString = "Endpoint=sb://servicebustp046685.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=asWb/NHtFsrxzISfC0BUN6iDWSawBm2+xem5O9Bpl1k=";
                                                   
        const string QueueName = "appointmentqueue";
        public const string Namespace = "servicebustp046685";
        static IQueueClient queueClient;
        static List<string> items;
        private UserManager<FindPropertyUser> userManager;
        //Part 1: Send Message to the Service Bus
        public void Index(string Message)
        {
            
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            try
            {
                CreateQueueFunctionAsync();     
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
                ViewBag.msg = exception.ToString();
            }
            
        }

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


        //Part 2: Received Message from the Service Bus - cal get data function
        public async Task<IActionResult> ProcessMsg()
        {
            //queueClient = new QueueClient(ServiceBusConnectionString, QueueName, ReceiveMode.PeekLock);
            items = new List<string>();
            await Task.Factory.StartNew(() =>
            {
                queueClient = new QueueClient(ServiceBusConnectionString, QueueName, ReceiveMode.PeekLock);
                var options = new MessageHandlerOptions(ExceptionMethod)
                {
                    MaxConcurrentCalls = 1,
                    AutoComplete = false
                };
                queueClient.RegisterMessageHandler(ExecuteMessageProcessing, options);
            });

            return RedirectToAction("ProcessMsgResult");
        }

        //Part 2: Received Message from the Service Bus - get data step
        private static async Task ExecuteMessageProcessing(Message message, CancellationToken arg2)
        {
            //var result = JsonConvert.DeserializeObject<Ostring>(Encoding.UTF8.GetString(message.Body));
            // Console.WriteLine($"Order Id is {result.OrderId}, Order name is {result.OrderName} and quantity is {result.OrderQuantity}");
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);

            items.Add("Received message: SequenceNumber:" + message.SystemProperties.SequenceNumber + " Body:" + Encoding.UTF8.GetString(message.Body));

        }

        //Part 2: Received Message from the Service Bus
        private static async Task ExceptionMethod(ExceptionReceivedEventArgs arg)
        {
            await Task.Run(() =>
           Console.WriteLine($"Error occured. Error is {arg.Exception.Message}")
           );
        }

        //Part 2: Received Message from the Service Bus - Display step
        //however, there is a bug where you have to reload the page for second time only can see the result.
        public IActionResult ProcessMsgResult()
        {
            return View(items);
        }
    }
}
