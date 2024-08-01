using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityRoomSlotPanel;
using UnityRoomController;
using MahjongRoomStructure;
using UnityAuthStructure;

namespace UnityRoomPanel
{
    
    public class RoomPanel : MonoBehaviour {
        public Text roomTitleText;
        public RoomSlotPanel[] slots;  
        public Button addButton;
        public Button startButton;
        private int onlinePlayerCounts;

        private void Update() {
            DisplayJoinedPlayersList();
        }
        private void DisplayJoinedPlayersList(){
            ReceivedRoomData data = RoomController.GetRoomDatas();

            int length = 0;
            if (data.roomInfo.roomId != null)
            {
                length = data.userinfo.Length;
                SetInformation(data.roomInfo.roomId, data.userinfo[0].userId, data.roomInfo.maxOfPlayer, length);
                for (int i = 0; i < data.userinfo.Length; i++)
                {
                    slots[i].gameObject.SetActive(true);
                    if (i == 0){
                        slots[i].Set(true, data.userinfo[i].userName, true);
                    } else 
                    {
                        slots[i].Set(false, data.userinfo[i].userName, true);
                        
                    }
                }
                for (int i = length; i < slots.Length; i++)
                {
                    slots[i].gameObject.SetActive(false);
                }
            }
        }

        private void SetInformation(string roomName, string Id, string roomSize, int length){
            ReceivedUserData receivedUserData = AuthStructure.Instance.GetUserData();
            roomTitleText.text = roomName;
            onlinePlayerCounts = length;
            if (Id == receivedUserData.userId)
            {
                if (onlinePlayerCounts == 4)
                {
                    startButton.interactable = true;
                    addButton.interactable = false;
                }
                else
                {
                    addButton.interactable = true;
                    startButton.interactable = false;
                }
            }
            else
            {
                startButton.interactable = false;
                addButton.interactable = false;
            }
        }

        public void AddBot(){
            ReceivedRoomData data = RoomController.GetRoomDatas();
            ReceivedUserData receivedUserData = AuthStructure.Instance.GetUserData();
            RoomController.MATMATCH_MAKINGCH(data.roomInfo.roomId, Convert.ToInt32(data.roomInfo.maxOfPlayer), Convert.ToInt32(data.roomInfo.roomType), Convert.ToInt32(data.roomInfo.roomLevel), false);

        }

        public void StartGame(){
            ReceivedRoomData data = RoomController.GetRoomDatas();
            RoomController.StartGame(data.roomInfo.roomId);
        }

        public void ExitRoom()
        {
            ReceivedUserData receivedUserData = AuthStructure.Instance.GetUserData();
            ReceivedRoomData data = RoomController.GetRoomDatas();
            RoomController.LEAVE_ROOM(data.roomInfo.roomId, receivedUserData.userId);

        }
    }
}