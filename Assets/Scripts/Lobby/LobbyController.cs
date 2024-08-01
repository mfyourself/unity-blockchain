using UnityEngine;
using UnitySocketManager;
using UnityEngine.UI;
using UnityAuthStructure;
using MahJongController;

namespace MahjongLobbyController
{
    public class LobbyController : MonoBehaviour
    {
        public static int roomType = 2;
        public static int roomLevel = 1;
        public static int limitTime = 1;
        public static int maxofPlayer = 4;

        public static void CreateGameRoom()
        {
            ReceivedUserData receivedUserData = AuthStructure.Instance.GetUserData();
            Communication instance = FindObjectOfType<Communication>();
            roomLevel = LobbyController.GetMyLevel(receivedUserData);
            
            if (instance == null)
            {
                GameObject lobbyPanelObject = new GameObject("Communication");
                instance = lobbyPanelObject.AddComponent<Communication>();
            }
            Debug.Log("roomType" + roomType + "roomLevel" + roomLevel);
            //instance.CreateGameRoom(receivedUserData.userId, SocketManager.GetSocket(), maxofPlayer, roomType, roomLevel, limitTime);
            instance.MATMATCH_MAKINGCH(receivedUserData.userId, SocketManager.GetSocket(), maxofPlayer, roomType, roomLevel, true);
        }

        public void CreatFourPlayerEastRoomHanchan()
        {
            roomType = 2;
            CreateGameRoom();
        }

        public void CreatFourPlayerEastWind()
        {
            roomType = 1;
            CreateGameRoom();
        }
        public static int GetMyLevel(ReceivedUserData receivedUserData)
        {
            int level, experience = receivedUserData.userExperience;
            if (experience < 300)
                level = 1;
            else if (experience < 600 && experience > 300)
                level = 2;
            else if (experience >= 600 && experience < 1400)
                level = 3;
            else if (experience >= 1400 && experience < 2000)
                level = 4;
            else if (experience >= 2000 && experience < 9000)
                level = 5;
            else level = 6;
            return level;
        }
    }
}