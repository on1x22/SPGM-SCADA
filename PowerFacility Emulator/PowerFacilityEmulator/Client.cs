using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using PowerFacilityEmulator.Model;
using Newtonsoft.Json;

namespace PowerFacilityEmulator
{
    internal class Client
    {
        Facility _facility;
        string host;
        int port;
        TcpClient client = new TcpClient();
        StreamReader? Reader = null;
        StreamWriter? Writer = null;

        internal Client(string[] args)
        {
            _facility = new Facility(args);
            host = _facility.ServerIPEndPoint.Split(new char[] { ':' })[0];
            port = Convert.ToInt32(_facility.ServerIPEndPoint.Split(new char[] { ':' })[1]);

            ShowInitialData();
        }
        /* string host = "127.0.0.1";
        int port = 8888;
        using TcpClient client = new TcpClient();
        Console.Write("Введите свое имя: ");
        string? userName = Console.ReadLine();
        Console.WriteLine($"Добро пожаловать, {userName}");
        StreamReader? Reader = null;
        StreamWriter? Writer = null;*/

        private void ShowInitialData()
        {
            Console.WriteLine(DateTime.Now + $" Открыт эмулятор для объекта {_facility.FacilityName}. " +
                $"IP-адрес {_facility.FacilityIPEndPoint}. IP-адрес сервера {_facility.ServerIPEndPoint}");
            Console.WriteLine("Исходные параметры:");
            foreach (TopologicalNode node in _facility.TopologicalNodes)
            {
                Console.WriteLine($"Узел {node.Number}: {node.TopologicalNodeValues}");
            }
            Console.WriteLine("Ожидается подключение к серверу");
        }

        internal async Task StartClientAsync()
        {
            try
            {
                client.Connect(host, port); //подключение клиента
                Reader = new StreamReader(client.GetStream());
                Writer = new StreamWriter(client.GetStream());
                // запускаем новый поток для получения данных
                Task.Run(() => ReceiveMessageAsync(Reader));
                // запускаем ввод сообщений
                await SendMessageAsync(Writer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Writer?.Close();
                Reader?.Close();
            }
        }

        // отправка сообщений
        async Task SendMessageAsync(StreamWriter writer)
        {
            // сначала отправляем имя
            var setting = new JsonSerializerSettings();
            setting.ReferenceLoopHandling= ReferenceLoopHandling.Ignore;
            await writer.WriteLineAsync(_facility.FacilityIPEndPoint);
            await writer.FlushAsync();
            string message = JsonConvert.SerializeObject(_facility, setting);
            await writer.WriteLineAsync(message);
            await writer.FlushAsync();
            Console.WriteLine(DateTime.Now + " " + message);
            ValuesEmulator emulator = new ValuesEmulator(_facility);

            while (true)
            {
                message = JsonConvert.SerializeObject(await emulator.GenerateValuesAcync());
                await writer.WriteLineAsync(message);
                await writer.FlushAsync();
                Console.WriteLine(DateTime.Now + " " + message);
            }
        }
        // получение сообщений
        async Task ReceiveMessageAsync(StreamReader reader)
        {
            /*while (true)
            {
                try
                {
                    // считываем ответ в виде строки
                    string? message = await reader.ReadLineAsync();
                    // если пустой ответ, ничего не выводим на консоль
                    if (string.IsNullOrEmpty(message)) continue;
                    Print(message);//вывод сообщения
                }
                catch
                {
                    break;
                }
            }*/
        }

        /*try
        {
            client.Connect(host, port); //подключение клиента
            Reader = new StreamReader(client.GetStream());
            Writer = new StreamWriter(client.GetStream());
            if (Writer is null || Reader is null) return;
            // запускаем новый поток для получения данных
            Task.Run(()=>ReceiveMessageAsync(Reader));
            // запускаем ввод сообщений
            await SendMessageAsync(Writer);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        Writer?.Close();
        Reader?.Close();

        // отправка сообщений
        async Task SendMessageAsync(StreamWriter writer)
        {
            // сначала отправляем имя
            await writer.WriteLineAsync(userName);
            await writer.FlushAsync();
            Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");

            while (true)
            {
                string? message = Console.ReadLine();
                await writer.WriteLineAsync(message);
                await writer.FlushAsync();
            }
        }
        // получение сообщений
        async Task ReceiveMessageAsync(StreamReader reader)
        {
            while (true)
            {
                try
                {
                    // считываем ответ в виде строки
                    string? message = await reader.ReadLineAsync();
                    // если пустой ответ, ничего не выводим на консоль
                    if (string.IsNullOrEmpty(message)) continue;
                    Print(message);//вывод сообщения
                }
                catch
                {
                    break;
                }
            }
        }
        // чтобы полученное сообщение не накладывалось на ввод нового сообщения
        void Print(string message)
        {
            if (OperatingSystem.IsWindows())    // если ОС Windows
            {
                var position = Console.GetCursorPosition(); // получаем текущую позицию курсора
                int left = position.Left;   // смещение в символах относительно левого края
                int top = position.Top;     // смещение в строках относительно верха
                // копируем ранее введенные символы в строке на следующую строку
                Console.MoveBufferArea(0, top, left, 1, 0, top + 1);
                // устанавливаем курсор в начало текущей строки
                Console.SetCursorPosition(0, top);
                // в текущей строке выводит полученное сообщение
                Console.WriteLine(message);
                // переносим курсор на следующую строку
                // и пользователь продолжает ввод уже на следующей строке
                Console.SetCursorPosition(left, top + 1);
            }
            else Console.WriteLine(message);
        }*/
    }
}
