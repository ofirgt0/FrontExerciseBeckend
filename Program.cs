using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FrontendExercise
{
    public class Program
    {

        private static async Task HandleWebSocketAsync(HttpListenerContext context)
        {
            if (context.Request.IsWebSocketRequest)
            {
                HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);

                // Start a task to send random numbers every 5 seconds
                await SendRandomNumbersAsync(webSocketContext.WebSocket);
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }

        private static async Task SendRandomNumbersAsync(WebSocket webSocket)
        {
            Random random = new Random();

            while (webSocket.State == WebSocketState.Open)
            {
                try
                {
                    int randomNumber = random.Next(1, 101); // Generate a random number between 1 and 100
                    string message = $"Random Number: {randomNumber}";

                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                    await webSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);

                    await Task.Delay(5000); // Wait for 5 seconds before sending the next update
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    break;
                }
            }

            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "WebSocket closed", CancellationToken.None);
        }
            
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            using (HttpListener listener = new HttpListener())
            {
                listener.Prefixes.Add("http://localhost:8080/"); // Specify the URL and port for the WebSocket server
                listener.Start();
                Console.WriteLine("WebSocket server started.");

                while (true)
                {
                    HttpListenerContext context = listener.GetContext();
                    Task.Run(() => HandleWebSocketAsync(context));
                }
            }
        }



        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
