using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant
{
        /* AWS */
    public static readonly string ServerAddress= "http://18.176.111.167";
 //  public static string ServerURL = "http://18.176.111.167:5000";
 

    

   //     /* LOCAL */
      public static readonly string ServerURL = "http://192.168.145.25:5000";  // Local
   
   



    //   public static readonly string ServerURL = "http://13.52.139.172:4000";
    public static readonly string ValPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    public static readonly string GAME_EVENTS_FIRST_ROUND = "100";
    public static readonly string GAME_EVENTS_GAME_START = "0";
    public static readonly string GAME_EVENTS_GAME_RUN = "1";
    public static readonly string GAME_EVENTS_CHI = "2";
    public static readonly string GAME_EVENTS_PON = "3";
    public static readonly string GAME_EVENTS_KAN = "4";
    public static readonly string GAME_EVENTS_RIICHI = "5";
    public static readonly string GAME_EVENTS_TSUMO = "6";
    public static readonly string GAME_EVENTS_RON = "7";
    public static readonly string GAME_EVENTS_COMPARE = "98";
    public static bool GAME_EVENTS_TSUMO_STATUS = true;
    public static bool GAME_EVENTS_RON_STATUS = false;


    public static readonly string GAME_SETTINGS_DEFAULT_EMPTY_VALUE = "";
    public static int GAME_SETTINGS_DEFAULT_VALUE = 0;
    public static int GAME_SETTINGS_DEFAULT_ACTIVATE = 1;
    public static int GAME_SETTINGS_DEFAULT_MAX_ORDER = 3;
    public static int GAME_SETTINGS_DEFAULT_WALL_TILE_COUNT = 14;
    public static int GAME_SETTINGS_DEFAULT_GAME_SCORE = 25000;
    public static int GAME_SETTINGS_DEFAULT_HANDTILE_COUNTS = 13;
}