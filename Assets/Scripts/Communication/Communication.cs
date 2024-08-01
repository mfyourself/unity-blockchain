using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using SocketIOClient;
using UnityEngine.Events;
using UnitySocketManager;
using System.Collections.Generic;
using UnityAuthStructure;
using MahjongLobbyController;
using MahjongRoomStructure;
using System.Net.NetworkInformation;
using UnityRoomController;
using System.Threading;

namespace MahJongController
{
    public class SocketEventHandler : UnityEvent<string> { }

    public class Communication : MonoBehaviour {

        static SocketIOUnity socket;
        private List<string> packets;
        private readonly object _lockObject;

        public SocketEventHandler OnReceiveDataOfStartGame;
        public SocketEventHandler OnReceiveCreatedRoom;
        public SocketEventHandler OnReceiveJoinedRoom;
        public SocketEventHandler OnReceiveLeftedRoom;
        public SocketEventHandler OnReceiveLeavedRoom;
        public SocketEventHandler OnReceiveCreatedAccount;
        public SocketEventHandler OnReceiveCreateAccountFailed;
        public SocketEventHandler OnReceiveLoginAccount;
        public SocketEventHandler OnReceiveLoginAccountFailed;
        public void Awake(){

            packets = new List<string>();
            socket = SocketManager.GetSocket();
            socket.On("GAME_INFO", (response)=>{
                GameInfo1.Instance.ReceiveJsonParser(response.ToString());
                Debug.Log(response.ToString());
                HandleSocketEvent(packets, () =>
                {
                    packets.Add("GAME_INFO");
                });
                
            });

            socket.On("CREATED_ROOM", (response) => {
                RoomStructure roomStructure = new RoomStructure();
                roomStructure.ReceivedJsonDataOfRoom(response.ToString());
                HandleSocketEvent(packets, () =>
                {
                    packets.Add("CREATED_ROOM");
                });
               
            });

            socket.On("JOINED_ROOM", (response) => {
                Debug.Log("------ssss---" + response.ToString());
                RoomStructure roomStructure = new RoomStructure();
                roomStructure.ReceivedJsonDataOfRoom(response.ToString());

                HandleSocketEvent(packets, () =>
                {
                    packets.Add("JOINED_ROOM");
                });

            }); 
            
            socket.On("LEFT_PLAYER", (response) =>
            {
                Debug.Log("------dddddd---" + response.ToString());
                RoomStructure roomStructure = new RoomStructure();
                roomStructure.ReceivedJsonDataOfRoom(response.ToString());

                HandleSocketEvent(packets, () =>
                {
                    packets.Add("LEFT_PLAYER");
                });
            });
            
            socket.On("LEAVED_ROOM", (response) =>
            {
                Debug.Log("------dddddd---" + response.ToString());
                RoomStructure roomStructure = new RoomStructure();
                roomStructure.ReceivedJsonDataOfRoom(response.ToString());

                HandleSocketEvent(packets, () =>
                {
                    packets.Add("LEAVED_ROOM");
                });
              
            });

            socket.On("CREATED_USER", (response) =>
            {
                AuthStructure authStructure = new AuthStructure();
                authStructure.ReceivedJsonDataOfAuth(response.ToString());

                HandleSocketEvent(packets, () =>
                {
                    packets.Add("CREATED_USER");
                });
              
            });

            socket.On("ALREADY_EXIT", (response) =>
            {

                HandleSocketEvent(packets, () =>
                {
                    packets.Add("ALREADY_EXIT");
                });

              
            });

            socket.On("LOGINED_USER", (response) =>
            {

                

                AuthStructure authStructure = new AuthStructure();
                authStructure.ReceivedJsonDataOfAuth(response.ToString());
                HandleSocketEvent(packets, () =>
                {
                    packets.Add("LOGINED_USER");
                });
            });

            socket.On("LOGIN_FAILED", (response) =>
            {

                HandleSocketEvent(packets, () =>
                {
                    packets.Add("LOGIN_FAILED");
                });
            });

            OnReceiveDataOfStartGame = new SocketEventHandler();
            OnReceiveLeavedRoom = new SocketEventHandler();
            OnReceiveCreatedRoom = new SocketEventHandler();
            OnReceiveJoinedRoom = new SocketEventHandler();
            OnReceiveLeftedRoom = new SocketEventHandler();
            OnReceiveCreatedAccount = new SocketEventHandler();
            OnReceiveCreateAccountFailed = new SocketEventHandler();
            OnReceiveLoginAccount = new SocketEventHandler();
            OnReceiveLoginAccountFailed = new SocketEventHandler();

            OnReceiveDataOfStartGame.AddListener(JoinGameRoom);
            OnReceiveLeavedRoom.AddListener(LeaveGameRoom);
            OnReceiveCreatedRoom.AddListener(HandleCreatedRoom);
            OnReceiveJoinedRoom.AddListener(HandleJoinedRoom);
            OnReceiveLeftedRoom.AddListener(HandleJoinedRoom);
            OnReceiveCreatedAccount.AddListener(CreatedAccount);
            OnReceiveCreateAccountFailed.AddListener(AccountCreateFailed);
            OnReceiveLoginAccount.AddListener(LoginAccount);
            OnReceiveLoginAccountFailed.AddListener(LoginAccountFailed);
        }

        private void OnDestroy() {
            OnReceiveLeavedRoom.RemoveListener(LeaveGameRoom);
            OnReceiveDataOfStartGame.RemoveListener(JoinGameRoom);
            OnReceiveCreatedRoom.RemoveListener(HandleCreatedRoom);
            OnReceiveJoinedRoom.RemoveListener(HandleJoinedRoom);
            OnReceiveLeftedRoom.RemoveListener(HandleJoinedRoom);
            OnReceiveCreatedAccount.RemoveListener(CreatedAccount);
            OnReceiveCreateAccountFailed.RemoveListener(AccountCreateFailed);
            OnReceiveLoginAccount.RemoveListener(LoginAccount);
            OnReceiveLoginAccountFailed.RemoveListener(LoginAccountFailed);
        }

        private void Update()
        {

            //using (new DisposableLock(packets))
            HandleSocketEvent(packets, () =>
            {
                int i;
                bool isFound = false;
                for (i = 0; i < packets.Count; i++)
                {
                    if (packets[i] == "GAME_INFO")
                    {
                        isFound = true;
                        OnReceiveDataOfStartGame.Invoke("GAME_INFO");
                        break;
                    }
                    if (packets[i] == "CREATED_ROOM")
                    {
                        isFound = true;
                        OnReceiveCreatedRoom.Invoke("CREATED_ROOM");
                        break;
                    }

                    if (packets[i] == "JOINED_ROOM")
                    {
                        isFound = true;
                        OnReceiveJoinedRoom.Invoke("JOINED_ROOM");
                        break;
                    }

                    if (packets[i] == "LEFT_PLAYER")
                    {
                        isFound = true;
                        OnReceiveLeftedRoom.Invoke("LEFT_PLAYER");
                        break;
                    }
                    if (packets[i] == "LEAVED_ROOM")
                    {
                        isFound = true;
                        OnReceiveLeavedRoom.Invoke("LEAVED_ROOM");
                        break;
                    }
                    if (packets[i] == "CREATED_USER")
                    {
                        isFound = true;
                        OnReceiveCreatedAccount.Invoke("CREATED_USER");
                        break;
                    }
                    if (packets[i] == "ALREADY_EXIT")
                    {
                        isFound = true;
                        OnReceiveCreateAccountFailed.Invoke("ALREADY_EXIT");
                        break;
                    }
                    if (packets[i] == "LOGINED_USER")
                    {
                        isFound = true;
                        OnReceiveLoginAccount.Invoke("LOGINED_USER");
                        break;
                    }
                    if (packets[i] == "LOGIN_FAILED")
                    {
                        isFound = true;
                        OnReceiveLoginAccountFailed.Invoke("LOGIN_FAILED");
                        break;
                    }
                }

                if (isFound)
                    packets.RemoveAt(i);
            });   
        }



        private void JoinGameRoom(string data)
        {
            if (!IsSceneLoaded("MahJong_Game")) { 
                SceneManager.LoadScene("MahJong_Game", LoadSceneMode.Single); 
            }
        }

        private void CreatedAccount(string data)
        {
            if (!IsSceneLoaded("MahJong_Lobby"))
            {
                SceneManager.LoadScene("MahJong_Lobby", LoadSceneMode.Single);
            }
        }

        private void AccountCreateFailed(string data)
        {
            GameObject obj = FindObjectOfType<Canvas>().gameObject;
            GameObject objWarning = obj.transform.GetChild(4).gameObject;
            WarningPanel warningPanel = objWarning.GetComponent<WarningPanel>();
            warningPanel.Show(400, 200, "メールアドレスまたはユーザー名は既に存在します。");
            return;
        }

        private void LoginAccount(string data)
        {
            if (!IsSceneLoaded("MahJong_Lobby"))
            {
                SceneManager.LoadScene("MahJong_Lobby", LoadSceneMode.Single);
            }
        }

        private void LoginAccountFailed(string data)
        {
            GameObject obj = FindObjectOfType<Canvas>().gameObject;
            GameObject objWarning = obj.transform.GetChild(4).gameObject;
            WarningPanel warningPanel = objWarning.GetComponent<WarningPanel>();
            warningPanel.Show(400, 200, "ユーザー名またはパスワードが正しくありません。");
            return;
        }
        
        private void LeaveGameRoom(string data)
        {
            if (!IsSceneLoaded("MahJong_Lobby")) { 
                SceneManager.LoadScene("MahJong_Lobby", LoadSceneMode.Single); 
            }
        }

        private void HandleCreatedRoom(string data)
        {
            if (!IsSceneLoaded("MahJong_Room")) { 
                SceneManager.LoadScene("MahJong_Room", LoadSceneMode.Single); 
            }
        }

        private void HandleJoinedRoom(string data)
        {
            if (!IsSceneLoaded("MahJong_Room")) { 
                SceneManager.LoadScene("MahJong_Room", LoadSceneMode.Single); 
            }
        } 




        public void LoginPlayer(string userPassword, string userEmail, SocketIOUnity socket)
        {
            socket.Emit("LOGIN_USER", new {
                userEmail = userEmail,
                userPassword = userPassword,
            });
        }

        public void CreateAccounts(string userName, string userPassword, string userEmail, string avatar, SocketIOUnity socket)
        {
            socket.Emit("CREATE_USER", new {
                email = userEmail,
                password = userPassword,
                name = userName,
                avatar = avatar
            });
        }

        public void CreateGameRoom(string userId, SocketIOUnity socket, int maxOfPlayer, int roomType, int roomLevel, int limitTime)
        {
            socket.Emit("CREATE_ROOM", new {
                playerId = userId,
                maxOfPlayer = maxOfPlayer,
                roomType = roomType,
                roomLevel = roomLevel,
                limitTime = limitTime
            });

        }

        public void JoinRoom(SocketIOUnity socket, string userId, string roomId, bool flag)
        {
            socket.Emit("JOIN_ROOM", new {
                userId = userId,
                roomName = roomId,
                flag = flag
            });

        }

        public void LEAVE_ROOM(SocketIOUnity socket, string roomId, string userId)
        {
            socket.Emit("LEAVE_ROOM", new
            {
                userId = userId,
                roomId = roomId,
            });

        }

        public void StartGame(SocketIOUnity socket, string roomId, string userId) {
            socket.Emit("START_GAME", new {
                roomId = roomId,
                userId = userId
            });
        }

        bool IsSceneLoaded(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(i);
                if (loadedScene.name == sceneName)
                {
                    return true;
                }
            }
            return false;
        }

        public static void Send() {
            GameInfoData gameInfo = GameInfo1.Instance.GetSendGameData();
            string jsonData = GameInfo1.Instance.SendJsonStringify();
            string roomId = (gameInfo.roomInfo.roomId).ToString();
            Debug.Log("sendingdata------"+jsonData);
            socket.Emit("GAME", new {
                roomId = roomId,
                gameInfo = jsonData
            });
        }

        public void MATMATCH_MAKINGCH(string userId, SocketIOUnity socket, int maxOfPlayer, int roomType, int roomLevel, bool flag)
        {
            socket.Emit("MATCH_MAKING", new
            {
                playerId = userId,
                maxOfPlayer = maxOfPlayer,
                roomType = roomType,
                roomLevel = roomLevel,
                flag = flag
            });

        }

        private void HandleSocketEvent(object lockObject, Action callback)
        {
            LockHelper lockHelper = null;
            try
            {
                lockHelper = new LockHelper(lockObject);
                callback();
            }
            finally
            {
                if (lockHelper != null)
                {
                    lockHelper.Dispose();
                }
            }
        }
    }
    public class LockHelper : IDisposable
    {
        private readonly object _lockObject;

        public LockHelper(object lockObject)
        {
            _lockObject = lockObject;
            Monitor.Enter(_lockObject);
        }

        public void Dispose()
        {
            Monitor.Exit(_lockObject);
        }
    }

 

}
