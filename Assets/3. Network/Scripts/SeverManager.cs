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

		// 127.0.0.1 내 로컬 컴퓨터를 가리킴
		public string ipAddress = "127.0.0.1"; // IPv6와의 호환성을 위해 string을 주로 사용한다.
		//public byte[] ipAddressArray = { 127, 0, 0, 1 }; <먼소리지
		public int port = 9998; // 80이전의 포트는 사실상 거의 선점이 되어있음 9000번대이상을 쓰는게 좋다 
								// 0~65,535 => ushort 사이즈의 숫자만 취급할 수 있으나 (port주소는 2바이트의 부호없는 정수를 사용)
								// C#에서는 int사용

		public bool isConnected = false;
		private Thread severMainThread;

		public static Queue<string> log = new Queue<string>(); 
		//모든 스레드가 접근할 수 있는 Data 영역의 Queue 

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
                //서버를 열고
                severMainThread = new Thread(SeverThread);
                severMainThread.IsBackground = true;
                severMainThread.Start();
                isConnected = true;
            }
            else
            {
                //서버를 닫기
                severMainThread.Abort();
                isConnected = false;
            }
        }

		// 통신에도 활용되지만, 데이터 입출력 등 데이터의 전송을 책임지는 Input, output 스트림이 필요함
		private StreamReader reader;
		private StreamWriter writer;

		private void SeverThread() // 멀티스레드로 생성이 되어야 함
        {
			//오늘 과제
			// 서버 스레드를 List로 관리하여
			// 다중 연결이 가능한 서버로 만들어보세요.


			//File.WriteAllText(ipAddress, messagePrefab.text);

			TcpListener tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
			tcpListener.Start(); // tcp서버를 가동시킨다.

			//Text logText = Instantiate(messagePrefab, textArea);
			//logText.text = "서버 시작";
			log.Enqueue("서버 시작");


			TcpClient tcpClient = tcpListener.AcceptTcpClient(); // 대기가 걸린다.
			//Text logText2 = Instantiate(messagePrefab, textArea);
			//logText2.text = "클라이언트 연결됨";
			//Thread.sleep(1000); 전체 연산잉 멈추기때문에 메인에서 쓰면안됨
			log.Enqueue("클라이언트 연결됨");


			reader = new StreamReader(tcpClient.GetStream());
			writer = new StreamWriter(tcpClient.GetStream());
			writer.AutoFlush = true;

			//writer.WriteLine("메시지");
			//writer.WriteLine("메시지");
			//writer.WriteLine("메시지");
			//writer.Flush(); 이걸 자동으로 해줌

			while(tcpClient.Connected)
            {
				string readString = reader.ReadLine();
				if(string.IsNullOrEmpty(readString))
                {
					continue;
                }
				//Text messageText = Instantiate(messagePrefab, textArea);
				//messageText.text = readString;
				// 받은 메세지를 그대로 writer에 쓴다.

				writer.WriteLine($"당신의 메세지 : {readString}");

				log.Enqueue($" client message : {readString}");
            }

			log.Enqueue("클라이언트 연결 종료");
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
