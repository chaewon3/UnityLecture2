using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using MyProject;

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

        private List<ClientHandler> clients = new List<ClientHandler>();
        private List<Thread> threads = new List<Thread>();

        public static Queue<string> log = new Queue<string>();
        //모든 스레드가 접근할 수 있는 Data 영역의 Queue 

        private void Awake()
        {
            connectButton.onClick.AddListener(SeverConnectButtonClick);
        }

        public void SeverConnectButtonClick()
        {
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
        private int clientID = 0;

        private void SeverThread() // 멀티스레드로 생성이 되어야 함
        {
            // try catch문 의 용도 : 예외상황 발생시  메세지 수동으로 활용 할 수 있도록 함

            //File.WriteAllText(ipAddress, messagePrefab.text);
            try // if 블록 안의 구문에서 에러가 없을 경우
            {
                TcpListener tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
                tcpListener.Start(); // tcp서버를 가동시킨다.

                log.Enqueue("서버 시작");
                while (true)
                {
                    TcpClient client = tcpListener.AcceptTcpClient();

                    ClientHandler handler = new ClientHandler();
                    handler.Connect(clientID ++,this, client);
                    clients.Add(handler);

                    Thread clientThread = new Thread(handler.Run);
                    clientThread.IsBackground = true;
                    clientThread.Start();

                    threads.Add(clientThread);

                    log.Enqueue($"{handler.id}번 클라이언트가 접속됨.");
                }

                ////Text logText = Instantiate(messagePrefab, textArea);
                ////logText.text = "서버 시작";


                //TcpClient tcpClient = tcpListener.AcceptTcpClient(); // 대기가 걸린다.
                //                                                     //Text logText2 = Instantiate(messagePrefab, textArea);
                //                                                     //logText2.text = "클라이언트 연결됨";
                //                                                     //Thread.sleep(1000); 전체 연산잉 멈추기때문에 메인에서 쓰면안됨
                //log.Enqueue("클라이언트 연결됨");


                //reader = new StreamReader(tcpClient.GetStream());
                //writer = new StreamWriter(tcpClient.GetStream());
                //writer.AutoFlush = true;

                ////writer.WriteLine("메시지");
                ////writer.WriteLine("메시지");
                ////writer.WriteLine("메시지");
                ////writer.Flush(); 이걸 자동으로 해줌

                //while (tcpClient.Connected)
                //{
                //    string readString = reader.ReadLine();
                //    if (string.IsNullOrEmpty(readString))
                //    {
                //        continue;
                //    }
                //    //Text messageText = Instantiate(messagePrefab, textArea);
                //    //messageText.text = readString;
                //    // 받은 메세지를 그대로 writer에 쓴다.

                //    writer.WriteLine($"당신의 메세지 : {readString}");

                //    log.Enqueue($" client message : {readString}");
                //}

                //log.Enqueue("클라이언트 연결 종료");
            }
            catch(ArgumentException e) // 다중캐치 가능
            {
                log.Enqueue("파라미터 발생");
                log.Enqueue(e.Message);
            }
            catch (NullReferenceException e)
            {
                log.Enqueue("널 포인터 발생");
                log.Enqueue(e.Message);
            }
            catch (Exception e) // try 문 내의 구문중에 에러 발생 시 호출
            {
                log.Enqueue("에러 발생");
                log.Enqueue(e.Message);
            }
            finally
            {
                //try문 내에서 에러가 발생해도 실행되고 안해도 실행된다.
                //주로, 중간에 흐름이 끊기지 않고 생성된 객체를 해제하는 등의 반드시 필요한 절차를 여기서 수행하게 됨.
                foreach(var thread in threads)
                {
                    thread?.Abort();
                }
            }


        }

        public void Disconnect(ClientHandler client)
        {
            clients.Remove(client);

        }

        public void BroadcastToClients(string message)
        {
            log.Enqueue(message);

            foreach(ClientHandler client in clients)
            {
                client.MessageToClient(message);
            }    
        }

        private void Update()
        {
            if (log.Count > 0)
            {
                Text logText = Instantiate(messagePrefab, textArea);
                logText.text = log.Dequeue();
            }
        }
    }
}

//클라이언트가 TCP 접속 요청을 할 떄마다 해당 클라이언트를 붙들고 있는 객체를 생성한다.
public class ClientHandler
{
    public int id;
    public SeverManager sever;
    public TcpClient tcpClient;
    public StreamReader reader;
    public StreamWriter writer;

    public void Connect(int id, SeverManager sever, TcpClient tcpClient)
    {
        this.id = id;
        this.sever = sever;
        this.tcpClient = tcpClient;
        reader = new StreamReader(tcpClient.GetStream());
        writer = new StreamWriter(tcpClient.GetStream());
        writer.AutoFlush = true;
    }

    public void Disconnect()
    {
        writer.Close();
        reader.Close();
        tcpClient.Close();
        sever.Disconnect(this);
    }

    public void MessageToClient(string message)
    {
        writer.WriteLine(message);
    }

    public void Run()
    {
        try
        {
            while(tcpClient.Connected)
            {
                string readString = reader.ReadLine();
                if(string.IsNullOrEmpty(readString)) // 문자열을 안전하게 널체크하는 방법
                {
                    continue;
                }

                //읽어온 메세지가 있으면 서버에게 전달
                sever.BroadcastToClients($"{id}님의 말 : {readString}"); 
            }
        }
        catch(Exception e)
        {
            SeverManager.log.Enqueue($"{id}번 클라이언트 오류 발생 : {e.Message}");
        }
        finally
        {
            Disconnect();
        }
    }
}
