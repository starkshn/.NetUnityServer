using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    class Listener
    {
        Socket _listenSocket;
        Func<Session> _sessionFactroy;

        public void Init(IPEndPoint endPoint, Func<Session> sessionFactroy)
        {
            // 문지기
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _sessionFactroy += sessionFactroy;

            // 문지기 교육
            _listenSocket.Bind(endPoint);

            // backlog
            _listenSocket.Listen(10);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            RegisterAccept(args);
        }
        void RegisterAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;

            bool pending =_listenSocket.AcceptAsync(args);
            if (pending == false)
                OnAcceptCompleted(null, args);
        }

        // 이부분 Red Zone이다.
        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args) 
        {
            if (args.SocketError == SocketError.Success)
            {
                Session session = _sessionFactroy.Invoke();
                session.Start(args.AcceptSocket);
                session.OnConnected(args.AcceptSocket.RemoteEndPoint);
                // TODO 실제로 유저가 왔을 때
               
            }
            else
                Console.WriteLine(args.SocketError.ToString());

            RegisterAccept(args);
        }
    }
}
