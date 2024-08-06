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

        // 127.0.0.1 �� ���� ��ǻ�͸� ����Ŵ
        public string ipAddress = "127.0.0.1"; // IPv6���� ȣȯ���� ���� string�� �ַ� ����Ѵ�.
                                               //public byte[] ipAddressArray = { 127, 0, 0, 1 }; <�ռҸ���
        public int port = 9998; // 80������ ��Ʈ�� ��ǻ� ���� ������ �Ǿ����� 9000�����̻��� ���°� ���� 
                                // 0~65,535 => ushort �������� ���ڸ� ����� �� ������ (port�ּҴ� 2����Ʈ�� ��ȣ���� ������ ���)
                                // C#������ int���

        public bool isConnected = false;
        private Thread severMainThread;

        private List<ClientHandler> clients = new List<ClientHandler>();
        private List<Thread> threads = new List<Thread>();

        public static Queue<string> log = new Queue<string>();
        //��� �����尡 ������ �� �ִ� Data ������ Queue 

        private void Awake()
        {
            connectButton.onClick.AddListener(SeverConnectButtonClick);
        }

        public void SeverConnectButtonClick()
        {
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
        private int clientID = 0;

        private void SeverThread() // ��Ƽ������� ������ �Ǿ�� ��
        {
            // try catch�� �� �뵵 : ���ܻ�Ȳ �߻���  �޼��� �������� Ȱ�� �� �� �ֵ��� ��

            //File.WriteAllText(ipAddress, messagePrefab.text);
            try // if ��� ���� �������� ������ ���� ���
            {
                TcpListener tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
                tcpListener.Start(); // tcp������ ������Ų��.

                log.Enqueue("���� ����");
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

                    log.Enqueue($"{handler.id}�� Ŭ���̾�Ʈ�� ���ӵ�.");
                }

                ////Text logText = Instantiate(messagePrefab, textArea);
                ////logText.text = "���� ����";


                //TcpClient tcpClient = tcpListener.AcceptTcpClient(); // ��Ⱑ �ɸ���.
                //                                                     //Text logText2 = Instantiate(messagePrefab, textArea);
                //                                                     //logText2.text = "Ŭ���̾�Ʈ �����";
                //                                                     //Thread.sleep(1000); ��ü ������ ���߱⶧���� ���ο��� ����ȵ�
                //log.Enqueue("Ŭ���̾�Ʈ �����");


                //reader = new StreamReader(tcpClient.GetStream());
                //writer = new StreamWriter(tcpClient.GetStream());
                //writer.AutoFlush = true;

                ////writer.WriteLine("�޽���");
                ////writer.WriteLine("�޽���");
                ////writer.WriteLine("�޽���");
                ////writer.Flush(); �̰� �ڵ����� ����

                //while (tcpClient.Connected)
                //{
                //    string readString = reader.ReadLine();
                //    if (string.IsNullOrEmpty(readString))
                //    {
                //        continue;
                //    }
                //    //Text messageText = Instantiate(messagePrefab, textArea);
                //    //messageText.text = readString;
                //    // ���� �޼����� �״�� writer�� ����.

                //    writer.WriteLine($"����� �޼��� : {readString}");

                //    log.Enqueue($" client message : {readString}");
                //}

                //log.Enqueue("Ŭ���̾�Ʈ ���� ����");
            }
            catch(ArgumentException e) // ����ĳġ ����
            {
                log.Enqueue("�Ķ���� �߻�");
                log.Enqueue(e.Message);
            }
            catch (NullReferenceException e)
            {
                log.Enqueue("�� ������ �߻�");
                log.Enqueue(e.Message);
            }
            catch (Exception e) // try �� ���� �����߿� ���� �߻� �� ȣ��
            {
                log.Enqueue("���� �߻�");
                log.Enqueue(e.Message);
            }
            finally
            {
                //try�� ������ ������ �߻��ص� ����ǰ� ���ص� ����ȴ�.
                //�ַ�, �߰��� �帧�� ������ �ʰ� ������ ��ü�� �����ϴ� ���� �ݵ�� �ʿ��� ������ ���⼭ �����ϰ� ��.
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

//Ŭ���̾�Ʈ�� TCP ���� ��û�� �� ������ �ش� Ŭ���̾�Ʈ�� �ٵ�� �ִ� ��ü�� �����Ѵ�.
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
                if(string.IsNullOrEmpty(readString)) // ���ڿ��� �����ϰ� ��üũ�ϴ� ���
                {
                    continue;
                }

                //�о�� �޼����� ������ �������� ����
                sever.BroadcastToClients($"{id}���� �� : {readString}"); 
            }
        }
        catch(Exception e)
        {
            SeverManager.log.Enqueue($"{id}�� Ŭ���̾�Ʈ ���� �߻� : {e.Message}");
        }
        finally
        {
            Disconnect();
        }
    }
}
