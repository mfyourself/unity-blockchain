using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAuthStructure;
using System.Linq;
using MahjongLobbyController;

namespace MahJongController
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private HandTiles handTilesObj;
        public DisplayPlayersInfo playerInfo;
        public static int myPlace;
        private string userId;
        private bool isMyTurn;
        public string drawTile, userClickedTile;
        public GameInfoData gameInfoData;
        private Communication communication;
        public static int myPlusTime = 10;
        public static IEnumerator secondTimer = null;

        public void Awake()
        {
            communication = FindObjectOfType<Communication>();
        }

        public void Start()
        {
            gameInfoData = GameInfo1.Instance.GetRecvGI();
            ReceivedUserData receivedUserData = AuthStructure.Instance.GetUserData();

            if (communication)
                communication.OnReceiveDataOfStartGame.AddListener(OnReceive);

            string gameEvent = Convert.ToString(gameInfoData.roomInfo.roomGameEvent);
            string roomCreater = Convert.ToString(gameInfoData.roomInfo.roomCreater);
            userId = receivedUserData.userId;

            if (gameEvent == "100" && userId == roomCreater)
            {
                string[] str = { };
                SendPlayingInfo.SendController(0, str, str, "");
            }

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
        public static void Remove3DHandTiles(GameInfoData gameInfoData, int myPlace)
        {
            List<UserData> users = new List<UserData> { gameInfoData.user1, gameInfoData.user2, gameInfoData.user3, gameInfoData.user4 };
            foreach (var user in users)
            {
                var playerPosition = SetPlaceTile(myPlace, user.playerplace);
                FindObjectOfType<HandTiles>().RemoveTiles3D(playerPosition, user.handTiles.Length);

            }
        }
        public static int GetPlayerPlace(GameInfoData gameInfoData)
        {

            int playerPlaces = 0;
            List<UserData> users = new List<UserData> { gameInfoData.user1, gameInfoData.user2, gameInfoData.user3, gameInfoData.user4 };

            foreach (var user in users)
            {
                if (user.playerplace == gameInfoData.roomInfo.order)
                {
                    playerPlaces = user.playerplace;
                    break;
                }
            }
            return playerPlaces;
        }
        public static bool DicisionAuto(GameInfoData gameInfoData, int order)
        {
            bool autoFlag = false;
            List<UserData> users = new List<UserData> { gameInfoData.user1, gameInfoData.user2, gameInfoData.user3, gameInfoData.user4 };

            foreach (var user in users)
            {
                if (user.playerplace == order)
                {
                    if (user.userId == "999997" || user.userId == "999998" || user.userId == "999999")
                    {
                        autoFlag = true;
                        break;
                    }
                }
            }

            return autoFlag;
        }

        public static bool CheckMyStatus(GameInfoData gameInfoData, int order, string userId)
        {
            bool me = false;
            List<UserData> users = new List<UserData> { gameInfoData.user1, gameInfoData.user2, gameInfoData.user3, gameInfoData.user4 };

            foreach (var user in users)
            {
                if (user.userId == userId)
                {
                    if (user.playerplace == order)
                    {
                        me = true;
                        break;
                    }
                }
            }

            return me;
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
        public static UserData GetPlayerData(GameInfoData gameInfoData, int playerPlace)
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
        public static int SetPlaceTile(int myPlace, int order)
        {
            int playerPlace = 0;
            playerPlace = order >= myPlace ? order - myPlace : 4 + order - myPlace;

            return playerPlace;
        }
        private string[] GetLastElements(string[] arrStr, int n)
        {
            if (n == 3)
                return new string[] { arrStr[arrStr.Length - 3], arrStr[arrStr.Length - 2], arrStr[arrStr.Length - 1] };
            else
                return new string[] { arrStr[arrStr.Length - 4], arrStr[arrStr.Length - 3], arrStr[arrStr.Length - 2], arrStr[arrStr.Length - 1] };
        }

        public void OnReceive(string data)
        {
            if (secondTimer != null)
                StopCoroutine(secondTimer);
            gameInfoData = GameInfo1.Instance.GetRecvGI();
            HandTiles.gameInfoData = gameInfoData;
            string gameEvent = Convert.ToString(gameInfoData.roomInfo.roomGameEvent);


            //if (gameInfoData.roomInfo.remineTiles.Length > 70)
            //{
            //    gameInfoData.user1.handTiles = new string[] { "1s", "1s", "1s", "1s", "1m", "2m", "4m", "6m", "5m", "8m", "7m", "9m", "1p" };
            //    gameInfoData.user2.handTiles = new string[] { "1p", "1p", "1p", "1p", "3p", "3p", "7m", "1m", "0m", "4z", "4z", "2p", "3p" };
            //    gameInfoData.user3.handTiles = new string[] { "2s", "2s", "2s", "2s", "3m", "3m", "4m", "5m", "6m", "6z", "3z", "3z", "5z" };
            //    gameInfoData.user4.handTiles = new string[] { "3s", "3s", "3s", "3s", "6m", "8s", "4z", "8z", "8m", "5z", "8z", "7m", "6m" };
            //}

            int roomHint = gameInfoData.roomInfo.roomEventHint;
            UserData userinfo = GetMyinfoData(gameInfoData, userId);
            string[] handTiles = GetHandTile(gameInfoData, userId);
            drawTile = gameInfoData.roomInfo.drawTile;
            myPlace = GetMyPlace(gameInfoData, userId);
            int gameOrder = gameInfoData.roomInfo.order;
            isMyTurn = (myPlace == gameOrder) ? true : false;
            HandTiles.myPlace = myPlace;
            HandTiles.isClicked = !isMyTurn;
            HandTiles.userInfo = userinfo;
            handTilesObj.ShowHandTiles(handTiles, (isMyTurn ? drawTile : null));
            UserData hisGameInfoData = GetPlayerData(gameInfoData, gameOrder);
            int hisTablePosition = gameOrder >= myPlace ? gameOrder - myPlace : 4 + gameOrder - myPlace;
            int last_order = gameOrder == 0 ? 3 : gameOrder - 1;
            int playerPlace = SetPlaceTile(myPlace, last_order);
            int tmpOrder = gameOrder;
            userClickedTile = HandTiles.GetClickedTile(gameInfoData, last_order);
            bool myStatus = CheckMyStatus(gameInfoData, last_order, userId);

            //Show 
            FindObjectOfType<StatusBoard>().PutDoras(gameInfoData.roomInfo.doraTiles);
            FindObjectOfType<StatusBoard>().PutTitle(gameInfoData.roomInfo.roomType, gameInfoData.roomInfo.maxOfPlayer);
            FindObjectOfType<TableBoard>().SetRemindTilesDisplay(gameInfoData.roomInfo.remineTiles.Length);
            FindObjectOfType<TableBoard>().ShowCurrentUser((tmpOrder >= myPlace) ? (tmpOrder - myPlace) : 4 + tmpOrder - myPlace);
            FindObjectOfType<TableBoard>().SetPoints(gameInfoData, myPlace);
            FindObjectOfType<TableBoard>().SetRoundDisplay(gameInfoData.roomInfo.roomRound);
            if (gameEvent == Constant.GAME_EVENTS_GAME_START)
            {
                FindObjectOfType<TableBoard>().SetDirection((gameInfoData.roomInfo.roomDealer >= myPlace) ? (gameInfoData.roomInfo.roomDealer - myPlace) : 4 + gameInfoData.roomInfo.roomDealer - myPlace);
                FindObjectOfType<StatusBoard>().ClearRiichiCount();
                myPlusTime = 10;
                playerInfo.DisplayUserInfo(myPlace);
                HandTile.FomartAllTilesDiable();
                FindObjectOfType<DisplayPlayersInfo>().DisplayUserInfo(myPlace);
            }
            if (gameInfoData.roomInfo.roomGameEvent == Constant.GAME_EVENTS_COMPARE || gameInfoData.roomInfo.remineTiles.Length == 14)
            {
                {
                    HandTile.FomartAllTilesDiable();
                    HandTiles.isClicked = true;
                    ShowPointPage();


                    if (gameInfoData.roomInfo.roomType == 1 && gameInfoData.roomInfo.roomRound == 4)
                    {
                        ShowFinalPage();
                    }
                    else if (gameInfoData.roomInfo.roomType == 2 && gameInfoData.roomInfo.roomRound == 8)
                    {
                        ShowFinalPage();
                    }
                }
            }

            if (!isMyTurn)
                if (secondTimer != null)
                {
                    FindObjectOfType<TimeCount>().Clear();
                    StopCoroutine(secondTimer);
                }







            // Switch (roomGameEvent)

            if (gameEvent == Constant.GAME_EVENTS_GAME_RUN)
            {
                Remove3DHandTiles(gameInfoData, myPlace);

                if (!myStatus)
                {
                    handTilesObj.ThrowChildTile(userClickedTile, playerPlace.ToString());
                }

                for (int i = 0; i < userinfo.eventHint.Length; i++)
                {
                    string e = userinfo.eventHint[i];
                    if (userinfo.gameEvent != Constant.GAME_EVENTS_RIICHI)
                    {
                        if (e == Constant.GAME_EVENTS_CHI)
                            FindObjectOfType<HandTiles>().ChiShow();
                        if (e == Constant.GAME_EVENTS_PON)
                            FindObjectOfType<HandTiles>().PonShow();
                        if (e == Constant.GAME_EVENTS_KAN)
                            FindObjectOfType<HandTiles>().KanShow();
                        if (e == Constant.GAME_EVENTS_RIICHI)
                            FindObjectOfType<HandTiles>().RiichShow();

                        if (!isMyTurn)
                        {
                            if (secondTimer != null)
                            {
                                StopCoroutine(secondTimer);
                                FindObjectOfType<TimeCount>().Clear();
                            }
                            secondTimer = SetClock(5, 0, 0);
                            Debug.Log(303);
                            StartCoroutine(secondTimer);
                        }
                    }
                    if (e == Constant.GAME_EVENTS_TSUMO)
                        FindObjectOfType<HandTiles>().TusmoShow();
                    if (e == Constant.GAME_EVENTS_RON)
                        FindObjectOfType<HandTiles>().RonShow();
                }

            }

            else if (gameEvent == Constant.GAME_EVENTS_CHI)
            {
                if (roomHint == 2)
                {
                    if (hisTablePosition > 0)
                    {
                        int keyTilePosition = (hisTablePosition == 0) ? 3 : hisTablePosition - 1;
                        FindObjectOfType<HandTiles>().ShowTableTile(keyTilePosition, false); // Hide table key tile of Chi
                        string[] lastSaveTiles = GetLastElements(hisGameInfoData.saveTiles, 3);
                        MeldSet.SetTiles(lastSaveTiles, hisTablePosition, "MeldThreeL");  // Put 3D Save Tiles

                    }
                }
            }

            else if (gameEvent == Constant.GAME_EVENTS_PON)
            {
                if (roomHint > 1)
                {
                    if (hisTablePosition > 0)
                    {
                        int keyTileOrder = (HandTiles.lastGameInfo.roomInfo.order == 0) ? 3 : HandTiles.lastGameInfo.roomInfo.order - 1;
                        int LastUserPlayerPlace = HandTiles.GetPlayerInfo(HandTiles.lastGameInfo, keyTileOrder).playerplace;
                        int lastUserTablePlace = (LastUserPlayerPlace >= myPlace) ? LastUserPlayerPlace - myPlace : 4 + LastUserPlayerPlace - myPlace;
                        FindObjectOfType<HandTiles>().ShowTableTile(lastUserTablePlace, false);

                        string[] lastSaveTiles = GetLastElements(hisGameInfoData.saveTiles, 3);
                        string shape = "MeldThreeL";
                        MeldSet.SetTiles(lastSaveTiles, hisTablePosition, shape);  // Put 3D Save Tiles
                    }
                }
            }


            else if (gameEvent == Constant.GAME_EVENTS_KAN)
            {
                if (roomHint > 1)
                {
                    if (hisTablePosition > 0)
                    {
                        string[] lastSaveTiles = GetLastElements(hisGameInfoData.saveTiles, 4);

                        int keyTileOrder = (HandTiles.lastGameInfo.roomInfo.order == 0) ? 3 : HandTiles.lastGameInfo.roomInfo.order - 1;
                        int LastUserPlayerPlace = HandTiles.GetPlayerInfo(HandTiles.lastGameInfo, keyTileOrder).playerplace;
                        int lastUserTablePlace = (LastUserPlayerPlace >= myPlace) ? LastUserPlayerPlace - myPlace : 4 + LastUserPlayerPlace - myPlace;
                        FindObjectOfType<HandTiles>().ShowTableTile(lastUserTablePlace, false); // Hide table key tile of Chi


                        //FindObjectOfType<HandTiles>().ShowTableTile(lastUserTablePlace, false);
                        string keyTile = (gameInfoData.roomInfo.drawTile == "") ? FindObjectOfType<HandTiles>().GetOtherPlayersCLickedTile(gameInfoData, last_order) : gameInfoData.roomInfo.drawTile;


                        string shape = FindObjectOfType<HandTiles>().GetKanShape(lastUserTablePlace, keyTile, lastSaveTiles);
                        MeldSet.SetTiles(lastSaveTiles, hisTablePosition, shape);  // Put 3D Save Tiles


                    }
                }
            }

            else if (gameEvent == Constant.GAME_EVENTS_RIICHI)
            {
                if (roomHint < Constant.GAME_SETTINGS_DEFAULT_ACTIVATE)
                {
                    if (!isMyTurn)
                    {
                        FindObjectOfType<StatusBoard>().AddRiichiCount();
                        FindObjectOfType<TableBoard>().SetRichiiStatus(hisTablePosition);
                    }
                    else
                    {
                        FindObjectOfType<HandTiles>().CheckRiichiAbleTiles(userinfo);
                    }
                }
                else if (roomHint >= Constant.GAME_SETTINGS_DEFAULT_ACTIVATE)
                {
                    if (!myStatus)
                    {
                        FindObjectOfType<HandTile>().ThrowRichiTile(userClickedTile, playerPlace);
                        if (userinfo.eventHint.Length > 0)
                        {
                            for (int i = 0; i < userinfo.eventHint.Length; i++)
                            {
                                string e = userinfo.eventHint[i];
                                if (userinfo.gameEvent != Constant.GAME_EVENTS_RIICHI)
                                {
                                    if (e == Constant.GAME_EVENTS_CHI)
                                        FindObjectOfType<HandTiles>().ChiShow();
                                    if (e == Constant.GAME_EVENTS_PON)
                                        FindObjectOfType<HandTiles>().PonShow();
                                    if (e == Constant.GAME_EVENTS_KAN)
                                        FindObjectOfType<HandTiles>().KanShow();
                                    if (e == Constant.GAME_EVENTS_RIICHI)
                                        FindObjectOfType<HandTiles>().RiichShow();

                                    if (secondTimer != null)
                                    {
                                        FindObjectOfType<TimeCount>().Clear();
                                        StopCoroutine(secondTimer);
                                    }
                                    secondTimer = SetClock(5, 0, 0);
                                    Debug.Log(416);
                                    StartCoroutine(secondTimer);

                                }
                                if (e == Constant.GAME_EVENTS_TSUMO)
                                    FindObjectOfType<HandTiles>().TusmoShow();
                                if (e == Constant.GAME_EVENTS_RON)
                                    FindObjectOfType<HandTiles>().RonShow();
                            }
                        }
                    }
                }
            }

            else if (gameEvent == Constant.GAME_EVENTS_RON)
            {
                StartCoroutine(showWinAgarisPage(gameInfoData));

            }

            else if (gameEvent == Constant.GAME_EVENTS_TSUMO)
            {
                StartCoroutine(showWinAgarisPage(gameInfoData));
            }

            if (isMyTurn && gameEvent != Constant.GAME_EVENTS_RON && gameEvent != Constant.GAME_EVENTS_TSUMO)
            {
                Debug.Log(444);
                secondTimer = SetClock(5, myPlusTime, 0);
                StartCoroutine(secondTimer);
            }

            HandTiles.lastGameInfo = gameInfoData;
        }



        private IEnumerator showWinAgarisPage(GameInfoData gameInfoData)
        {
            //int winner = 3; // winner playerplace
            int winner = gameInfoData.roomInfo.roundWinner[0]; // winner playerplace
            UserData winnerInfo = GetPlayerData(gameInfoData, winner); // winner own information
            bool AgariType = (gameInfoData.roomInfo.roomGameEvent == Convert.ToString(Constant.GAME_EVENTS_TSUMO)) ? true : false;
            string[] playerHandtiles = winnerInfo.handTiles;
            List<string> handtiles = playerHandtiles.ToList();
            int order = gameInfoData.roomInfo.order;
            int lastUserPlace = (order == Constant.GAME_SETTINGS_DEFAULT_VALUE) ? Constant.GAME_SETTINGS_DEFAULT_MAX_ORDER : order - Constant.GAME_SETTINGS_DEFAULT_ACTIVATE;
            string winTile = AgariType ? drawTile : userClickedTile;
            string[] playerSaveTiles = winnerInfo.saveTiles.Length > Constant.GAME_SETTINGS_DEFAULT_VALUE ? winnerInfo.saveTiles : null;

            if (!AgariType)
            {
                if (playerSaveTiles != null)
                {
                    handtiles.AddRange(playerSaveTiles);
                }
            }
            handtiles.Add(winTile);

            string[] mergedArray = handtiles.ToArray<string>();

            if (!AgariType)
            {
                FindObjectOfType<EffecctPanel>().RonRiichiNotification(SetPlaceTile(myPlace, winner));
                yield return new WaitUntil(() => FindObjectOfType<EffecctPanel>().IsAnimationCompleted());
            }

            FindObjectOfType<TsumoWinPage>().StartAnimation(string.Format("cutin_{0}", winnerInfo.avatar));
            yield return new WaitUntil(() => FindObjectOfType<TsumoWinPage>().IsAnimationCompleted());

            FindObjectOfType<PointSummary>().ShowPointSummary(mergedArray, gameInfoData.roomInfo.doraTiles, gameInfoData.roomInfo.hiddenDora, AgariType, winnerInfo.userName, winnerInfo.roundScore.ToString(), string.Format("cutin_{0}", winnerInfo.avatar), gameInfoData.roomInfo.yakuList, gameInfoData.roomInfo.roomRound, gameInfoData.roomInfo.han, gameInfoData.roomInfo.fu);
            yield return new WaitUntil(() => FindObjectOfType<PointSummary>().IsAnimationCompleted());

            ShowPointPage();
            yield return new WaitUntil(() => FindObjectOfType<ScoreTransfer>().IsCompletedDisplayPointDuration());

            if (gameInfoData.roomInfo.roomRound == (4 * gameInfoData.roomInfo.roomType))
            {
                ShowFinalPage();
            }
        }

        private void ShowPointPage()
        {
            FindObjectOfType<ScoreTransfer>().ShowPoints(gameInfoData, myPlace);

        }

        private void ShowFinalPage()
        {
            FindObjectOfType<IngameFinal>().ShowFinalPoints(gameInfoData, myPlace);
        }

        public IEnumerator SetClock(int baseTIme, int bonusTime, int typeAction)
        {
            for (int i = 0; i <= baseTIme + bonusTime; i++)
            {
                yield return new WaitForSeconds(1);
                FindObjectOfType<TimeCount>().DisplaySeonds(i, baseTIme, bonusTime);
            }
            FindObjectOfType<TimeCount>().Clear();

            if (gameInfoData.roomInfo.drawTile != Constant.GAME_SETTINGS_DEFAULT_EMPTY_VALUE || (gameInfoData.roomInfo.roomGameEvent == HandTiles.userInfo.gameEvent))
            {

                FindFirstObjectByType<HandTile>().OnAutoThrowLastTile(null);
            }
            else if (FindObjectOfType<HandTiles>().isCPK() )
            {
                FindObjectOfType<HandTiles>().CancelCPK();
            }
            myPlusTime = 0;

        }
        private void OnDisable()
        {
            if (communication)
                communication.OnReceiveDataOfStartGame.RemoveListener(OnReceive);
        }

    }

    public class RiichiMahjong
    {
        private List<int> _usedTileNumbers;

        public bool CanDeclareRiichi(List<string> handTiles, string drawTile, out List<string> usedTiles)
        {
            usedTiles = new List<string>();

            if (handTiles == null || handTiles.Count != 13)
            {
                return false;
            }
            List<int> tiles = new List<int>();
            if (drawTile == Constant.GAME_SETTINGS_DEFAULT_EMPTY_VALUE)
            {
                tiles = ConvertTilesToNumbers(handTiles.ToList());
            }
            else
            {
                ConvertTilesToNumbers(handTiles.Append(drawTile).ToList());
            }
            bool isTenpai = IsTenpai(tiles);

            if (isTenpai)
            {
                Console.WriteLine(_usedTileNumbers.ToString());
                usedTiles = ConvertNumbersToTiles(_usedTileNumbers);
            }

            return isTenpai;
        }

        public List<int> ConvertTilesToNumbers(List<string> handTiles)
        {
            var tileTypes = new Dictionary<char, int> { { 'm', 0 }, { 'p', 10 }, { 's', 20 }, { 'z', 30 } };

            return handTiles.Select(tile =>
            {
                var type = tile.Last();
                var number = int.Parse(tile.Substring(0, tile.Length - 1));
                if (number == 0) number = 5; // Treat 0m, 0p, 0s as 5m, 5p, 5s
                return tileTypes[type] + number;
            }).ToList();
        }

        public List<int> getUnUsedTiles(List<int> tiles)
        {
            var possibleTiles = new List<int>();
            for (int i = 0; i < tiles.Count; i++)
            {
                var tempHand = new List<int>(tiles);
                int removedTile = tempHand[i];
                tempHand.RemoveAt(i);

                if (IsTenpai(tempHand))
                {
                    possibleTiles.Add(removedTile);
                    //        Debug.Log("==================Removing this tile results in non-Tenpai: " + removedTile);
                }
            }
            return possibleTiles;

        }

        private bool IsTenpai(List<int> tiles)
        {
            bool TenpaiS = false;
            var possibleTiles = new List<int>();
            for (int i = 1; i <= 9; i++)
            {
                foreach (var suit in new[] { 'm', 'p', 's' })
                {
                    possibleTiles.Add(ConvertTile(suit, i));
                }
            }
            for (int i = 1; i <= 7; i++)
            {
                possibleTiles.Add(ConvertTile('z', i));
            }


            foreach (var tile in possibleTiles)
            {
                var tempHand = new List<int>(tiles) { tile };
                string tempHandString = string.Join(", ", tempHand);
                string tilesstring = string.Join(", ", tiles);

                if (CanCompleteHand(tempHand))
                {
                    //       Debug.Log("==================" + tempHandString + "^^^^^^^^^^" + tile + "*************" + tilesstring);
                    TenpaiS = true;
                }
            }




            return TenpaiS;
        }

        private int ConvertTile(char type, int number)
        {
            var tileTypes = new Dictionary<char, int>
     {
         {'m', 0}, {'p', 10}, {'s', 20}, {'z', 30}
     };

            return tileTypes[type] + number;
        }

        private bool CanCompleteHand(List<int> tiles)
        {
            if (tiles.Count < 14) return false;

            var counts = new int[40];
            foreach (var tile in tiles)
            {
                counts[tile]++;
            }

            for (int i = 0; i < counts.Length; i++)
            {
                if (counts[i] >= 2)
                {
                    var countsCopy = (int[])counts.Clone();
                    countsCopy[i] -= 2;
                    if (IsValidHand(countsCopy, out List<int> tempUsedTiles))
                    {
                        _usedTileNumbers = tempUsedTiles;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsValidHand(int[] counts, out List<int> usedTiles)
        {
            usedTiles = new List<int>();
            return FindMelds(counts, 0, usedTiles);
        }

        private bool FindMelds(int[] counts, int melds, List<int> currentUsedTiles)
        {
            if (melds == 4)
            {
                _usedTileNumbers = new List<int>(currentUsedTiles); // Store the used tiles
                return true;
            }

            for (int i = 0; i < counts.Length; i++)
            {
                if (counts[i] >= 3)
                {
                    var countsCopy = (int[])counts.Clone();
                    countsCopy[i] -= 3;
                    var newUsedTiles = new List<int>(currentUsedTiles) { i, i, i };
                    if (FindMelds(countsCopy, melds + 1, newUsedTiles))
                    {
                        return true;
                    }
                }
                if (i % 10 < 8 && counts[i] > 0 && counts[i + 1] > 0 && counts[i + 2] > 0)
                {
                    var countsCopy = (int[])counts.Clone();
                    countsCopy[i]--;
                    countsCopy[i + 1]--;
                    countsCopy[i + 2]--;
                    var newUsedTiles = new List<int>(currentUsedTiles) { i, i + 1, i + 2 };
                    //     Debug.Log("----------------" + newUsedTiles.ToString());
                    if (FindMelds(countsCopy, melds + 1, newUsedTiles))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public List<string> ConvertNumbersToTiles(List<int> tileNumbers)
        {
            var tileTypes = new[] { 'm', 'p', 's', 'z' };

            return tileNumbers.Select(tileNumber =>
            {
                var type = tileTypes[tileNumber / 10];
                var number = tileNumber % 10;
                return number.ToString() + type;
            }).ToList();
        }
    }

}
