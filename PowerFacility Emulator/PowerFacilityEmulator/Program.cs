
using System.Net.Sockets;
using Newtonsoft.Json;
using PowerFacilityEmulator.Model;

namespace PowerFacilityEmulator
{
    class Program
    {
        static private Facility _facility;
                
 
        /*//string host = "127.0.0.1";
        //int port = 8888;
        TcpClient client = new TcpClient();
        //Console.Write("Введите свое имя: ");
        string? userName = Console.ReadLine();
        //Console.WriteLine($"Добро пожаловать, {userName}");
        StreamReader? Reader = null;
        StreamWriter? Writer = null;*/

        static void Main(string[] args)
        {
            /*_facility = new Facility(args);
            ShowInitialData();*/

            Client client = new Client(args);
            client.StartClientAsync();

            Console.ReadKey();
        }

        

       
    }
}