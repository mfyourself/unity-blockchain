using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MahjongLobbyController
{
    [SerializeField]
    public class LobbyStructure : MonoBehaviour
    {
        private static LobbyStructure instance;
        public static LobbyStructure Instance
        {
            get { return instance; }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static ReceivedLobbyData receivedLobbyData;

        public void ReceivedJsonDataOfLobby(string receivedData)
        {
            ReceivedLobbyData[] dataArray = JsonHelper.FromJsonArray<ReceivedLobbyData>(receivedData);

            if (dataArray.Length > 0)
            {
                receivedLobbyData = dataArray[0];
            }
        }

        public void SetLobbyData(ReceivedLobbyData lobbyData)
        {
            receivedLobbyData = lobbyData;
        }

        public ReceivedLobbyData GetLobbyData()
        {
            return receivedLobbyData;
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
    public class ReceivedLobbyData
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
        public int maxOfPlayer;
        public int plusTime;
        public int basedTime;
        public int roomType;
        public int roomKinds;
        public int contestSettings;
        public int startPoint;
        public int requirePoint;
        public int redDora;
        public int binding;
        public int eater;
    }
}