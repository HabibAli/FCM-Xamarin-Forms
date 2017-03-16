using System.Threading.Tasks;
using FirebaseNet.Messaging;
using System;

namespace MessageSender
{
    class MessageSender
    {
      
        

        static void Main(string[] args)
        {
            Task t = new Task(async () =>
            {
                await foo();
            });
            t.Start();


            Console.ReadLine();
        }

        public static async Task<IFCMResponse> foo()
        {
            FCMClient client = new FCMClient("YOUR_APP_SERVER_KEY"); //as derived from https://console.firebase.google.com/project/
            var message = new Message()
            {
                To = "DEVICE_ID_OR_ANY_PARTICULAR_TOPIC", //topic example /topics/all
                Notification = new AndroidNotification()
                {
                    Body = "great match!",
                    Title = "Portugal vs. Denmark",
                }
            };
            var result = await client.SendMessageAsync(message);
            return result;
        }
    }
}