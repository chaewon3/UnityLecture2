using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace MyProject
{
	public class SeverManager : MonoBehaviour
	{
		public Button connectButton;
		public Text messagePrefab;
		public RectTransform textArea;

		// 127.0.0.1 �� ���� ��ǻ�͸� ����Ŵ
		public string ipAddress = "127.0.0.1"; // IPv6���� ȣȯ���� ���� string�� �ַ� ����Ѵ�.
		//public byte[] ipAddressArray = { 127, 0, 0, 1 }; <�ռҸ���
		public int port = 9998; // 80������ ��Ʈ�� ��ǻ� ���� ������ �Ǿ����� 9000�����̻��� ���°� ���� 
								// 0~65,535 => ushort �������� ���ڸ� ����� �� ������ (port�ּҴ� 2����Ʈ�� ��ȣ���� ������ ���)
								// C#������ int���

		public bool isConnected = false;
		private Thread severMainThread;

		public static Queue<string> log = new Queue<string>(); 
		//��� �����尡 ������ �� �ִ� Data ������ Queue 

        private void Awake()
        {
			connectButton.onClick.AddListener(SeverConnectButtonClick);
        }

		public void SeverConnectButtonClick()
        {
			//if(severThreads.Count <5)
   //         {
			//	Thread thread = new Thread(SeverThread);
			//	thread.IsBackground = true;
			//	thread.Start();

			//	severThreads.Add(thread);
			//}

            if (false == isConnected)
            {
                //������ ����
                severMainThread = new Thread(SeverThread);
                severMainThread.IsBackground = true;
                severMainThread.Start();
                isConnected = true;
            }
            else
            {
                //������ �ݱ�
                severMainThread.Abort();
                isConnected = false;
            }
        }

		// ��ſ��� Ȱ�������, ������ ����� �� �������� ������ å������ Input, output ��Ʈ���� �ʿ���
		private StreamReader reader;
		private StreamWriter writer;

		private void SeverThread() // ��Ƽ������� ������ �Ǿ�� ��
        {
			//���� ����
			// ���� �����带 List�� �����Ͽ�
			// ���� ������ ������ ������ ��������.


			//File.WriteAllText(ipAddress, messagePrefab.text);

			TcpListener tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
			tcpListener.Start(); // tcp������ ������Ų��.

			//Text logText = Instantiate(messagePrefab, textArea);
			//logText.text = "���� ����";
			log.Enqueue("���� ����");


			TcpClient tcpClient = tcpListener.AcceptTcpClient(); // ��Ⱑ �ɸ���.
			//Text logText2 = Instantiate(messagePrefab, textArea);
			//logText2.text = "Ŭ���̾�Ʈ �����";
			//Thread.sleep(1000); ��ü ������ ���߱⶧���� ���ο��� ����ȵ�
			log.Enqueue("Ŭ���̾�Ʈ �����");


			reader = new StreamReader(tcpClient.GetStream());
			writer = new StreamWriter(tcpClient.GetStream());
			writer.AutoFlush = true;

			//writer.WriteLine("�޽���");
			//writer.WriteLine("�޽���");
			//writer.WriteLine("�޽���");
			//writer.Flush(); �̰� �ڵ����� ����

			while(tcpClient.Connected)
            {
				string readString = reader.ReadLine();
				if(string.IsNullOrEmpty(readString))
                {
					continue;
                }
				//Text messageText = Instantiate(messagePrefab, textArea);
				//messageText.text = readString;
				// ���� �޼����� �״�� writer�� ����.

				writer.WriteLine($"����� �޼��� : {readString}");

				log.Enqueue($" client message : {readString}");
            }

			log.Enqueue("Ŭ���̾�Ʈ ���� ����");
        }

        private void Update()
        {
            if(log.Count >0)
            {
				Text logText = Instantiate(messagePrefab, textArea);
				logText.text = log.Dequeue();
            }
        }
    }
}
