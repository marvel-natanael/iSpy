using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

//This class is to handle client connection to the matchmaker
public class MatchmakerClient : MonoBehaviour
{
    public static MatchmakerClient instance;
    public static int dataBufferSize = 4096;

    public readonly ushort id;
    public readonly ushort port;
    public TCP transport;

    private delegate void PacketHandler(Packet _packet);

    private static Dictionary<int, PacketHandler> packetHandlers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log($"Client instance already exists, destroying object");
            Destroy(this);
        }
    }

    private void Start()
    {
        transport = new TCP();
    }

    private MatchmakerClient()
    {
    }

    public MatchmakerClient(ushort _id)
    {
        id = _id;
    }

    public void Connect()
    {
        InitializeClientData();
        try
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                transport.Connect(new TcpClient(endPoint.Address.ToString(), 27015));
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Exception caught in Client.cs: {e}");
        }
    }

    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private byte[] receiveBuffer;

        private Packet receivedPacket;

        /// <summary>
        /// This function connects the server to the matchmaker as a client.
        /// </summary>
        /// <remarks>
        /// This function acts as a connection opener for the server to the matchmaker.
        /// Once the server is connected to the matchmaker, it will begin to read any incoming data from the matchmaker.
        /// If data is received, <c>ReceiveCallback</c> will be called.
        /// </remarks>
        /// <param name="_socket">Socket to connect to</param>
        public void Connect(TcpClient _socket)
        {
            socket = _socket;
            socket.ReceiveBufferSize = dataBufferSize;
            socket.SendBufferSize = dataBufferSize;

            stream = socket.GetStream();

            receiveBuffer = new byte[dataBufferSize];

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        /// <summary>
        /// This function is called when the client's NetworkStream has received a stream
        /// </summary>
        /// <remarks>
        /// If a stream is received by the server, it will try to copy the stream into <c>receiveBuffer</c>.
        /// After copying, the buffer will run through <c>HandleData</c> check to see if the packet is whole or segmented.
        /// </remarks>
        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _bytelength = stream.EndRead(_result);
                if (_bytelength <= 0)
                {
                    return;
                }

                byte[] _data = new byte[_bytelength];
                Array.Copy(receiveBuffer, _data, _bytelength);

                receivedPacket.Reset(HandleData(_data));

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception thrown: {e}");
                //todo: disconnect
            }
        }

        /// <summary>
        /// Send data to the matchmaker using TCP
        /// </summary>
        /// <param name="dataStream">packet to be sent</param>
        public void SendData(Packet dataStream)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(dataStream.ToArray(), 0, dataStream.Length(), null, null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending data to matchmaker: {e}");
            }
        }

        /// <summary>
        /// This function handles incoming data from matchmaker.
        /// </summary>
        /// <remarks>
        /// Basically, when a packet is received, it has a chance of it not being whole and, if not handled correctly, may result in data loss.
        /// This may happen because once a packet is used, the content will get discarded for the next incoming packet.
        /// To avoid this, we have to check if the unread length is less than the packet length contained.
        /// If the length contained is different from the data unread, then the data is segmented and some information has yet to come.
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool HandleData(byte[] data)
        {
            int packetLength = 0;

            receivedPacket.SetBytes(data);
            if (receivedPacket.UnreadLength() >= 4)
            {
                packetLength = receivedPacket.ReadInt();
                if (packetLength <= 0)
                {
                    return true;
                }
            }

            while (packetLength > 0 & packetLength <= receivedPacket.UnreadLength())
            {
                byte[] packetBytes = receivedPacket.ReadBytes(packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(packetBytes))
                    {
                        int _packetID = _packet.ReadInt();
                        packetHandlers[_packetID](_packet);
                    }
                });

                packetLength = 0;
                if (receivedPacket.UnreadLength() >= 4)
                {
                    packetLength = receivedPacket.ReadInt();
                    if (packetLength <= 0)
                    {
                        return true;
                    }
                }
            }
            if (packetLength <= 1)
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Initialize the different types of client packets.
    /// </summary>
    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            {(int) MatchmakerPackets.initRequest, ClientHandle.HandleInit},
            {(int) MatchmakerPackets.updateRequest, ClientHandle.HandleUpdateReq},
            {(int) MatchmakerPackets.terminateRequest, ClientHandle.HandleTerminationReq}
        };
        Debug.Log($"Initialized packets...");
    }
}