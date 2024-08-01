using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAuthStructure;


namespace MahjongLobbyController
{

    public class ModalRankDeny : MonoBehaviour
    {
        private void Start()
        {
            isMyLevel();
        }
        public void isMyLevel()
        {
            ReceivedUserData receivedUserData = AuthStructure.Instance.GetUserData();
            int mine = LobbyController.GetMyLevel(receivedUserData);
            if(LobbyController.roomLevel > mine)
            {
                this.gameObject.SetActive(false);
            }
                
        }
    }

}