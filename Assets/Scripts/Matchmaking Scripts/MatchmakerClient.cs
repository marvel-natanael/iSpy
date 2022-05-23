using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

//This class is to handle client connection to the matchmaker
public class MatchmakerClient
{
    private static MatchmakerClient singleton;

    public static MatchmakerClient Singleton
    { get { if (singleton == null) singleton = new MatchmakerClient(); return singleton; } }

    public static int dataBufferSize = 4096;

    private readonly int id;
    public TCP transport = null;
    private bool isConnected = false;

    public int ID => id;

    //private void Awake()
    //{
    //    if (Singleton == null)
    //    {
    //        Singleton = this;
    //    }
    //    else if (Singleton != this)
    //    {
    //        Debug.Log($"Client instance already exists, destroying object");
    //        Destroy(this);
    //    }
    //}

    //private void Start()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}

    /// <summary>
    /// Hidden
    /// </summary>
    private MatchmakerClient()
    {
    }

    public void Initialize()
    {
        if (singleton == null) return;
        Debug.Log($"Matchmaker singleton initialized!");
    }

    /// <summary>
    /// Connect to matchmaker service as a client
    /// </summary>
    /// <param name="address">matchmaker address</param>
    /// <param name="port">matchmaker port</param>
    /// <param name="_isServer">connect as server?</param>
    public void Connect(string address, ushort port)
    {
        if (transport != null) transport = new TCP();

        if (transport.IsConnected) { Debug.LogError($"Transport already connected"); return; }
        try
        {
            transport.Connect(address, port);
        }
        catch (Exception e)
        {
            Debug.LogError($"Exception caught in Client.cs: {e}");
        }
    }

    /// <summary>
    /// Connect to matchmaker service as a server
    /// </summary>
    public void Connect()
    {
        if (transport == null) transport = new TCP(true);

        if (transport.IsConnected) { Debug.LogError($"Transport is already connected"); return; }
        try
        {
            transport.Connect("127.0.0.1", 7777);
        }
        catch (Exception e)
        {
            Debug.LogError($"Exception caught in Client.cs: {e}");
        }
    }

    /// <summary>
    /// Disconnects the matchmaker client from the matchmaker service
    /// </summary>
    public void Disconnect()
    {
        if (isConnected)
        {
            ServerSend.SendDisconnect(null);
            isConnected = false;
            transport.socket.Close();

            Debug.Log($"Disconnected from matchmaker");
        }
    }

    /// <summary>
    /// This class handles the TCP connection between matchmaker and client
    /// </summary>
    public class TCP
    {
        public TcpClient socket;
        private bool isConnected = false;
        public bool IsConnected { get => isConnected; }

        private NetworkStream stream = null;
        private readonly bool isServer = false;
        private int connectionAttempt = 0;
        private readonly int maxConnectionAttempt = 3;

        private string latestAddress;
        private int latestPort;

        private byte[] receiveBuffer;
        private Packet receivedPacket;

        private delegate void PacketHandler(Packet _packet);

        private static Dictionary<int, PacketHandler> packetHandle;

        /// <summary>
        /// Create a new TCP transport
        /// </summary>
        /// <param name="_isServer"><see langword="true"/>, if is a server</param>
        public TCP(bool _isServer = false)
        {
            isServer = _isServer;
            if (_isServer)
            {
                InitializeServerMatchmakerPackets();
            }
            else
            {
                InitializeClientMatchmakerPackets();
            }
        }

        /// <summary>
        /// Opens a connection to matchmaker.
        /// </summary>
        /// <remarks>
        /// This function acts as a connection opener for the client to the matchmaker.
        /// Once the client is connected to the matchmaker, it will begin to read any incoming data from the matchmaker.
        /// If data is received, <c>ReceiveCallback</c> will be called.
        /// </remarks>
        /// <param name="_socket">Socket to connect to</param>
        public void Connect(string endPointAddress, int endpointPort)
        {
            latestAddress = endPointAddress;
            latestPort = endpointPort;
            Debug.Log($"Connecting to matchmaker ({latestAddress}:{latestPort})...");
            socket = new TcpClient()
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };
            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(endPointAddress, endpointPort, ConnectCallback, null);
        }

        /// <summary>
        /// Called when <c>BeginRead</c> has finished
        /// </summary>
        /// <param name="res"></param>
        private void ConnectCallback(IAsyncResult res)
        {
            socket.EndConnect(res);

            if (!socket.Connected)
            {
                Debug.LogWarning($"First connection attempt to matchmaker ({latestAddress}:{latestPort}) failed, retrying...");
                if (connectionAttempt < maxConnectionAttempt)
                {
                    Debug.LogWarning($"Failed to connect to matchmaker service ({latestAddress}:{latestPort}), retrying...");
                    connectionAttempt++;
                    Connect(latestAddress, latestPort);
                }
                else
                {
                    Debug.LogError($"Connection to matchmaker failed ({latestAddress}:{latestPort}), shutting down...");
                    Application.Quit();
                    return;
                }
            }
            else
            {
                Debug.Log($"Connected to matchmaker ({latestAddress}:{latestPort})!");
                isConnected = true;
                stream = socket.GetStream();

                receivedPacket = new Packet();

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
        }

        /// <summary>
        /// This function is called when the client's <c>stream</c> has received a stream
        /// </summary>
        /// <remarks>
        /// If a stream is received by the server, it will try to copy the stream into <c>receiveBuffer</c>.
        /// After copying, the buffer will run through <c>HandleData</c> check to see if the packet is whole or segmented.
        /// </remarks>
        private void ReceiveCallback(IAsyncResult _result)
        {
            Debug.Log($"Received a packet!");
            try
            {
                int _bytelength = stream.EndRead(_result);
                if (_bytelength <= 0)
                {
                    Singleton.Disconnect();
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
                Disconnect();
            }
        }

        /// <summary>
        /// Send data to the matchmaker using TCP
        /// </summary>
        /// <param name="dataStream">packet to be sent</param>
        public void SendData(Packet dataStream)
        {
            Debug.Log($"Sending a packet to matchmaker");
            if (!isConnected) { Debug.LogError($"Matchmaker client is not connected to matchmaker, aborting..."); return; }
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
                    using Packet _packet = new Packet(packetBytes);
                    int _packetID = _packet.ReadInt();
                    packetHandle[_packetID](_packet);
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

        /// <summary>
        /// Disconnects this transport
        /// </summary>
        private void Disconnect()
        {
            Singleton.Disconnect();

            stream = null;
            receiveBuffer = null;
            receivedPacket = null;
            socket = null;
        }

        /// <summary>
        /// Initialize server - matchmaker packets
        /// </summary>
        private void InitializeServerMatchmakerPackets()
        {
            packetHandle = new Dictionary<int, PacketHandler>()
        {
            { (int) MatchmakerServerPackets.terminationRequest, ServerHandle.HandleTerminationReq }
        };
            Debug.Log($"Initialized packets...");
        }

        /// <summary>
        /// Initialize client - matchmaker packets
        /// </summary>
        private void InitializeClientMatchmakerPackets()
        {
            packetHandle = new Dictionary<int, PacketHandler>()
        {
            { (int) MatchmakerClientPackets.updateReply, ClientHandle.HandleUpdate }
        };
        }
    }
}