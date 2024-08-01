using System.Collections;
using UnityEngine;
using UnityAuthStructure;
using UnityEngine.UI;

namespace MahjongLobbyController
{
    public class DisplayPlayerInfo : MonoBehaviour
    {

        // Use this for initialization
        public ReceivedUserData receiveUserData;

        // Update is called once per frame
        void Update()
        {
            DisplayPlyaerInformation();
        }

        private void DisplayPlyaerInformation()
        {
            receiveUserData = AuthStructure.Instance.GetUserData();
            string playerImage = string.Format("chara_icon_{0}", receiveUserData.userAvatar);
            string playerTitles = string.Format("Levels_{0}", receiveUserData.userTitle);
            GameObject ObjPlayerAvatar = this.transform.GetChild(1).gameObject;
            GameObject ObjPlayerName = this.transform.GetChild(3).gameObject;
            GameObject ObjPlayerTitle = this.transform.GetChild(4).gameObject;
            Image playerAvatar = ObjPlayerAvatar.GetComponent<Image>();
            Image playerTitle = ObjPlayerTitle.GetComponent<Image>();
            Text playerName = ObjPlayerName.GetComponent<Text>();

            playerName.text = receiveUserData.userName;
            playerAvatar.sprite = GetAvatarImage(playerImage, "UITextures/GameUI/ingame/chara_icon");
            playerTitle.sprite = GetAvatarImage(playerTitles, "UITextures/GameUI/Room/Levels");


        }

        public static Sprite GetAvatarImage(string image, string path)
        {
            if (image != "")
            {
                Sprite[] sprites = Resources.LoadAll<Sprite>(path);
                Sprite titleSprite = null;
                foreach (Sprite sprite in sprites)
                {
                    if (sprite.name == image)
                    {
                        titleSprite = sprite;
                        break;
                    }
                }
                return titleSprite;
            }
            return null;
        }
    }
}