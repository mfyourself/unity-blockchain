using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityAuthStructure;

namespace MahjongLobbyController
{

    public class LobbyPanel : MonoBehaviour
    {
        public void CreateRoom()
        {
            LobbyController.CreateGameRoom();
        }




    }
}