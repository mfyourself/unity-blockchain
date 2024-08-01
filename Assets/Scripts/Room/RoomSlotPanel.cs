using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace UnityRoomSlotPanel
{
    public class RoomSlotPanel : MonoBehaviour {
        public Image roomMaster;
        public Image readySign;
        public Text playerNameText;

        public void Set(bool isMaster, string playerName, bool isReady)
        {
            roomMaster.gameObject.SetActive(isMaster);
            readySign.gameObject.SetActive(isMaster || isReady);
            playerNameText.text = playerName;
        }
    }
}