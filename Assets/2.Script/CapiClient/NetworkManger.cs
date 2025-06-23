using System;
using System.Net;
using System.Net.Sockets;


public class NetworkManager
{
    public static Socket clientSocket;
    public static byte[] ip;
    public static int port;

    public event Action<byte[]> OnDataReceived;
    public event Action OnConnected;

    private IPAddress connectIp;
    private int connectPort;

    public void Connect(IPAddress ipAdress, int portNumber)
    {
        // UnityEngine.Debug.Log("넷웟 연결시도해보기");

        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // IPAddress ipAddress = new IPAddress(ip);

        //임시 로컬
        connectIp = ipAdress;
        connectPort = portNumber;


        IPEndPoint endPoint = new IPEndPoint(connectIp, connectPort);
        clientSocket.BeginConnect(endPoint, ConnectCallback, null);
    }

    private void ConnectCallback(IAsyncResult result)
    {
        try
        {
            UnityEngine.Debug.Log("컨넥콜백");
            clientSocket.EndConnect(result);
            OnConnected?.Invoke();
            BeginReceive();
        }
        catch
        {
            UnityEngine.Debug.Log("연결 재시도");
            Connect(connectIp, connectPort); // retry
        }
    }

    private void BeginReceive()
    {
        byte[] headerBuffer = new byte[2];
        if (clientSocket.Connected)
        {
            clientSocket.BeginReceive(headerBuffer, 0, 2, 0, ReceiveCallback, headerBuffer);
        }

    }

    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            UnityEngine.Debug.Log("매니저에서 받음");
            byte[] msgLengthBuff = result.AsyncState as byte[]; //받을그릇을 2개로 받기 - 메시지 길이 정의
            ushort msgLength = EndianChanger.NetToHost(msgLengthBuff);

            byte[] recvBuffer = new byte[msgLength];
            byte[] recvData = new byte[msgLength];
            int recv = 0;
            int recvIdx = 0;
            int rest = msgLength;
            do
            {
                recv = clientSocket.Receive(recvBuffer);
                Buffer.BlockCopy(recvBuffer, 0, recvData, recvIdx, recv);
                recvIdx += recv;
                rest -= recv;
                recvBuffer = new byte[rest];//퍼올 버퍼 크기 수정
                if (recv == 0)
                {
                    //만약 남은게있으면 어떡함?
                    break;
                }
            } while (rest >= 1);

            OnDataReceived?.Invoke(recvData);
            BeginReceive();
        }
        catch
        {
            // 연결 실패 또는 중단 처리
        }
    }

    public void Send(byte[] data)
    {
        // DebugManager.instance.EnqueMessege("매니저에서 샌드");
        ushort length = (ushort)data.Length;
        byte[] header = EndianChanger.HostToNet(length);
        byte[] packet = new byte[header.Length + data.Length];
        Buffer.BlockCopy(header, 0, packet, 0, header.Length);
        Buffer.BlockCopy(data, 0, packet, header.Length, data.Length);
        //clientSocket.Send(packet);


        int totalSent = 0;
        int totalLength = packet.Length;

        while (totalSent < totalLength)
        {
            int sent = clientSocket.Send(packet, totalSent, totalLength - totalSent, SocketFlags.None);
          //  UnityEngine.Debug.Log($"{totalLength} 중 : {sent}만큼 보냄");
            if (sent == 0)
            {
                Console.WriteLine("데이터 전송 중 상대방 연결 종료");
                return;
            }
            totalSent += sent;
        }
    }

    public void Disconnect()
    {
        clientSocket?.Close();
        clientSocket?.Dispose();
    }
}


public class EndianChanger
{

    public static ushort NetToHost(byte[] _bigEndians)
    {
        return (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(_bigEndians));
    }

    public static byte[] HostToNet(ushort _length)
    {
        byte[] lengthByte = BitConverter.GetBytes((ushort)IPAddress.HostToNetworkOrder((short)_length));
        return lengthByte;
    }
}

public class ParseCurIP
{
    public static string GetLocalIP()
    {
        string result = string.Empty;

        var host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                result = ip.ToString();
                return result;
            }


        }
        return null;
    }
}
