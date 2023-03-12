using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SpgmProxy
{
    internal class ClientObject
    {
        protected internal string Id { get; } = Guid.NewGuid().ToString();
        protected internal StreamWriter Writer { get; }
        protected internal StreamReader Reader { get; }

        TcpClient client;
        ServerObject server; // объект сервера

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            client = tcpClient;
            server = serverObject;
            // получаем NetworkStream для взаимодействия с сервером
            var stream = client.GetStream();
            // создаем StreamReader для чтения данных
            Reader = new StreamReader(stream);
            // создаем StreamWriter для отправки данных
            Writer = new StreamWriter(stream);
        }

        public async Task ProcessAsync()
        {
            try
            {
                // получаем имя пользователя
                string? facilityAddress = await Reader.ReadLineAsync();
                string? message = DateTime.Now + $" Подключение энергообъекта с IP-адресом {facilityAddress}";
                // посылаем сообщение о входе в чат всем подключенным пользователям
                //await server.old__BroadcastMessageAsync(message, Id);
                Console.WriteLine(message);
                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = await Reader.ReadLineAsync();
                        if (message == null) continue;
                        JObject jsonObject= JObject.Parse(message);
                        if(jsonObject != null)
                        {
                            Console.WriteLine(DateTime.Now + " " + jsonObject.ToString());
                        }
                        //message = $"{userName}: {message}";
                        //Console.WriteLine(message);
                        //await server.old__BroadcastMessageAsync(message, Id);
                    }
                    catch (Exception ex)
                    {
                        //message = $"{userName} покинул чат";
                        Console.WriteLine(DateTime.Now + " " + ex.Message);
                        //await server.old__BroadcastMessageAsync(message, Id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.old__RemoveConnection(Id);
            }
        }
        // закрытие подключения
        protected internal void old__Close()
        {
            Writer.Close();
            Reader.Close();
            client.Close();
        }
    }
}
