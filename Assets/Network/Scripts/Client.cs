using System;
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

	private void Update()
	{
		if(isConnected == false)
		{
			return;
		}

		if(stream.DataAvailable == false)
		{
			return;
		}

		string text = reader.ReadLine();
		ReceiveChat(text);
	}

	public void Connect()
	{
		if (isConnected)
		{
			return;
		}

		clienName = nameField.text;
		ip = ipField.text;
		port = int.Parse(portField.text);

		try
		{
			client = new TcpClient(ip, port);
			stream = client.GetStream();
			writer = new StreamWriter(stream);
			reader = new StreamReader(stream);

			Debug.Log("Connect Success");
			isConnected = true;
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

	public void DisConnect()
	{
		writer?.Close();
		writer = null;
		reader?.Close();
		reader = null;
		stream?.Close();
		stream = null;
		client?.Close();
		client = null;

		isConnected = false;
	}

	public void SendChat(string chatText)
	{
		if (isConnected == false)
		{
			return;
		}

		Debug.Log($"Client send message : {chatText}");

		try
		{
			writer.WriteLine($"{clienName} : {chatText}");
			writer.Flush();
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

	public void ReceiveChat(string chatText)
	{
		Debug.Log($"Client receive message : {chatText}");
		chat.AddMessage(chatText);
	}

	private void AddMessage(string message)
	{
		Debug.Log($"[Client] {message}");
		chat.AddMessage($"[Client] {message}");
	}
}
