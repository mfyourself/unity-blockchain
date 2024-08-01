using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGrandTileList
{
    public class GrandTile : MonoBehaviour
    {
        public static GameObject me;
        // Start is called before the first frame update
        public void ShowMe(string tileName, int order){
            me = this.gameObject.transform.GetChild(0).gameObject;
            var newGameObject = Resources.Load<GameObject>($"Prefabs/Tiles/{tileName}");
            // me.

        }
    }
}
