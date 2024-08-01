using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MahJongController
{
    public class ScoreTransfer : MonoBehaviour
    {
        bool isCompletedDisplayDuration;
        public void ShowPoints(GameInfoData gameInfoData, int myPlace)
        {
            StartCoroutine(ShowPointTransfer(gameInfoData, myPlace));
        }
        private int myPlace;
        public IEnumerator ShowPointTransfer(GameInfoData gameInfoData, int myPlace)
        {
            ClearArrows();
            float displayDuration = 2f;
            this.myPlace = myPlace;
            isCompletedDisplayDuration = false;
            UserData myUserInfo = new UserData();
            List<UserData> users = new List<UserData> { gameInfoData.user1, gameInfoData.user2, gameInfoData.user3, gameInfoData.user4 };
            GameObject Transfer = this.transform.GetChild(0).gameObject;
            Transfer.SetActive(true);
            GameObject Content = Transfer.transform.GetChild(1).gameObject;
            for (int i = 0; i < 4; i++)
            {
                UserData userData = users[i];
                string score = Convert.ToString((userData.score < 0) ? -userData.score : userData.score);
                string roundScore = Convert.ToString((userData.roundScore < 0) ? -userData.roundScore : userData.roundScore);
                int playerPlace = SetPlaceTile(myPlace, userData.playerplace);
                var players = Content.transform.GetChild(playerPlace).gameObject;
                players.transform.Find("PointPanel").GetComponent<PointSettings>().SetPoint(score);
                players.transform.Find("RoundPanel").GetComponent<PointSettings>().SetGoldPoint(roundScore);
                players.transform.Find("PlayerName/Name").GetComponent<Text>().text = userData.userName;
                string avatar = string.Format("chara_b_{0}", userData.avatar);
                GameObject obj = players.transform.GetChild(0).gameObject;
                obj.GetComponent<Image>().sprite = GetAvatarImage(avatar, "UITextures/GameUI/ingame/chara_b");
            }

            if (gameInfoData.roomInfo.roundWinner.Length > 0)
            {
                Debug.Log(gameInfoData.roomInfo.roundFailer.Length + ",---," + gameInfoData.roomInfo.roundWinner.Length);
                for (int i = 0; i < gameInfoData.roomInfo.roundWinner.Length; i++)
                {
                    ShowArrows(SetPlaceTile(myPlace, gameInfoData.roomInfo.roundWinner[i]), gameInfoData.roomInfo.roundFailer);
                }
            }
            else
            {
                ClearArrows();
            }

            yield return new WaitForSeconds(displayDuration);
            Transfer.SetActive(false);
            isCompletedDisplayDuration = true;
        }

        public static Sprite GetAvatarImage(string image, string source)
        {
            if (image != "")
            {
                Sprite[] sprites = Resources.LoadAll<Sprite>(source);
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

        public static int SetPlaceTile(int myPlace, int order)
        {
            int playerPlace = 0;
            if (order > myPlace)
            {
                playerPlace = order - myPlace;
            }
            else if (order < myPlace)
            {
                playerPlace = myPlace - order;
                playerPlace = 4 - playerPlace;
            }

            return playerPlace;
        }

        public void ExitGame()
        {
            Debug.Log("Exiting...");
            SceneManager.LoadScene("MahJong_Lobby");
        }

        public void ShowArrows(int winner, int[] losers)
        {
            for (int i = 0; i < losers.Length; i++)
            {
                int e = losers[i];
                e = SetPlaceTile(myPlace, e);
                transform.Find($"Transfer/Content/Player{e}/Arrow/To{winner}").gameObject.SetActive(true);
            }
        }

        public void ClearArrows()
        {
            for (int i = 0; i < 4; i++)
            {
                foreach (Transform child in transform.Find($"Transfer/Content/Player{i}/Arrow"))
                {
                    child.gameObject.SetActive(false);
                }
            }
        }

        public bool IsCompletedDisplayPointDuration()
        {
            return isCompletedDisplayDuration;
        }
    }

}