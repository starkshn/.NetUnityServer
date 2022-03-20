using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class GameSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected {endPoint}");
            byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome To MMO RPG Server!"); // 이 문자열을 buffer로 만듦
            Send(sendBuff);

            Thread.Sleep(1000);

            Disconnect();
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected {endPoint}");
        }

        public override void OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"From Client : {recvData}");
        }
        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes {numOfBytes}");
        }
        class Program
        {
            static Listener _listener = new Listener();

            static void Main(string[] args)
            {
                // DNS
                // www.NY.com -> 172.123.123.12
                string host = Dns.GetHostName(); // host이름 찾기
                IPHostEntry iphost = Dns.GetHostEntry(host);
                IPAddress ipAddr = iphost.AddressList[0]; // 여기서 원하는 ip주소를 뱉어줌 (ipAddr이 IP를 가지고있음)
                IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); // 최종적으로 endPoint가 최종주소 7777은 포트(정문인지 후문인지)

                _listener.Init(endPoint, () => { return new GameSession(); });
                Console.WriteLine("Listening . . . ");

                while (true)
                {
                    ;
                }
            }
        }
    }
}