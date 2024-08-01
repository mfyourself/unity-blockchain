using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;


namespace MahJongController
{
    public class HandTiles : MonoBehaviour
    {
        public static bool isClicked = false;
        public static GameInfoData gameInfoData, lastGameInfo;
        public static int myPlace;
        public static UserData userInfo;
        public static string[] availableTiles;
        public float positionY = 0;

        public void Start()
        {
            positionY = transform.GetChild(0).position.y;
        }
        public bool Contains<T>(T[] array, T value)
        {
            return Array.IndexOf(array, value) != -1;
        }

        public void ShowHandTiles(string[] tiles, string LastDrawTile)
        {
            ClearDisableTiles();
            tiles = HandTile.SortArray(tiles);  // sorted!
            for (int i = 0; i < 17; i++)
            {
                GameObject child = this.transform.GetChild(i).gameObject;
                Image image = child.GetComponent<Image>();
                if (image != null && i < tiles.Length)
                {
                    child.gameObject.SetActive(true);
                    image.sprite = GetTileImage(tiles[i]);
                }
                else child.gameObject.SetActive(false);

                if (child.gameObject.name == "DrawTile")
                {
                    if (LastDrawTile == "" || LastDrawTile == null)
                        child.gameObject.SetActive(false);
                    else
                    {
                        Vector3 newPosition = this.transform.GetChild(tiles.Length - 1).position;
                        newPosition.x += 70;
                        child.transform.position = newPosition;

                        Image imgLast = child.transform.GetComponent<Image>();
                        if (GetTileImage(LastDrawTile) != null)
                        {
                            imgLast.sprite = GetTileImage(LastDrawTile);
                            child.gameObject.SetActive(true);
                        }
                    }
                }
            }
            if (userInfo.gameEvent == "5")
            {
                CheckRiichiAbleTiles(userInfo);
            }
        }

        public void CheckRiichiAbleTiles(UserData userinfo)
        {
            if (GameController.secondTimer != null)
                StopCoroutine(GameController.secondTimer);
            var handTiles1 = userinfo.handTiles.ToList<string>();
            var drawTile1 = gameInfoData.roomInfo.drawTile;

            var riichiMahjong = new RiichiMahjong();
            //bool canDeclareRiichi = riichiMahjong.CanDeclareRiichi(handTiles1, drawTile1, out List<string> usedTiles);
            List<int> tiles = new List<int>();
            if (drawTile1 == Constant.GAME_SETTINGS_DEFAULT_EMPTY_VALUE)
            {
                tiles = riichiMahjong.ConvertTilesToNumbers(handTiles1.ToList());
            }
            else
            {
                tiles = riichiMahjong.ConvertTilesToNumbers(handTiles1.Append(drawTile1).ToList());
            }
            var unUsedTiles = riichiMahjong.getUnUsedTiles(tiles);
            var result = riichiMahjong.ConvertNumbersToTiles(unUsedTiles);
            string[] res = result.ToArray<string>();
            availableTiles = res;
            Debug.Log("------------------------------------------ : " + string.Join("-", res));
            if (userInfo.gameEvent == Constant.GAME_EVENTS_RIICHI)
            // if (HandTiles.userInfo.gameEvent == Constant.GAME_EVENTS_RIICHI && FindObjectOfType<HandTiles>().Contains(HandTiles.userInfo.eventHint, Constant.GAME_EVENTS_RIICHI))
            {
                //if (gameInfoData.roomInfo.order == myPlace)
                if (gameInfoData.roomInfo.order == myPlace && gameInfoData.roomInfo.roomGameEvent == "5")
                {
                    Debug.Log("RiichDetectedtiles : " + string.Join("-", res));
                    DisableTileInteractive(res);
                }
                else
                {
                    if (gameInfoData.roomInfo.order == myPlace)
                    {
                        ClearDisableTiles();
                        DisableHandTiles();
                    }
                    else
                    {
                        ClearDisableTiles();
                    }
                }
            }
        }

        public static string GetClickedTile(GameInfoData gameInfoData, int order)
        {
            string clickedTile = "";
            List<UserData> users = new List<UserData> { gameInfoData.user1, gameInfoData.user2, gameInfoData.user3, gameInfoData.user4 };

            foreach (var user in users)
            {
                if (user.playerplace == order)
                {
                    clickedTile = user.clickedTile;
                    break;
                }
            }

            return clickedTile;
        }

        // If Riichi Tiles are in my hand, Disable handtiles except arrHints.
        public void DisableTileInteractive(string[] arrHints)
        {
            string[] hands = userInfo.handTiles;  // Hand Tiles

            for (int i = 0; i < hands.Length; i++)
            {
                if (Contains(arrHints, transform.GetChild(i).GetComponent<Image>().sprite.name))
                {
                    transform.GetChild(i).Find("Panel").gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log(transform.GetChild(i).GetComponent<Image>().sprite.name + ":DisableTileInteractiveokkkkkk:" + hands[i]);
                    transform.GetChild(i).Find("Panel").gameObject.SetActive(true);
                }
            }
            transform.Find("DrawTile").GetChild(0).gameObject.SetActive(true);
        }

        public void DisableHandTiles()
        {

            for (int i = 0; i < userInfo.handTiles.Length; i++)
            {
                transform.GetChild(i).Find("Panel").gameObject.SetActive(true);
            }
            transform.Find("DrawTile").GetChild(0).gameObject.SetActive(false);
        }

        //public string[] GetForbiddenTiles(string[] handTiles, string[] res)
        //{
        //    int i, j;
        //    List<string> tiles = new List<string>();
        //    for (i = 0; i < res.Length; i++)
        //    {
        //        for (j = 0; j < handTiles.Length; j++)
        //        {
        //            if (res[i] == handTiles[j])
        //                handTiles[j] = "0";
        //        }
        //    }
        //    foreach (string tile in handTiles)
        //    {
        //        if (tile != "0")
        //            tiles.Add(tile);
        //    }
        //    return tiles.ToArray();
        //}

        public void ThrowChildTile(string tileName, string Order)
        {
            int orders = Convert.ToInt32(Order);
            GameObject child = this.transform.GetChild(0).gameObject;
            HandTile tile = child.GetComponent<HandTile>();
            tile.ThrowTile(tileName, orders);
        }

        public static Sprite GetTileImage(string image)
        {
            if (image != "")
            {
                Sprite[] sprites = Resources.LoadAll<Sprite>("UITextures/GameUI/Room/ui");
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

        public void PonShow()
        {
            GameObject ChiPanel = transform.Find("PonPanel").gameObject;
            ChiPanel.SetActive(true);
            isClicked = true;
        }

        public void KanShow()
        {
            GameObject ChiPanel = transform.Find("KanPanel").gameObject;
            ChiPanel.SetActive(true);
            isClicked = true;
        }

        public void ChiShow()
        {
            GameObject ChiPanel = transform.Find("ChiPanel").gameObject;
            ChiPanel.SetActive(true);
            isClicked = true;
        }

        public void RiichShow()
        {
            GameObject RiichiPanel = transform.Find("RiichiPanel").gameObject;
            RiichiPanel.SetActive(true);
            isClicked = true;
        }

        public void TusmoShow()
        {
            GameObject TsumoPanel = transform.Find("TsumoPanel").gameObject;
            TsumoPanel.SetActive(true);
            isClicked = true;
        }

        public void RonShow()
        {
            GameObject RonPanel = transform.Find("RonPanel").gameObject;
            RonPanel.SetActive(true);
            isClicked = true;
        }

        public void Richii()
        {
            FindFirstObjectByType<HandTile>().CancelTimer();
            transform.Find("RiichiPanel").gameObject.SetActive(false);
            gameInfoData.roomInfo.roomGameEvent = Constant.GAME_EVENTS_RIICHI;

            HandTile.SetPlyaerInfo(gameInfoData, myPlace);
            FindObjectOfType<StatusBoard>().AddRiichiCount();         // Add Riich Count to TableBoard
            FindObjectOfType<TableBoard>().SetRichiiStatus(0);         //Table Border Canvas
            SendPlayingInfo.SendController(Convert.ToInt32(Constant.GAME_EVENTS_RIICHI), userInfo.handTiles, userInfo.saveTiles, gameInfoData.roomInfo.drawTile);

            isClicked = false;
        }

        public void Tusmo()
        {
            FindFirstObjectByType<HandTile>().CancelTimer();
            transform.Find("TsumoPanel").gameObject.SetActive(false);

            gameInfoData.roomInfo.roomGameEvent = "6";
            string drawTile = gameInfoData.roomInfo.drawTile;
            string[] handTiles = GetPlayerInfo(gameInfoData, myPlace).handTiles;
            string[] saveTiles = GetPlayerInfo(gameInfoData, myPlace).saveTiles;
            HandTile.SetPlyaerInfo(gameInfoData, myPlace);
            SendPlayingInfo.SendController(6, handTiles, saveTiles, drawTile);

            isClicked = false;
        }

        public void Ron()
        {
            FindFirstObjectByType<HandTile>().CancelTimer();
            transform.Find("RonPanel").gameObject.SetActive(false);
            gameInfoData.roomInfo.roomGameEvent = "7";
            string[] handTiles = GetPlayerInfo(gameInfoData, myPlace).handTiles;
            string[] saveTiles = GetPlayerInfo(gameInfoData, myPlace).saveTiles;
            int order = gameInfoData.roomInfo.order;

            int lastClickedOrder = (order == 0) ? 3 : order - 1;
            string tableTile = GetOtherPlayersCLickedTile(gameInfoData, lastClickedOrder);
            HandTile.SetPlyaerInfo(gameInfoData, myPlace);
            isClicked = false;
            SendPlayingInfo.SendController(7, handTiles, saveTiles, tableTile);
        }

        public void Pon()
        {
            FindFirstObjectByType<HandTile>().CancelTimer();

            transform.Find("PonPanel").gameObject.SetActive(false);
            isClicked = false;

            string[] handTiles = GetPlayerInfo(gameInfoData, myPlace).handTiles;
            int order = gameInfoData.roomInfo.order;

            int lastUserPlace = (order == 0) ? 3 : order - 1;
            string tableTile = GetOtherPlayersCLickedTile(gameInfoData, lastUserPlace);

            // Remove TableTile
            int LastUserPlayerPlace = GetPlayerInfo(gameInfoData, lastUserPlace).playerplace;
            int lastUserTablePlace = (LastUserPlayerPlace >= myPlace) ? LastUserPlayerPlace - myPlace : 4 + LastUserPlayerPlace - myPlace;
            ShowTableTile(lastUserTablePlace, false);

            // Put Meld TIles
            if (tableTile == "0m") { tableTile = "5m"; } else if (tableTile == "0s") { tableTile = "5s"; } else if (tableTile == "0p") { tableTile = "5p"; }

            string[] ponTiles = new string[] { tableTile, tableTile, tableTile };
            string[] handRemoveTiles = new string[] { tableTile, tableTile };
            string[] newHandTiles = HandTile.RemoveElementsFromArray(handTiles, handRemoveTiles);
            string[] newSaveTiles = HandTile.MergeArrays(GetPlayerInfo(gameInfoData, myPlace).saveTiles, ponTiles);
            string shape = GetPonShape(gameInfoData, myPlace);

            Debug.Log("PonTILES:" + string.Join(".", ponTiles) + ", HandTileLeng:" + handTiles.Length + ", PONNN------last clickedTile :"
               + tableTile + ", " + ": resHandTilsRest" + HandTile.RemoveElementsFromArray(handTiles, handRemoveTiles).Length);


            MeldSet.SetTiles(ponTiles, 0, shape);
            SendPlayingInfo.SendController(3, newHandTiles, newSaveTiles, tableTile);
        }

        public static string GetPonShape(GameInfoData gameInfoData, int myPlace)
        {
            string savetileShape = "MeldThreeL";
            switch (gameInfoData.roomInfo.order - myPlace)
            {
                case 1:
                case -3:
                    savetileShape = "MeldThreeR"; break;
                case 2:
                case -2:
                    savetileShape = "MeldThreeCenter";
                    break;
                case 3:
                case -1:
                    savetileShape = "MeldThreeL"; break;
                default:
                    break;
            }
            return savetileShape;
        }

        public void Chi()
        {
            isClicked = false;
            transform.Find("ChiPanel").gameObject.SetActive(false);
            FindFirstObjectByType<HandTile>().CancelTimer();
            ChiiAction();
        }

        public void ChiiAction()
        {
            string[] handTiles = GetPlayerInfo(gameInfoData, myPlace).handTiles;

            int order = gameInfoData.roomInfo.order;
            int lastUserPlace = (order == 0) ? 3 : order - 1;

            //Table Tile
            string lastClickedTile = GetPlayerInfo(gameInfoData, lastUserPlace).clickedTile;

            // Put Meld TIles
            string[] chiiTIles = SelectChiTiles(handTiles, lastClickedTile);
            if (chiiTIles.Length > 3)
            {
                chiiTIles = new string[] { chiiTIles[0], chiiTIles[1], chiiTIles[2] };
            }
            else
            {
                //transform.Find("HintPanel").gameObject.SetActive(true);
            }
            string[] handRemoveTiles = HandTile.RemoveElementsFromArray(chiiTIles, new string[] { lastClickedTile });
            string[] newHandTiles = HandTile.RemoveElementsFromArray(handTiles, handRemoveTiles);
            ShowHandTiles(newHandTiles, null);
            string[] newSaveTiles = HandTile.MergeArrays(GetPlayerInfo(gameInfoData, myPlace).saveTiles, chiiTIles);
            Debug.Log("CHILTILES:" + string.Join(".", chiiTIles) + ", HandTileLeng:" + handTiles.Length + ", CHIIIIII------last clickedTile :"
                + lastClickedTile + ", " + ": resHandTilsRest" + HandTile.RemoveElementsFromArray(handTiles, handRemoveTiles).Length);

            // Remove Key Tile
            int LastUserPlayerPlace = GetPlayerInfo(gameInfoData, lastUserPlace).playerplace;
            int lastUserTablePlace = (LastUserPlayerPlace > myPlace) ? LastUserPlayerPlace - myPlace : 4 + LastUserPlayerPlace - myPlace;
            ShowTableTile(lastUserTablePlace, false);

            // Put SaveTiles
            MeldSet.SetTiles(chiiTIles, 0, "MeldThreeL");
            SendPlayingInfo.SendController(2, newHandTiles, newSaveTiles, lastClickedTile);
        }

        //public void Kan()
        //{
        //    FindFirstObjectByType<HandTile>().CancelTimer();
        //    transform.Find("KanPanel").gameObject.SetActive(false);

        //    UserData userInfo = GetPlayerInfo(gameInfoData, myPlace);
        //    string[] handTiles = userInfo.handTiles;
        //    string[] myTiles = HandTile.MergeArrays(handTiles, userInfo.saveTiles);
        //    int order = gameInfoData.roomInfo.order;
        //    int lastUserPlace = (order == 0) ? 3 : order - 1;
        //    int LastUserPlayerPlace = GetPlayerInfo(gameInfoData, lastUserPlace).playerplace;
        //    int lastUserTablePlace = (LastUserPlayerPlace >= myPlace) ? LastUserPlayerPlace - myPlace : 4 + LastUserPlayerPlace - myPlace;
        //    string drTile = gameInfoData.roomInfo.drawTile;
        //    handTiles = (drTile != "") ? HandTile.MergeArrays(handTiles, new string[] { drTile }) : handTiles;

        //    // Remove Key Tile
        //    if (order != myPlace)
        //        ShowTableTile(lastUserTablePlace, false); // Remove key tile


        //    //Last Key TileName
        //    string tableTile = (gameInfoData.roomInfo.drawTile == "") ? GetOtherPlayersCLickedTile(gameInfoData, lastUserPlace) : gameInfoData.roomInfo.drawTile;
        //    if (tableTile == "0m") { tableTile = "5m"; } else if (tableTile == "0s") { tableTile = "5s"; } else if (tableTile == "0p") { tableTile = "5p"; }


        //    //  string tableTile = gameInfoData.roomInfo.drawTile;
        //    string[] removeTiles = new string[] { tableTile, tableTile, tableTile, tableTile };
        //    string[] handRemoveTiles = (gameInfoData.roomInfo.drawTile != "") ? new string[] { tableTile, tableTile, tableTile, tableTile } : new string[] { tableTile, tableTile, tableTile };
        //    string[] newHandTIles = HandTile.RemoveElementsFromArray(handTiles, handRemoveTiles);

        //    ShowHandTiles(newHandTIles, null);
        //    string[] newSaveTiles = HandTile.MergeArrays(userInfo.saveTiles, removeTiles);
        //    Debug.Log("KANLTILES:" + string.Join(".", removeTiles) + ",Left: " + newSaveTiles.Length + ", KANNNN------NEW_SAVE_TILES_LENG :" + newSaveTiles.Length);

        //    string shape = GetKanShape(lastUserTablePlace, tableTile, newSaveTiles);
        //    MeldSet.SetTiles(removeTiles, 0, shape);
        //    SendPlayingInfo.SendController(4, newHandTIles, newSaveTiles, tableTile);
        //    isClicked = false;
        //}
        public void Kan()
        {
            FindFirstObjectByType<HandTile>().CancelTimer();
            transform.Find("KanPanel").gameObject.SetActive(false);

            UserData userInfo = GetPlayerInfo(gameInfoData, myPlace);
            string[] handTiles = userInfo.handTiles;
            string[] myTiles = HandTile.MergeArrays(handTiles, userInfo.saveTiles);

            int order = gameInfoData.roomInfo.order;
            int lastUserPlace = (order == 0) ? 3 : order - 1;
            int lastUserPlayerPlace = GetPlayerInfo(gameInfoData, lastUserPlace).playerplace;

            string drTile = gameInfoData.roomInfo.drawTile;
            string[] tilesForKanCheck = handTiles;

            if (drTile != "")
                tilesForKanCheck = HandTile.MergeArrays(handTiles, new[] { drTile });
            else
            {
                string clickedTile = GetClickedTile(gameInfoData, lastUserPlace);
                tilesForKanCheck = HandTile.MergeArrays(handTiles, new string[] { clickedTile });
            }
            
            string[] tableTiles = CheckKanElements(tilesForKanCheck);
            
            // Show Kan Selection Modal
            if (drTile != "")
            {
                int lastUserTablePlace = (lastUserPlayerPlace >= myPlace) ? lastUserPlayerPlace - myPlace : 4 + lastUserPlayerPlace - myPlace;
                ShowTableTile(lastUserTablePlace, false); // Remove key tile  
            }

            // keyTile
            string keyTile = tableTiles[0];
            string[] removeTiles = new[] { keyTile, keyTile, keyTile, keyTile };
            string[] newHandTIles = HandTile.RemoveElementsFromArray(handTiles, removeTiles);
            ShowHandTiles(newHandTIles, null);

            string[] newSaveTiles = HandTile.MergeArrays(userInfo.saveTiles, removeTiles);
            Debug.Log($"KANLTILES: {string.Join(".", removeTiles)}, Left: {newSaveTiles.Length}, KANNNN------NEW_SAVE_TILES_LENG: {newSaveTiles.Length}");

            string shape = GetKanShape(lastUserPlace, keyTile, newSaveTiles);
            MeldSet.SetTiles(removeTiles, 0, shape);
            SendPlayingInfo.SendController(4, newHandTIles, newSaveTiles, keyTile);
            isClicked = false;
        }

        public string GetKanShape(int lastUserTablePlace, string keyTile, string[] newSaveTiles)
        {
            string shape = null;
            Debug.Log(lastUserTablePlace + ", keyTile=" + keyTile);
            if (gameInfoData.roomInfo.drawTile != "")
                shape = "KanQueue";
            else
            {
                switch (lastUserTablePlace)
                {
                    case 1:
                        shape = Contains(newSaveTiles, keyTile) ? "KaKanRight" : "MinKanRight"; break;
                    case 2:
                        shape = Contains(newSaveTiles, keyTile) ? "KaKanCenter" : "MinKanCenter"; break;
                    case 3:
                        shape = Contains(newSaveTiles, keyTile) ? "KaKanLeft" : "MinKanLeft"; break;
                }
            }

            return shape;
        }

        public void CancelCPK()
        {
            DisableAllNotification();
            SendPlayingInfo.CancelCPK();
            isClicked = false;
        }

        public bool isCPK()
        {
            return (Contains(userInfo.eventHint, Constant.GAME_EVENTS_CHI) || Contains(userInfo.eventHint, Constant.GAME_EVENTS_KAN) || Contains(userInfo.eventHint, Constant.GAME_EVENTS_PON));
        }

        public void DisableAllNotification()
        {
            transform.Find("PonPanel").gameObject.SetActive(false);
            transform.Find("ChiPanel").gameObject.SetActive(false);
            transform.Find("KanPanel").gameObject.SetActive(false);
            transform.Find("RiichiPanel").gameObject.SetActive(false);
            transform.Find("RonPanel").gameObject.SetActive(false);
            transform.Find("TsumoPanel").gameObject.SetActive(false);
        }

        public void ClosePanels()
        {

        }

        public bool HasTriSameStrings(string[] strings, string target)
        {
            int count = 0;
            foreach (string str in strings)
            {
                if (String.Equals(str, target, StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                }
            }
            return count >= 3;
        }

        public void CancelRichii()
        {
            transform.Find("RiichiPanel").gameObject.SetActive(false);
            isClicked = false;
        }

        private string[] SelectChiTiles(string[] tiles, string t)
        {
            string[] tmpTiles = new string[tiles.Length];
            string temp = t;
            if (t[0] == '0')
                temp = ((char)(t[0] + 5)).ToString() + t[1].ToString();
            bool[] flagChangedTiles = new bool[tiles.Length];
            string prevTile = ((char)(temp[0] - 1)).ToString() + temp[1].ToString();
            string pPrevTile = ((char)(temp[0] - 2)).ToString() + temp[1].ToString();
            string nextTile = ((char)(temp[0] + 1)).ToString() + temp[1].ToString();
            string nNextTile = ((char)(temp[0] + 2)).ToString() + temp[1].ToString();

            List<string> res = new List<string>();
            if (t[1] != 'z')
            {
                for (int i = 0; i < tiles.Length; i++)
                {
                    string tmp = tiles[i];
                    flagChangedTiles[i] = false;
                    if (tmp[0] == '0')
                    {
                        flagChangedTiles[i] = true;
                        tmp = "5" + (char)tiles[i][1];
                    }
                    tmpTiles[i] = tmp;
                }

                if (Contains(tmpTiles, prevTile) && Contains(tmpTiles, pPrevTile))
                {
                    string pPrevTileTmp = pPrevTile;
                    string prevTileTmp = prevTile;

                    for (int i = 0; i < flagChangedTiles.Length; i++)
                    {
                        if (flagChangedTiles[i])
                        {
                            if (tmpTiles[i] == prevTile)
                                prevTileTmp = tiles[i];
                            if (tmpTiles[i] == pPrevTile)
                                pPrevTileTmp = tiles[i];
                        }
                    }
                    res.Add(t);
                    res.Add(pPrevTileTmp);
                    res.Add(prevTileTmp);
                }

                if (Contains(tmpTiles, prevTile) && Contains(tmpTiles, nextTile))
                {
                    string prevTileTmp = prevTile;
                    string nextTileTmp = nextTile;

                    for (int i = 0; i < flagChangedTiles.Length; i++)
                    {
                        if (flagChangedTiles[i])
                        {
                            Debug.Log(106);
                            if (tmpTiles[i] == prevTile)
                                prevTileTmp = tiles[i];

                            if (tmpTiles[i] == nextTile)
                                nextTileTmp = tiles[i];
                        }
                    }
                    res.Add(t);
                    res.Add(prevTileTmp);
                    res.Add(nextTileTmp);
                }

                if (Contains(tmpTiles, nNextTile) && Contains(tmpTiles, nextTile))
                {
                    string nextTileTmp = nextTile;
                    string nNextTileTmp = nNextTile;

                    for (int i = 0; i < flagChangedTiles.Length; i++)
                    {
                        if (flagChangedTiles[i])
                        {
                            if (tmpTiles[i] == nextTileTmp)
                                nextTileTmp = tiles[i];

                            if (tmpTiles[i] == nNextTileTmp)
                                nNextTileTmp = tiles[i];
                        }
                    }
                    res.Add(t);
                    res.Add(nextTileTmp);
                    res.Add(nNextTileTmp);
                }

            }
            return res.ToArray();
        }

        public string GetOtherPlayersCLickedTile(GameInfoData gameInfoData, int playerPlace)
        {
            List<UserData> users = new List<UserData> { gameInfoData.user1, gameInfoData.user2, gameInfoData.user3, gameInfoData.user4 };

            //  UserData userData = new UserData();
            foreach (UserData user in users)
            {
                if (user.playerplace == playerPlace)
                {
                    //userData = user;
                    return user.clickedTile;
                }
            }
            return null;
        }

        private int CountString(string[] array, string target)
        {
            int count = 0;
            foreach (string s in array)
            {
                if (s == target)
                {
                    count++;
                }
            }
            return count;
        }

        public static UserData GetPlayerInfo(GameInfoData gameInfoData, int playerPlace)
        {
            List<UserData> users = new List<UserData> { gameInfoData.user1, gameInfoData.user2, gameInfoData.user3, gameInfoData.user4 };
            UserData res = null;
            foreach (var user in users)
            {
                if (user.playerplace == playerPlace)
                {
                    res = user;
                    break;
                }
            }
            return res;
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

        public void ShowTableTile(int playerPlace, bool bShow)
        {
            GameObject IndexPlayer = transform.root.GetChild(playerPlace).gameObject;
            GameObject playerGround = IndexPlayer.transform.GetChild(0).gameObject; //Ground of This Player

            int activeChildCount = FindObjectOfType<HandTile>().GetActiveChildCount(playerGround);
            if (activeChildCount > 0)
            {
                playerGround.transform.GetChild(activeChildCount - 1).gameObject.SetActive(bShow);
                Debug.Log(IndexPlayer.name + "(--)" + playerGround.transform.GetChild(activeChildCount - 1).gameObject.name);
            }
            else
                Debug.LogError("Empty active tiles of his table yard");
        }

        public void RemoveTiles3D(int tablePlace, int leng)
        {
            var tiles = transform.root.GetChild(tablePlace).transform.Find("LowHand");

            for (int i = 12; i >= leng - 1; i--)
            {
                tiles.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < leng; i++)
            {
                tiles.GetChild(i).gameObject.SetActive(true);
            }

        }

        public void DisableTileInteractiveAll()
        {
            string[] hands = userInfo.handTiles;
            for (int j = 0; j < hands.Length; j++)
            {
                transform.GetChild(j).GetChild(0).gameObject.SetActive(true);
            }

        }

        public void ClearDisableTiles()
        {
            for (int i = 0; i < 13; i++)
            {
                transform.Find($"tile{i}/Panel").gameObject.SetActive(false);
                Vector3 currentPosition = transform.Find($"tile{i}").transform.position;
                currentPosition.y = positionY;
                transform.Find($"tile{i}").transform.position = currentPosition;
            }
            transform.Find("DrawTile/Panel").gameObject.SetActive(false);
        }

        public static string[] CheckKanElements(string[] array)
        {
            string[] modifiedArray = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == "0m")
                    modifiedArray[i] = "5m";
                else if (array[i] == "0p")
                    modifiedArray[i] = "5p";
                else if (array[i] == "0s")
                    modifiedArray[i] = "5s";
                else
                    modifiedArray[i] = array[i];
            }

            var groups = modifiedArray.GroupBy(s => s).ToArray();
            return groups.Any(g => g.Count() >= 4) ? groups.Where(g => g.Count() >= 4).Select(g => g.Key).ToArray() : null;
        }

    }
}