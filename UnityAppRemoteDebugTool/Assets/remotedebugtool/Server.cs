using UnityEngine;
using MessagePack;
using System.Net;
using System.Net.Sockets;
using System.Collections;

public class Server : MonoBehaviour
{
    [SerializeField] private int port = 12345;

    void Start()
    {
        // コルーチンで受信処理を実行
        StartCoroutine(ReceiveData());
    }

    IEnumerator ReceiveData()
    {
        // UDPソケットの作成
        using (UdpClient udpClient = new UdpClient(port))
        {
            // 通信待機
            while (true)
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);

                // データの受信
                byte[] receivedData = udpClient.Receive(ref endPoint);

                // データのデシリアライズ
                MyData receivedObject = MessagePackSerializer.Deserialize<MyData>(receivedData);

                // 受信したデータをログに出力
                Debug.Log($"Data received in ScriptB: {receivedObject.Message}");

                // 通信の結果をメインスレッドで処理
                yield return null;
            }
        }
    }
}
