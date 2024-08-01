using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using UnitySocketManager;
namespace UnityRoomController
{
    public class RoomListPanel : MonoBehaviour
    {
        public RectTransform contentParent;
        public GameObject roomEntryPrefab;
        private const float height = 150f;
        private const float roomSpacing = 10f;
        SocketIOUnity socket;

        void Start(){
            socket = SocketManager.GetSocket();
        }

        private void Update()
        {
            StartCoroutine(DetectSocket());
        }

        private IEnumerator DetectSocket()
        {
            // SocketIOUnity socket = SocketManager.GetSocket();
            bool roomJoined = false;
            string roomListInfo = ""; // Initialize with null
            socket.Emit("ROOM_LIST", new  {});
            
            socket.On("ROOM_LIST", (response) =>
            {
                roomListInfo = response.ToString();
                roomJoined = true;
            });
            

            yield return new WaitUntil(() => roomJoined);
            string result = roomListInfo.Substring(1, roomListInfo.Length - 2);
            string resul1 = result.Substring(1, result.Length - 2);
            SetRoomList(resul1);
        }

        public void SetRoomList(string rooms)
        {
            ReceivedData[] dataArray = JsonHelper.FromJsonArray<ReceivedData>(rooms);
            int i = 0;
            foreach (ReceivedData data in dataArray)
            {
                float newContentHeight = dataArray.Length * (height + roomSpacing) - roomSpacing;
                contentParent.sizeDelta = new Vector2(contentParent.sizeDelta.x , newContentHeight);

                if (data.roomId != null)
                {
                        int length = 0;
                        if(data.user1 != "400000"){ length ++; }
                        if(data.user2 != "400000"){ length ++; }
                        if(data.user3 != "400000"){ length ++; }
                        if(data.user4 != "400000"){ length ++; }

                        RoomEntry entry;
                        if (i < contentParent.childCount)
                        {
                            var t = contentParent.GetChild(i);
                            t.gameObject.SetActive(true);
                            entry = t.GetComponent<RoomEntry>();
                        }
                        else
                        {
                            var obj = Instantiate(roomEntryPrefab, contentParent);
                            entry = obj.GetComponent<RoomEntry>();
                            RectTransform entryRect = obj.GetComponent<RectTransform>();
                            // entryRect.anchoredPosition = new Vector2(0, -i * (height + roomSpacing));
                            entryRect.anchoredPosition = new Vector2(0, 0); // Set anchoredPosition to (0, 0)
                        }
                    Debug.Log(data.roomId);
                    Debug.Log(data.maxOfPlayer);
                    Debug.Log(length);
                    entry.SetRoom(data.roomId, data.maxOfPlayer, length);
                        var rect = contentParent.GetChild(i).GetComponent<RectTransform>();
                        rect.localPosition = new Vector2(rect.sizeDelta.x/2 , -i * (height + roomSpacing));

                        i++;
                }
            }
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
    public class ReceivedData
    {
        public string roomId;
        public string maxOfPlayer;
        public string user1;
        public string user2;
        public string user3;
        public string user4;
        public string activaStatus;
    }
    [Serializable]
    public class Rooms
    {
        public string roomId;
        public string maxOfPlayer;
        public string user1;
        public string user2;
        public string user3;
        public string user4;
        public string activaStatus;
    }
}