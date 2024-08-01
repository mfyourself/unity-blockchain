using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MahJongController
{
    public class MeldTile : MonoBehaviour
    {
        public void SetOneTile(string tileName)
        {
            GameObject TileCharcter = this.transform.GetChild(0).gameObject;
   //         Debug.Log(TileCharcter.name + ":MeldTile.cs 13:" + tileName);
            GameObject TileNew = FindObjectOfType<HandTile>().GetResource(tileName);
            GameObject newChild = Instantiate(TileNew, TileCharcter.transform.parent) as GameObject;
            Destroy(TileCharcter, 0);
            newChild.SetActive(true);
        }
    }

}