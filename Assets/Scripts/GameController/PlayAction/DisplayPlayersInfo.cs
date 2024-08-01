using System;
using System.Collections.Generic;
using MahJongController;
using System.Collections;
using UnityAuthStructure;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;


namespace MahJongController
{
    public class DisplayPlayersInfo : MonoBehaviour
    {
        public ReceivedUserData receivedUserData;
        public GameInfoData gameInfoData;

        public void DisplayUserInfo(int myPlayce)
        {
            gameInfoData = HandTiles.gameInfoData;
            GameObject Child = this.transform.GetChild(1).gameObject;
            GameObject PlayerNames = this.transform.GetChild(0).gameObject;
            int playerPlace = myPlayce;
            for (int i = 0; i < 4; i++)
            {
                GameObject PlayerName = PlayerNames.transform.GetChild(i).gameObject;
                GameObject Names = PlayerName.transform.GetChild(0).gameObject;
                GameObject Avatars = Child.transform.GetChild(i).gameObject;
                GameObject Titles = Avatars.transform.GetChild(1).gameObject;
                Text player_Names = Names.GetComponent<Text>();
                Image titles = Titles.GetComponent<Image>();
                Image avatars = Avatars.GetComponent<Image>();
                UserData userInfo = GetMyinfoData(gameInfoData, playerPlace);
                string str_avatar = string.Format("chara_icon_{0}", userInfo.avatar);
                string str_title = string.Format("Levels_{0}", userInfo.level);
                string str_name = userInfo.userName;
                titles.sprite = GetTitleImage(str_title);
                avatars.sprite = GetAvatarImage(str_avatar);
                player_Names.text = str_name;
                if (playerPlace == 3)
                {
                    playerPlace = 0;
                    continue;
                } else
                {
                    playerPlace = playerPlace + 1;
                }
            }
        }

        public static UserData GetMyinfoData(GameInfoData gameInfoData, int playerPlace)
        {
            UserData myUserInfo = new UserData();
            List<UserData> users = new List<UserData> { gameInfoData.user1, gameInfoData.user2, gameInfoData.user3, gameInfoData.user4 };
            foreach (var user in users)
            {
                if (user.playerplace == playerPlace)
                {
                    myUserInfo = user;
                    break;
                }
            }
            return myUserInfo;
        }

        public static Sprite GetAvatarImage(string image)
        {
            if (image != "")
            {
                Sprite[] sprites = Resources.LoadAll<Sprite>("UITextures/GameUI/ingame/chara_icon");
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

        public static Sprite GetTitleImage(string image)
        {
            if (image != "")
            {
                Sprite[] sprites = Resources.LoadAll<Sprite>("UITextures/GameUI/Room/Levels");
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