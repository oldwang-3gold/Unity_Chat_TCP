using  System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{

    public string ipaddress = "192.168.1.7";
    public int port = 7788;
    private Socket clientSocket;
    public InputField inputField;
    private Thread t;
    private byte[] data=new byte[1024];//接收服务器端消息容器
    public Text show;
    private string message = "";//消息容器

    void Start()
    {
        ConnectToServer();
    }

    void Update()
    {
        if (message != null && message != "")
        {
            show.text += "\n" + message;
            message = "";//清空消息
        }
    }

    void ConnectToServer()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //跟服务器建立连接
        clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ipaddress), port));

        //创建一个新的线程用来接收消息
        t=new Thread(ReceiveMessage);
        t.Start();
    }

    void SendMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        clientSocket.Send(data);
    }
    /// <summary>
    /// 这个线程方法用来循环接受服务器端发来的广播消息
    /// </summary>
    void ReceiveMessage()
    {
        while (true)
        {
            if (clientSocket.Connected == false)
            {
                break;
            }
            int length = clientSocket.Receive(data);
            message = Encoding.UTF8.GetString(data,0,length);
            
            
        }
        

    }

    public void OnButtonSend()
    {
        SendMessage(inputField.text);
    }

    void OnDestroy()
    {
        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
    }
}
