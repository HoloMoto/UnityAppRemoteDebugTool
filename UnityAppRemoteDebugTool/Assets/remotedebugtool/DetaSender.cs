using UnityEngine;
using MessagePack;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class DetaSender : MonoBehaviour
{
    [SerializeField] private string ipAddress = "127.0.0.1";
    [SerializeField] private int port = 12345;

    void Start()
    {
        // サブスレッドで送信処理を実行
        Thread thread = new Thread(SendData);
        thread.Start();
    }

    void SendData()
    {
        // データオブジェクトの作成
        MyData data = new MyData { Message = "Hello from ScriptA" };

        // データのシリアライズ
        byte[] serializedData = MessagePackSerializer.Serialize(data);

        // UDPソケットの作成
        using (UdpClient udpClient = new UdpClient())
        {
            // 送信先のIPアドレスとポート
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);

            // データの送信
            udpClient.Send(serializedData, serializedData.Length, endPoint);
        }

        Debug.Log("Data sent from ScriptA");
    }
}

// 送信するデータのクラス
[MessagePackObject]
public class MyData
{
    [Key(0)]
    public string Message { get; set; }
}
