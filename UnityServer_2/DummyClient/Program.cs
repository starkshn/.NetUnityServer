using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DummyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                // DNS
                string host = Dns.GetHostName();
                IPHostEntry iphost = Dns.GetHostEntry(host);
                IPAddress ipAddr = iphost.AddressList[0];
                IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

                // 휴대폰 설정
                Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    // 문지기한테 입장 문의
                    socket.Connect(endPoint); // server쪽 문지기한테 입장가능한지 물어본다. 인자로 상대방의주소 == endPoint를 넣어준다.
                    Console.WriteLine($"Connected To { socket.RemoteEndPoint.ToString() }"); // socket.RemoteEndPoint == server의 대리인

                    // 보낸다.
                    byte[] sendBuff = Encoding.UTF8.GetBytes("Hello! Server! I Am From Client!");
                    int sendBytes = socket.Send(sendBuff);

                    // 받는다.
                    byte[] recvBuff = new byte[1024];
                    int recvBytes = socket.Receive(recvBuff);
                    string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes); // byte -> string 변환
                    Console.WriteLine($"From Server! : {recvData}");

                    // 나간다.
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                Thread.Sleep(100);
            }
        }
            

    }
}