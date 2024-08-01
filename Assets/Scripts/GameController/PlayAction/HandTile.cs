using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using UnityAuthStructure;
using System.Linq.Expressions;
using UnityEngine.Rendering.Universal;
using CommonUtils;


namespace MahJongController
{
    public class HandTile : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public ReceivedUserData retrievedData;
        public GameInfoData gameInfoData;
        private int flagDrawTilePositonPlus = -1;

        public bool IsDisable()
        {
            return transform.GetChild(0).gameObject.activeSelf;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsDisable())
                return;
            gameInfoData = HandTiles.gameInfoData;
            string[] handTiles = HandTiles.userInfo.handTiles;
            string[] saveTiles = HandTiles.userInfo.saveTiles;
            int myPlace = HandTiles.myPlace;
            Debug.Log("*********************" + HandTiles.isClicked);
            if (!HandTiles.isClicked && myPlace == gameInfoData.roomInfo.order)
            {
                Image imageComponent = this.GetComponent<Image>();
                string spriteName = imageComponent.sprite.name;
                if ((gameInfoData.roomInfo.roomGameEvent == Constant.GAME_EVENTS_RIICHI) && (gameInfoData.roomInfo.roomEventHint == Convert.ToInt32(Constant.GAME_SETTINGS_DEFAULT_VALUE)))
                {
                    Debug.Log("JKJJK gameEvent==5    roomEventhing > 1," + spriteName);
                    ThrowRichiTile(spriteName, 0);
                }
                else
                {
                    ThrowTile(spriteName, 0);
                }


                string[] addingTile = new string[] { gameInfoData.roomInfo.drawTile };

                if (gameInfoData.roomInfo.drawTile != "")
                    handTiles = MergeArrays(handTiles, addingTile);
                string[] arrSubElements = new string[] { spriteName };
                string[] tilesRest = RemoveElementsFromArray(handTiles, arrSubElements);

                if ((gameInfoData.roomInfo.roomGameEvent == Constant.GAME_EVENTS_RIICHI) && (gameInfoData.roomInfo.roomEventHint == Convert.ToInt32(Constant.GAME_SETTINGS_DEFAULT_VALUE)))
                {
                    gameInfoData.roomInfo.roomEventHint = Constant.GAME_SETTINGS_DEFAULT_ACTIVATE;
                    SendPlayingInfo.SendController(Convert.ToInt32(Constant.GAME_EVENTS_RIICHI), tilesRest, saveTiles, spriteName);
                }
                else
                {
                    gameInfoData.roomInfo.roomEventHint = Convert.ToInt32(Constant.GAME_EVENTS_GAME_RUN);
                    gameInfoData.roomInfo.roomGameEvent = Constant.GAME_EVENTS_GAME_RUN;
                    SendPlayingInfo.SendController(Convert.ToInt32(Constant.GAME_EVENTS_GAME_RUN), tilesRest, saveTiles, spriteName);
                }
                transform.parent.GetComponent<HandTiles>().ShowHandTiles(tilesRest, null);
                HandTiles.isClicked = true;


                //int childIndex = Array.IndexOf(transform.parent.GetComponentsInChildren<Transform>().ToArray(), this.transform);
                //if (childIndex == -1)
                //{
                //    Vector3 newPosition = transform.position;
                //    newPosition.y -= 5;
                //    transform.position = newPosition;
                //}
                CancelTimer();
            }
        }

        public void OnAutoThrowLastTile(string tileName)
        {
            gameInfoData = HandTiles.gameInfoData;
            string[] handTiles = HandTiles.userInfo.handTiles;
            string[] saveTiles = HandTiles.userInfo.saveTiles;
            int myPlace = HandTiles.myPlace;
            if (handTiles.Length < 13 && handTiles.Length > 11) // temporary
                return;

            tileName = (gameInfoData.roomInfo.drawTile == "") ? handTiles[handTiles.Length - 1] : gameInfoData.roomInfo.drawTile;
            if (gameInfoData.roomInfo.roomGameEvent == Constant.GAME_EVENTS_RIICHI && HandTiles.userInfo.gameEvent == "1" && gameInfoData.roomInfo.roomEventHint != 5)
                ThrowTile(HandTiles.availableTiles[0], 0);
            else
                ThrowTile(tileName, 0);


            string[] addingTile = new string[] { gameInfoData.roomInfo.drawTile };

            if (gameInfoData.roomInfo.drawTile != "")
                handTiles = MergeArrays(handTiles, addingTile);

            string[] arrSubElements = new string[] { tileName };
            string[] tilesRest = RemoveElementsFromArray(handTiles, arrSubElements);
            SendPlayingInfo.SendController(1, tilesRest, saveTiles, tileName);
            transform.parent.GetComponent<HandTiles>().ShowHandTiles(tilesRest, null);
            HandTiles.isClicked = true;

            CancelTimer();
        }

        public void CancelTimer()
        {
            try
            {
                StopCoroutine(GameController.secondTimer);
                FindObjectOfType<TimeCount>().Clear();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        public static string[] GetSaveTiles(GameInfoData gameInfoData, string userId)
        {
            string[] saveTiles = { "0" };
            List<UserData> users = new List<UserData> { gameInfoData.user1, gameInfoData.user2, gameInfoData.user3, gameInfoData.user4 };
            foreach (var user in users)
            {
                if (user.userId == userId)
                {
                    saveTiles = user.saveTiles;
                    break;
                }
            }
            return saveTiles;
        }
        public static string[] GetHandTile(GameInfoData gameInfoData, string userId)
        {
            string[] handTiles = { "0" };
            List<UserData> users = new List<UserData> { gameInfoData.user1, gameInfoData.user2, gameInfoData.user3, gameInfoData.user4 };
            foreach (var user in users)
            {
                if (user.userId == userId)
                {
                    handTiles = user.handTiles;
                    break;
                }
            }
            return handTiles;
        }

        public static void SetPlyaerInfo(GameInfoData gameInfoData, int playerPlace)
        {
            string roomGameEvent = gameInfoData.roomInfo.roomGameEvent;
            List<UserData> users = new List<UserData> { gameInfoData.user1, gameInfoData.user2, gameInfoData.user3, gameInfoData.user4 };
            foreach (var user in users)
            {
                if (user.playerplace == playerPlace)
                {
                    user.gameEvent = roomGameEvent;
                    gameInfoData.roomInfo.roomGameEvent = Constant.GAME_EVENTS_RIICHI;
                    gameInfoData.roomInfo.roomEventHint = Convert.ToInt32(Constant.GAME_EVENTS_GAME_START);
                    user.eventHint = new string[] { };
                    break;
                }
            }
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

        public static UserData GetMyinfoData(GameInfoData gameInfoData, string userId)
        {
            UserData myUserInfo = new UserData();
            List<UserData> users = new List<UserData> { gameInfoData.user1, gameInfoData.user2, gameInfoData.user3, gameInfoData.user4 };
            foreach (var user in users)
            {
                if (user.userId == userId)
                {
                    myUserInfo = user;
                    break;
                }
            }
            return myUserInfo;
        }
        public static int[] replaceArray(string[] arr)
        {
            Dictionary<char, int> index_arr = new Dictionary<char, int>();
            index_arr['m'] = 1;
            index_arr['p'] = 2;
            index_arr['s'] = 3;
            index_arr['z'] = 4;
            string tmp = "";
            List<int> addedTiles = new List<int>();

            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i][0] == '0')
                {
                    arr[i] = arr[i].Replace('0', '5');
                    addedTiles.Add(i);
                }
            }

            return addedTiles.ToArray();
        }

        public static string[] MergeArrays(string[] array, string[] elements)
        {
            if (elements != null)
            {
                List<string> resultList = array.ToList();
                resultList.AddRange(elements);
                return resultList.ToArray();
            }
            else return array;
        }

        public static string[] RemoveElementsFromArray(string[] array, string[] elements)
        {
            List<string> arrayList = array.ToList();
            List<string> elementsList = elements.ToList();
            for (int i = 0; i < elementsList.Count; i++)
            {
                arrayList.Remove(elementsList[i]);
            }
            return arrayList.ToArray();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsDisable())
                return;
            Vector3 newPosition = transform.position;
            newPosition.y += 5;
            transform.position = newPosition;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsDisable())
                return;
            Vector3 newPosition = transform.position;
            newPosition.y = FindObjectOfType<HandTiles>().positionY;
            transform.position = newPosition;
        }

        public void ThrowTile(string name, int playerPlace)
        {
            string roomEvent = gameInfoData.roomInfo.roomGameEvent;
            GameObject root = this.gameObject.transform.root.gameObject;
            GameObject IndexPlayer = root.transform.GetChild(playerPlace).gameObject;
            GameObject Ground = IndexPlayer.transform.GetChild(0).gameObject;
            GameObject TileNew = GetResource(name);
            int activeChildCount = GetActiveChildCount(Ground);   //count of player>ground 
            GameObject GroundTile0 = Ground.transform.GetChild(activeChildCount).gameObject;
            GameObject endTile = GroundTile0.transform.GetChild(0).gameObject;
            GameObject newChild = Instantiate(TileNew, GroundTile0.transform) as GameObject;

            if (roomEvent == Constant.GAME_EVENTS_RIICHI && gameInfoData.roomInfo.roomEventHint == 5 && FindObjectOfType<HandTiles>().Contains(HandTiles.userInfo.eventHint, Constant.GAME_EVENTS_RIICHI))
            {
                newChild.transform.Rotate(0, -90, 0);
                SetRiichTilePosition(0);
            }

            newChild.transform.SetSiblingIndex(0);
            GroundTile0.SetActive(true);
            endTile.SetActive(true);
            Destroy(endTile, 0);
            newChild.SetActive(true);
            Debug.Log(name + ",  ThrowedTileName : " + GroundTile0.name + ",  " + " playerPlace: " + playerPlace);
        }
        public void ThrowRichiTile(string name, int playerPlace)
        {
            string roomEvent = gameInfoData.roomInfo.roomGameEvent;
            GameObject root = this.gameObject.transform.root.gameObject;
            GameObject IndexPlayer = root.transform.GetChild(playerPlace).gameObject;
            GameObject Ground = IndexPlayer.transform.GetChild(0).gameObject;
            GameObject TileNew = GetResource(name);
            int activeChildCount = GetActiveChildCount(Ground);   //count of player>ground 
            GameObject GroundTile0 = Ground.transform.GetChild(activeChildCount).gameObject;
            GameObject endTile = GroundTile0.transform.GetChild(0).gameObject;
            GameObject newChild = Instantiate(TileNew, GroundTile0.transform) as GameObject;

            newChild.transform.Rotate(0, -90, 0);
            SetRiichTilePosition(0);

            newChild.transform.SetSiblingIndex(0);
            GroundTile0.SetActive(true);
            endTile.SetActive(true);
            Destroy(endTile, 0);
            newChild.SetActive(true);
            Debug.Log(name + ",  ThrowedRichiTileName : " + GroundTile0.name + ",  " + " playerPlace: " + playerPlace);
        }

        public static List<string> ConvertNumbersToTiles(List<int> tileNumbers)
        {
            var tileTypes = new[] { 'm', 'p', 's', 'z' };

            return tileNumbers.Select(tileNumber =>
            {
                var type = tileTypes[tileNumber / 10];
                var number = tileNumber % 10;
                return number.ToString() + type;
            }).ToList();
        }

        public static List<int> ConvertTilesToNumbers(List<string> handTiles)
        {
            var tileTypes = new Dictionary<char, int> { { 'm', 0 }, { 'p', 10 }, { 's', 20 }, { 'z', 30 } };
            if (handTiles.Count > 0)
                return handTiles.Select(tile =>
                {
                    var type = tile.Last();
                    var number = int.Parse(tile.Substring(0, tile.Length - 1));
                    //if (number == 0) number = 5; // Treat 0m, 0p, 0s as 5m, 5p, 5s
                    return tileTypes[type] + number;
                }).ToList();
            else return null;
        }


        public static void sort(int[] num, int len)
        {
            bool isSwapped;

            for (int i = 0; i < len; i++)
            {
                isSwapped = false;
                for (int j = 1; j < len - i; j++)
                {
                    if (Compare(num[j], num[j - 1]) < 0)
                    {
                        swapNums(num, j, j - 1);
                        isSwapped = true;
                    }
                }
                if (!isSwapped)
                {
                    break;
                }
            }
        }

        public static int Compare(int a, int b)
        {
            // Map the values for comparison
            int aMapped = MapValue(a);
            int bMapped = MapValue(b);

            return aMapped.CompareTo(bMapped);
        }

        public static int MapValue(int value)
        {
            // Map the special cases
            if (value == 0) return 5;
            if (value == 10) return 15;
            if (value == 20) return 25;
            if (value == 30) return 35;
            return value;
        }

        public static void swapNums(int[] nums, int first, int second)
        {
            int curr = nums[first];
            nums[first] = nums[second];
            nums[second] = curr;
        }

        public static string[] SortArray(string[] arr)
        {
            string[] arryas = { "" };
            int[] tilearray = { };
            List<int> list = new List<int>();
            List<string> Tilelist = new List<string>();
            tilearray = ConvertTilesToNumbers(arr.ToList()).ToArray();
            sort(tilearray, tilearray.Length);
            arryas = ConvertNumbersToTiles(tilearray.ToList()).ToArray();

            return arryas;
        }

        public void SetRiichTilePosition(int playerPlace)
        {
            GameObject root = this.gameObject.transform.root.gameObject;
            GameObject IndexPlayer = root.transform.GetChild(playerPlace).gameObject;
            GameObject Ground = IndexPlayer.transform.GetChild(0).gameObject;
            int activeChildCount = GetActiveChildCount(Ground);   //count of player>ground 

            GameObject GroundTileTarget = Ground.transform.GetChild(activeChildCount).gameObject;
            //   Debug.Log(activeChildCount + " :  ===========270 : " + GroundTileTarget.name);

            if (activeChildCount < 6 && activeChildCount >= 0)
                for (int i = activeChildCount; i < 6; i++)
                {
                    //      Debug.Log(Ground.transform.GetChild(i).name + " :  i = : " + i);
                    if (i == activeChildCount)
                        Ground.transform.GetChild(i).transform.position += new Vector3(0.005f, 0, 0);  //Move Tile when call Riich 
                    else
                        Ground.transform.GetChild(i).transform.position += new Vector3(0.01f, 0, 0);  //Move Tile when call Riich 
                }

            else if (activeChildCount < 12 && activeChildCount >= 6)
                for (int i = activeChildCount; i < 12; i++)
                {
                    {
                        if (i == activeChildCount)
                            Ground.transform.GetChild(i).transform.position += new Vector3(0.005f, 0, 0);  //Move Tile when call Riich 
                        else
                            Ground.transform.GetChild(i).transform.position += new Vector3(0.01f, 0, 0);  //Move Tile when call Riich 
                    }
                }

            else if (activeChildCount < 18 && activeChildCount >= 12)
                for (int i = activeChildCount; i < 18; i++)
                {
                    {
                        if (i == activeChildCount)
                            Ground.transform.GetChild(i).transform.position += new Vector3(0.005f, 0, 0);  //Move Tile when call Riich 
                        else
                            Ground.transform.GetChild(i).transform.position += new Vector3(0.01f, 0, 0);  //Move Tile when call Riich 
                    }
                }

            else if (activeChildCount < 21 && activeChildCount >= 18)
                for (int i = activeChildCount; i < 21; i++)
                {
                    if (i == activeChildCount)
                        Ground.transform.GetChild(i).transform.position += new Vector3(0.005f, 0, 0);  //Move Tile when call Riich 
                    else
                        Ground.transform.GetChild(i).transform.position += new Vector3(0.01f, 0, 0);  //Move Tile when call Riich 
                }

            //GroundTileTarget.transform.Rotate(0.0f, -90.0f, 0.0f, Space.Self);  // Turn Tile left

            //GameObject endTile = GroundTileTarget.transform.GetChild(0).gameObject;
            //GroundTileTarget.SetActive(true);
            //endTile.SetActive(true);
            //GameObject TileNew = GetResource(name);
            //GameObject newChild = Instantiate(TileNew, GroundTileTarget.transform) as GameObject;
            //Destroy(endTile, 0);
            //newChild.SetActive(true);

        }

        public int GetActiveChildCount(GameObject ground)
        {
            return ground.transform.Cast<Transform>().Where(c => c.gameObject.activeSelf).ToList().Count;
        }


        public GameObject GetResource(string Tile)
        {
            var tileObj1 = Resources.Load<GameObject>($"Prefabs/Tiles/{Tile}");
            return tileObj1;
        }

        public static void FomartAllTilesDiable()
        {
            GameObject root = GameObject.Find("Player0").transform.root.gameObject;
            for (int i = 0; i < 4; i++)
            {
                GameObject IndexPlayerGround = root.transform.GetChild(i).GetChild(0).gameObject;
                GameObject IndexPlayerMeld = root.transform.GetChild(i).Find("Meld").gameObject;

                foreach (Transform child in IndexPlayerGround.transform)
                {
                    child.gameObject.SetActive(false);
                }
                foreach (Transform child in IndexPlayerMeld.transform)
                {
                    child.gameObject.SetActive(false);
                }

            }
            //disable Riichi bar in TableCanvas
            root.transform.Find("Table/board/BoardCanvas").GetComponent<TableBoard>().ClearAllStatus();
        }
    }
}
