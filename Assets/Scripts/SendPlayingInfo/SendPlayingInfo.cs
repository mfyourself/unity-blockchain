using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnitySocketManager;
using UnityEngine.UI;
using MahjongRoomStructure;
using UnityAuthStructure;

namespace MahJongController
{
    public class SendPlayingInfo : MonoBehaviour
    {
        public static void SendController(int gameEvent, string[] handTiles, string[] saveTiles, string clickedTile)
        {
            ReceivedUserData receivedUserData = AuthStructure.Instance.GetUserData();

            var gameData = GameInfo1.Instance;
            gameData.SetSendGI(gameData.GetRecvGI());
            var sendGI = gameData.GetSendGI();
            string userId = receivedUserData.userId;

            if (gameEvent == 0)
            {
                // sendGI.roomInfo.roomGameEvent = gameEvent.ToString();
            }

            if (gameEvent == 1)
            {
                SaveHandTiles(userId, sendGI, handTiles, clickedTile);
            }
            if (gameEvent == 2 || gameEvent == 3 || gameEvent == 4)
            {
                SaveHandTiles(userId, sendGI, handTiles, clickedTile);
                sendGI.roomInfo.roomEventHint = gameEvent;
                SaveSaveTiles(userId, sendGI, saveTiles);
                changeEvent(userId, sendGI, gameEvent.ToString());
                changeTurn(userId, sendGI);
            }
            if (gameEvent == 5)
            {
                changeTurn(userId, sendGI);
                SaveHandTiles(userId, sendGI, handTiles, clickedTile);
            }
            if (gameEvent == 6 || gameEvent == 7)
            {
                SaveHandTiles(userId, sendGI, handTiles, clickedTile);
            }

            sendGI.roomInfo.roomGameEvent = gameEvent.ToString();
            Communication.Send();
        }

        public static void CancelCPK()
        {
            Debug.Log("Clicked CPK1");
            var gameData = GameInfo1.Instance;
            Debug.Log("Clicked CPK2");
            gameData.SetSendGI(gameData.GetRecvGI());
            var sendGI = gameData.GetSendGI();

            Debug.Log("Clicked CPK3");
            sendGI.roomInfo.roomEventHint = 0;
            sendGI.roomInfo.roomGameEvent = "2";
            //sendGI.roomInfo.order = myPlace;//== 0 ? 3 : myPlace - 1;
            //if (HandTiles.myPlace == sendGI.roomInfo.order)
            Communication.Send();

            Debug.Log("Clicked CPK4");
        }

        public static void SaveHandTiles(string userId, GameInfoData sendGI, string[] handTiles, string clickedTile)
        {
            if (sendGI.user1.userId == userId) { sendGI.user1.handTiles = handTiles; sendGI.user1.clickedTile = clickedTile; }
            if (sendGI.user2.userId == userId) { sendGI.user2.handTiles = handTiles; sendGI.user2.clickedTile = clickedTile; }
            if (sendGI.user3.userId == userId) { sendGI.user3.handTiles = handTiles; sendGI.user3.clickedTile = clickedTile; }
            if (sendGI.user4.userId == userId) { sendGI.user4.handTiles = handTiles; sendGI.user4.clickedTile = clickedTile; }
        }

        public static void SaveSaveTiles(string userId, GameInfoData sendGI, string[] saveTiles)
        {
            if (sendGI.user1.userId == userId) { sendGI.user1.saveTiles = saveTiles; }
            if (sendGI.user2.userId == userId) { sendGI.user2.saveTiles = saveTiles; }
            if (sendGI.user3.userId == userId) { sendGI.user3.saveTiles = saveTiles; }
            if (sendGI.user4.userId == userId) { sendGI.user4.saveTiles = saveTiles; }
        }

        public static void changeEvent(string userId, GameInfoData sendGI, string gameEvent)
        {
            if (sendGI.user1.userId == userId) { sendGI.user1.gameEvent = gameEvent; }
            if (sendGI.user2.userId == userId) { sendGI.user2.gameEvent = gameEvent; }
            if (sendGI.user3.userId == userId) { sendGI.user3.gameEvent = gameEvent; }
            if (sendGI.user4.userId == userId) { sendGI.user4.gameEvent = gameEvent; }
        }

        public static void changeTurn(string userId, GameInfoData sendGI)
        {
            if (sendGI.user1.userId == userId) { sendGI.roomInfo.order = GetMyPlace(sendGI, userId); }
            if (sendGI.user2.userId == userId) { sendGI.roomInfo.order = GetMyPlace(sendGI, userId); }
            if (sendGI.user3.userId == userId) { sendGI.roomInfo.order = GetMyPlace(sendGI, userId); }
            if (sendGI.user4.userId == userId) { sendGI.roomInfo.order = GetMyPlace(sendGI, userId); }
        }

        public static int GetMyPlace(GameInfoData gameInfoData, string userId)
        {
            int myplace = 0;
            List<UserData> users = new List<UserData> { gameInfoData.user1, gameInfoData.user2, gameInfoData.user3, gameInfoData.user4 };
            foreach (var user in users)
            {
                if (user.userId == userId)
                {
                    myplace = user.playerplace;
                    break;
                }
            }
            return myplace;
        }

    }

    public static class ArrayExtensions
    {
        public static int Push<T>(this T[] source, T value)
        {
            var index = Array.IndexOf(source, default(T));

            if (index != -1)
            {
                source[index] = value;
            }

            return index;
        }
    }
}