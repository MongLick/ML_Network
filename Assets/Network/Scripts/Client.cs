using System.IO;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField] Chat chat;

    [SerializeField] TMP_InputField nameField;
    [SerializeField] TMP_InputField ipField;
    [SerializeField] TMP_InputField portField;

    private TcpClient client;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    private string clienName;
    private string ip;
    private int port;

    private bool isConnected;
    public bool IsConnected { get { return isConnected; } }

    public void Connect()
    {
        if(isConnected)
        {
            return;
        }

        clienName = nameField.text;
        ip = ipField.text;
        port = int.Parse(portField.text);

        client = new TcpClient(ip, port);
        stream = client.GetStream();

    }

    public void DisConnect()
    {

    }

    public void SendChat(string chatText)
    {

    }

    public void ReceiveChat(string chatText)
    {

    }

    private void AddMessage(string message)
    {
        Debug.Log($"[Client] {message}");
        chat.AddMessage($"[Client] {message}");
    }
}
