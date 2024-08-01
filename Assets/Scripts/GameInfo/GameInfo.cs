using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

namespace MahJongController
{
    [Serializable]
    public class GameInfo1 : MonoBehaviour
    {
        
       
        // public static GameInfoData recvGI;
        
        public static GameInfo1 _instance;
        
        public static GameInfo1 Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameInfo1();
                }
                return _instance;
            }
        }
        

        public static GameInfoData sendGI,recvGI;

        public GameInfoData GetSendGI()
        {
            if(sendGI == null) {
                sendGI = new GameInfoData();
            }
            return sendGI;
        }

        public void SetSendGI(GameInfoData value)
        {
            sendGI = value;
        }

        public GameInfoData GetRecvGI()
        {
            if(recvGI == null) {
                recvGI = new GameInfoData();
            }
            return recvGI;
        }

        public void SetRecvGI(GameInfoData value)
        {
            recvGI = value;
        }

        public void ReceiveJsonParser(string recevedString){

            GameInfoData[] dataArray = JsonHelper.FromJsonArray<GameInfoData>(recevedString);
            recvGI = dataArray[0];
            sendGI = dataArray[0];
            foreach (GameInfoData data in dataArray)
            {
                recvGI = new GameInfoData
                {
                    roomInfo = new RoomInfoData
                    {
                        roomId = data.roomInfo.roomId,
                        order = data.roomInfo.order,
                        remineTiles = data.roomInfo.remineTiles.ToList().ToArray(),
                        doraTiles = data.roomInfo.doraTiles.ToList().ToArray(),
                        drawTile = data.roomInfo.drawTile,
                        basedTime = data.roomInfo.basedTime,
                        plusTime = data.roomInfo.plusTime,
                        roomType = data.roomInfo.roomType,
                        roomLevel = data.roomInfo.roomLevel,
                        maxOfPlayer = data.roomInfo.maxOfPlayer,
                        roomCreater = data.roomInfo.roomCreater,
                        roomDealer = data.roomInfo.roomDealer,
                        roundWinner = data.roomInfo.roundWinner,
                        roundFailer = data.roomInfo.roundFailer,
                        roomRound = data.roomInfo.roomRound,
                        roomGameEvent = data.roomInfo.roomGameEvent,
                        roomEventHint = data.roomInfo.roomEventHint,
                        yakuList = data.roomInfo.yakuList.ToList().ToArray(),
                        hiddenDora = data.roomInfo.hiddenDora.ToList().ToArray(),
                        fu = data.roomInfo.fu,
                        han = data.roomInfo.han
                    },
                    user1 = new UserData
                    {
                        userId = data.user1.userId,
                        playerplace = data.user1.playerplace,
                        userName = data.user1.userName,
                        avatar = data.user1.avatar,
                        score = data.user1.score,
                        roundScore = data.user1.roundScore,
                        handTiles = data.user1.handTiles.ToList().ToArray(),
                        saveTiles = data.user1.saveTiles.ToList().ToArray(),
                        throwTiles = data.user1.throwTiles.ToList().ToArray(),
                        experience = data.user1.experience,
                        level = data.user1.level,
                        title = data.user1.title,
                        time = data.user1.time,
                        blueGem = data.user1.blueGem,
                        redGem = data.user1.redGem,
                        rockGem = data.user1.rockGem,
                        yelloGem = data.user1.yelloGem,
                        gameEvent = data.user1.gameEvent,
                        clickedTile = data.user1.clickedTile,
                        eventHint = data.user1.eventHint,
                    },
                    user2 = new UserData
                    {
                        userId = data.user2.userId,
                        playerplace = data.user2.playerplace,
                        userName = data.user2.userName,
                        avatar = data.user2.avatar,
                        score = data.user2.score,
                        roundScore = data.user2.roundScore,
                        handTiles = data.user2.handTiles.ToList().ToArray(),
                        saveTiles = data.user2.saveTiles.ToList().ToArray(),
                        throwTiles = data.user2.throwTiles.ToList().ToArray(),
                        experience = data.user2.experience,
                        level = data.user2.level,
                        title = data.user2.title,
                        time = data.user2.time,
                        blueGem = data.user2.blueGem,
                        redGem = data.user2.redGem,
                        rockGem = data.user2.rockGem,
                        yelloGem = data.user2.yelloGem,
                        gameEvent = data.user2.gameEvent,
                        clickedTile = data.user2.clickedTile,
                        eventHint = data.user2.eventHint,
                    },
                    user3 = new UserData
                    {
                        userId = data.user3.userId,
                        playerplace = data.user3.playerplace,
                        userName = data.user3.userName,
                        avatar = data.user3.avatar,
                        score = data.user3.score,
                        roundScore = data.user3.roundScore,
                        handTiles = data.user3.handTiles.ToList().ToArray(),
                        saveTiles = data.user3.saveTiles.ToList().ToArray(),
                        throwTiles = data.user3.throwTiles.ToList().ToArray(),
                        experience = data.user3.experience,
                        level = data.user3.level,
                        title = data.user3.title,
                        time = data.user3.time,
                        blueGem = data.user3.blueGem,
                        redGem = data.user3.redGem,
                        rockGem = data.user3.rockGem,
                        yelloGem = data.user3.yelloGem,
                        gameEvent = data.user3.gameEvent,
                        clickedTile = data.user3.clickedTile,
                        eventHint = data.user3.eventHint,
                    },
                    user4 = new UserData
                    {
                        userId = data.user4.userId,
                        playerplace = data.user4.playerplace,
                        userName = data.user4.userName,
                        avatar = data.user4.avatar,
                        score = data.user4.score,
                        roundScore = data.user4.roundScore,
                        handTiles = data.user4.handTiles.ToList().ToArray(),
                        saveTiles = data.user4.saveTiles.ToList().ToArray(),
                        throwTiles = data.user4.throwTiles.ToList().ToArray(),
                        experience = data.user4.experience,
                        level = data.user4.level,
                        title = data.user4.title,
                        time = data.user4.time,
                        blueGem = data.user4.blueGem,
                        redGem = data.user4.redGem,
                        rockGem = data.user4.rockGem,
                        yelloGem = data.user4.yelloGem,
                        gameEvent = data.user4.gameEvent,
                        clickedTile = data.user4.clickedTile,
                        eventHint = data.user4.eventHint,
                    },
                };
            }
        }

        public string SendJsonStringify(){
            string jsonData = JsonUtility.ToJson(sendGI);
            return jsonData;
        }
        public GameInfoData GetSendGameData()
        {
            return sendGI;
        }
        public GameInfoData GetRecvGameData()
        {
            return recvGI;
        }

    }

    [Serializable]
    public class RoomInfoData
    {
        public long roomId;
        public int order;
        public string[] remineTiles;
        public string[] doraTiles;
        public string drawTile;
        public int plusTime;
        public int basedTime;
        public int roomType;
        public int roomLevel;
        public int maxOfPlayer;
        public string roomCreater;
        public int roomDealer;
        public int[] roundWinner;
        public int[] roundFailer;
        public int roomRound;
        public string roomGameEvent;
        public int roomEventHint;
        public string[] yakuList;
        public string[] hiddenDora;
        public int fu;
        public int han;
    }

    [Serializable]
    public class UserData
    {
        public string userId;
        public int playerplace;
        public string userName;
        public string avatar;
        public int score;
        public int roundScore;
        public string[] handTiles;
        public string[] saveTiles;
        public string[] throwTiles;
        public int experience;
        public int level;
        public int title;
        public string time;
        public int blueGem;
        public int redGem;
        public int rockGem;
        public int yelloGem;
        public string gameEvent;
        public string clickedTile;
        public string[] eventHint;
        public int plusTime;

    }

    [Serializable]
    public class GameInfoData
    {
        public RoomInfoData roomInfo;
        public UserData user1;
        public UserData user2;
        public UserData user3;
        public UserData user4;
        public GameInfoData()
        {
            roomInfo = new RoomInfoData();
            user1 = new UserData();
            user2 = new UserData();
            user3 = new UserData();
            user4 = new UserData();
        }
    }

    public static class JsonHelper
    {
        [Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }

        public static T[] FromJsonArray<T>(string json)
        {
            string newJson = "{\"array\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }
    }
}
