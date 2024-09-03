using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class Server : MonoBehaviour
{
	[SerializeField] RectTransform logContent;
	[SerializeField] TMP_Text logTextPrefab;
	[SerializeField] TMP_InputField ipField;
	[SerializeField] TMP_InputField portField;

	private TcpListener listener;
	private List<TcpClient> clients = new List<TcpClient>();
	private List<TcpClient> disconnect = new List<TcpClient>();

	private IPAddress ip;
	private int port;

	private bool isOpened;
	public bool IsOpened { get { return isOpened; } }

	private void Start()
	{
		IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
		ip = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
		ipField.text = ip.ToString();
	}

	private void OnDestroy()
	{
		if(isOpened)
		{
			Close();
		}
	}

	private void Update()
	{
		if(!isOpened)
		{
			return;
		}

		foreach(TcpClient client in clients)
		{
			if(CheckClient(client) == false)
			{
				client.Close();
				disconnect.Add(client);
				continue;
			}

			NetworkStream stream = client.GetStream();
			if(stream.DataAvailable)
			{
				StreamReader reader = new StreamReader(stream);
				string text = reader.ReadLine();
				AddLog(text);
				Debug.Log($"Server receive message {text}");
				SendAll(text);
			}
		}

		foreach(TcpClient client in disconnect)
		{
			clients.Remove(client);
		}
		disconnect.Clear();
	}

	public void Open()
	{
		if (isOpened)
		{
			return;
		}

		Debug.Log("Try to Open");
		port = int.Parse(portField.text);

		try
		{
			listener = new TcpListener(IPAddress.Any, port);
			listener.Start();

			isOpened = true;
			listener.BeginAcceptTcpClient(AcceptCallback, listener);
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

	public void Close()
	{
		listener?.Stop();
		listener = null;
		isOpened = false;
	}

	public void SendAll(string chat)
	{
		Debug.Log($"Server send message {chat}");
		foreach (TcpClient client in clients)
		{
			NetworkStream strean = client.GetStream();
			StreamWriter writer = new StreamWriter(strean);

			try
			{
				writer.WriteLine(chat);
				writer.Flush();
			}
			catch(Exception ex)
			{
				Debug.Log(ex.Message);
			}
		}
	}

	private void AcceptCallback(IAsyncResult ar)
	{
		if(!isOpened)
		{
			return;
		}

		TcpClient client = listener.EndAcceptTcpClient(ar);
		clients.Add(client);
		Debug.Log("Client connected");
		listener.BeginAcceptTcpClient(AcceptCallback, listener);
		Debug.Log("Client Begin");
	}

	private void AddLog(string message)
	{
		Debug.Log($"[Server] {message}");
		TMP_Text newLog = Instantiate(logTextPrefab, logContent);
		newLog.text = message;
	}

	private bool CheckClient(TcpClient client)
	{
		try
		{
			if(client == null)
			{
				return false;
			}
			if(client.Connected == false)
			{
				return false;
			}

			bool check = client.Client.Poll(0, SelectMode.SelectRead);
			if(check == false)
			{
				return false;
			}

			int size = client.Client.Receive(new byte[1], SocketFlags.Peek);
			if(size == 0)
			{
				return false;
			}

			return true;
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			return false;
		}
	}
}
