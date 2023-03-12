using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SpgmProxy
{
    internal class ServerObject
    { 
        TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.10"), port: 8888); // hardcode
        
        // сервер для прослушивания
        List<ClientObject> clients = new List<ClientObject>(); // все подключения
        
        // прослушивание входящих подключений
        protected internal async Task ListenAsync()
        {
            try
            {
                tcpListener.Start();
                Console.WriteLine($"Сервер {tcpListener.LocalEndpoint} запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                    ClientObject clientObject = new ClientObject(tcpClient, this);
                    clients.Add(clientObject);
                    Task.Run(clientObject.ProcessAsync);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                old__Disconnect();
            }
        }

        // трансляция сообщения подключенным клиентам
        protected internal async Task old__BroadcastMessageAsync(string message, string id)
        {
            foreach (var client in clients)
            {
                if (client.Id != id) // если id клиента не равно id отправителя
                {
                    await client.Writer.WriteLineAsync(message); //передача данных
                    await client.Writer.FlushAsync();
                }
            }
        }
        protected internal void old__RemoveConnection(string id)
        {
            // получаем по id закрытое подключение
            ClientObject? client = clients.FirstOrDefault(c => c.Id == id);
            // и удаляем его из списка подключений
            if (client != null) clients.Remove(client);
            client?.old__Close();
        }

        // отключение всех клиентов
        protected internal void old__Disconnect()
        {
            foreach (var client in clients)
            {
                client.old__Close(); //отключение клиента
            }
            tcpListener.Stop(); //остановка сервера
        }
    }
}
