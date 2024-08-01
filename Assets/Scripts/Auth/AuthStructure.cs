using System;
using UnityEngine;

namespace UnityAuthStructure
{
    [Serializable]
    public class AuthStructure : MonoBehaviour
    {
        private static AuthStructure instance;

        public static AuthStructure Instance
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

        public static ReceivedUserData receiveUserData;

        public void ReceivedJsonDataOfAuth(string receivedData)
        {
            ReceivedUserData[] dataArray = JsonHelper.FromJsonArray<ReceivedUserData>(receivedData);
            if (dataArray.Length > 0)
            {
                receiveUserData = dataArray[0]; 
            }
            else
            {
                Debug.LogError("No user data found in the received JSON.");
            }
        }

        // Method to set user data
        public void SetUserData(ReceivedUserData userData)
        {
            receiveUserData = userData;
        }

        // Method to get user data
        public ReceivedUserData GetUserData()
        {

            return receiveUserData;
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

        [Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }

    [Serializable]
    public class ReceivedUserData
    {
        public string userId;
        public string userEmail;
        public string userName;
        public int userLevel;
        public int userExperience;
        public string userTitle;
        public string userAvatar;
        public int blue_gem;
        public int rock_gem;
        public int red_gem;
        public int yellow_gem;
    }
}
