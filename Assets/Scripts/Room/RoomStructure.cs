using System;
using UnityEngine;

namespace MahjongRoomStructure
{
    [Serializable]
    public class RoomStructure : MonoBehaviour 
    {
        private static RoomStructure instance;

        public static RoomStructure Instance
        {
            get { return instance; }
        }    

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static ReceivedRoomData receivedRoomData;

        public void ReceivedJsonDataOfRoom(string receivedData){

            ReceivedRoomData[] dataArray = JsonHelper.FromJsonArray<ReceivedRoomData>(receivedData);

            if (dataArray.Length > 0)
            {
                receivedRoomData = dataArray[0];
            }
        }

        public void SetRoomData(ReceivedRoomData roomData)
        {
            receivedRoomData = roomData;
        }

        public ReceivedRoomData GetRoomData()
        {
            return receivedRoomData;
        }

    }

    public static class JsonHelper
    {
        public static T[] FromJsonArray<T>(string json)
        {
            string newJson = "{\"array\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }

    [Serializable]
    public class ReceivedRoomData
    {
        public Users[] userinfo;
        public Rooms roomInfo;
    }

    [Serializable]
    public class Users
    {
        public string userId; 
        public string userName;
        public string userLevel;
        public string userAvatar;
        public string userTitle;
        public string socketId;
        public string blue_gem;
        public string yellow_gem;
        public string red_gem;
        public string userActivate;

    }

    [Serializable]
    public class Rooms
    {
        public string roomId;
        public string maxOfPlayer;
        public string roomType;
        public string roomLevel;
        public string user1;
        public string user2;
        public string user3;
        public string user4;
        public string activaStatus;
    }

}