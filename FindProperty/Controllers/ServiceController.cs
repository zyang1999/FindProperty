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
      
        const string ServiceBusConnectionString = "Endpoint=sb://servicebustp000000.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=asWb/NHtFsrxzISfC0BUN6iDWSawBm2+xem5O9Bpl1k=";
        const string QueueName = "AppointmentQueue";
        static IQueueClient queueClient;
        static List<string> items;

        //Part 1: Send Message to the Service Bus
        public void Index()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            try
            {
                for (var i = 0; i < 10; i++)
                {
                    // Create a new message to send to the queue.
                    string messageBody = $"Message {i}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    // Write the body of the message to the console.
                    Console.WriteLine($"Sending message: {messageBody}");

                    // Send the message to the queue.
                     queueClient.SendAsync(message);
                    ViewBag.msg = "success";
                }
            }
            catch (Exception exception)
            {
                ViewBag.msg = exception.ToString();
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
