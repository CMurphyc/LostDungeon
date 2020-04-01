using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class Client
{
    public TcpClient remoteServer;
    public ClientListen sock_s2c;
    public ClientSend sock_c2s;
    Thread c_thread;
    string ip_address;
    int ip_port;
    public Client()
    {
        remoteServer = new TcpClient();  //创建TCPClient类来与服务器通信；
        sock_s2c = new ClientListen(remoteServer);

        sock_c2s = new ClientSend(remoteServer);
    }
    ~Client()
    {
        if (c_thread != null)
            c_thread.Abort();
    }

    public void CreateListenThread()
    {
        c_thread = new Thread(sock_s2c.Received);
        c_thread.IsBackground = true;
        c_thread.Start();

    }
    public void Connect ()
    {
        remoteServer.Connect(IPAddress.Parse(ip_address), ip_port);
    }

    public void SetIP( string str)
    {
        ip_address = str;

    }
    public void SetPort(int port)
    {
        ip_port = port;

    }
   
}
