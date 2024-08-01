using System;
using UnityEngine;
using SocketIOClient;

namespace UnitySocketManager
{
    public class SocketManager : MonoBehaviour {
        private static SocketManager instance;

        public static SocketManager Instance
        {
            get { return instance; }
        }

        private SocketIOUnity socket;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetSockets(SocketIOUnity socket)
        {
            this.socket = socket;

        }

        public SocketIOUnity GetSockets()
        {
            return socket;
        }

        public void Destroy()
        {
            if (socket != null)
            {
                socket = null; // Reset the reference to the socket
            }
        }

        public static void DestroySocket(){
              if (Instance != null)
            {
                Instance.Destroy();
            }
        }

        public static void SetSocket(SocketIOUnity socket)
        {
            Instance.SetSockets(socket);
        }

        public static SocketIOUnity GetSocket()
        {
            return Instance.GetSockets();
        }       
    }
}
