using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnitySocketManager;
using UnityEngine.UI;
using MahJongController;
using MahjongRoomStructure;
using UnityAuthStructure;



namespace UnityRoomController
{
    public class RoomController : MonoBehaviour {

        public static ReceivedRoomData GetRoomDatas(){
            ReceivedRoomData roomData = RoomStructure.Instance.GetRoomData();
            return roomData;
        }

        public static void JoinRoom(string roomId, string userId, bool flag){
            Communication instances = FindObjectOfType<Communication>();
            if (instances == null)
            {
                GameObject CommunicationObjects = new GameObject("Communication");
                instances = CommunicationObjects.AddComponent<Communication>();
            }
            instances.JoinRoom(SocketManager.GetSocket(), userId, roomId, flag);
        }

        public static void StartGame(string roomId){
            ReceivedUserData receivedUserData = AuthStructure.Instance.GetUserData();
            Communication communication = new Communication();
            communication.StartGame(SocketManager.GetSocket(), roomId, receivedUserData.userId);
        }

        public static void LEAVE_ROOM(string roomId, string userId)
        {
            Communication communication = new Communication();
            communication.LEAVE_ROOM(SocketManager.GetSocket(), roomId, userId);
        }
        public static void MATMATCH_MAKINGCH(string userId, int maxofPlayer, int roomType, int roomLevel, bool state)
        {
            Communication instances = FindObjectOfType<Communication>();
            if (instances == null)
            {
                GameObject CommunicationObjects = new GameObject("Communication");
                instances = CommunicationObjects.AddComponent<Communication>();
            }
            //instances.JoinRoom(SocketManager.GetSocket(), userId, roomId, flag);
            instances.MATMATCH_MAKINGCH(userId, SocketManager.GetSocket(), maxofPlayer, roomType, roomLevel, state);
        }
    }
}