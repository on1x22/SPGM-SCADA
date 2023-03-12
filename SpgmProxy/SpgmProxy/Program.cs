using System.Net;
using System.Net.Sockets;
using SpgmProxy;

ServerObject server = new ServerObject();// создаем сервер
await server.ListenAsync(); // запускаем сервер

