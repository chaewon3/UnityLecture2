using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject
{
	public class ClientManager : MonoBehaviour
	{
		public Button connectButton;
		public Text messagePrefab;
		public RectTransform textArea;

		public InputField ip;
		public InputField port;

		public InputField messageInput;
		public Button sendButton;

		private bool isconnected;
		private Thread clientThread;

		public static Queue<string> log = new Queue<string>();

		private StreamReader reader;
		private StreamWriter writer;

        private void Awake()
        {
            connectButton.onClick.AddListener(ConnectButtonClick);
            messageInput.onSubmit.AddListener(MessageToSever);
        }
        public void ConnectButtonClick()
        {
			if(false == isconnected)
            {
				//서버랑 접속 시도
				clientThread = new Thread(ClientThread);
				clientThread.IsBackground = true;
				clientThread.Start();
				isconnected = true;
            }
            else
            {
				//접속 끊기
				clientThread.Abort();
				isconnected = false;
            }
        }

		private void ClientThread()
        {
			TcpClient tcpClient = new TcpClient();

			IPAddress severAddress = IPAddress.Parse(ip.text);
			int portNum = int.Parse(port.text);

			IPEndPoint endPoint = new IPEndPoint(severAddress, portNum);
			tcpClient.Connect(endPoint);

			log.Enqueue($"서버에 접속됨. IP : {endPoint.Address}");

			reader = new StreamReader(tcpClient.GetStream());
			writer = new StreamWriter(tcpClient.GetStream());
			writer.AutoFlush = true;

			while(tcpClient.Connected)
            {
				string readString = reader.ReadLine();
				log.Enqueue(readString);
            }

			log.Enqueue("접속 종료");
        }

		public void MessageToSever(string message) // inputField의 OnSubmit에서 호출
        {
			writer.WriteLine(message);
			messageInput.text = "";
        }

        private void Update()
        {
            if(log.Count >0)
            {
				Instantiate(messagePrefab, textArea).text = log.Dequeue();
            }
        }
    }

}
