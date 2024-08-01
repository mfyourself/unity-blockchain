using UnityEngine;
using UnityEngine.UI;
using UnityAuthStructure;

namespace UnityRoomController
{
    public class RoomEntry : MonoBehaviour
    {
        public Text roomNameText;
        // public RectTransform QTJStatus;
        public Text playerStatusText;
        public Button checkRuleButton;
        public Button joinButton;
        public string roomName;

        public void SetRoom(string roomId, string maxOfPlayer, int length)
        {
            ReceivedUserData receivedUserData = AuthStructure.Instance.GetUserData();
            roomNameText.text = roomId;
            playerStatusText.text = $"{length}/{maxOfPlayer}";
            joinButton.onClick.RemoveAllListeners();
            roomName = roomId;
            joinButton.onClick.AddListener(() =>
            {
                RoomController.JoinRoom(roomId, receivedUserData.userId, true);
            });
        }

        public void JoinRoom(){
            ReceivedUserData receivedUserData = AuthStructure.Instance.GetUserData();
            RoomController.JoinRoom(roomName, receivedUserData.userId, true);
        }
    }
}
