using MahJongController;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeldSet:MonoBehaviour
{
    // Show Saved Tiles By UserPlace
    public static void SetTiles(string[] tiles, int userPlace, string meldShape)
    {
        Debug.Log($"Player{userPlace}/Meld/{meldShape}" );
        GameObject Meld = GameObject.Find($"Player{userPlace}/Meld/{meldShape}");
        GameObject MeldObject = Instantiate(Meld, Meld.transform.parent.transform) as GameObject;
        for (int i = 0; i < tiles.Length; i++)
        {
            Transform child = MeldObject.transform.GetChild(i);
            if (child == null)
            {
                Debug.LogError("No child found at index " + i + "!");
                continue;
            }
            MeldTile meldTile = child.GetComponent<MeldTile>();
            if (meldTile == null)
            {
                Debug.LogError("No MeldTile component found on child " + i + "!");
                continue;
            }
            meldTile.SetOneTile(tiles[i]);
        }

        foreach(Transform eleMeld in Meld.transform.parent)
        {
            if (eleMeld.gameObject.activeSelf)
            {
                float distanceToMove = 0.1f;
                if(eleMeld.gameObject.name.Contains("KanQueue"))
                  distanceToMove = 0.125f;
                else if(eleMeld.gameObject.name.Contains("MinKan"))
                  distanceToMove = 0.135f;
                Vector3 deltaPosition = new Vector3();
                switch (userPlace)
                {
                    case 0:
                        deltaPosition = new Vector3(distanceToMove, 0, 0);
                        break;
                    case 1:
                        deltaPosition = new Vector3(0, 0, distanceToMove);
                        break;
                    case 2:
                        deltaPosition = new Vector3(-distanceToMove, 0, 0);
                        break;
                    case 3:
                        deltaPosition = new Vector3(0, 0, -distanceToMove);
                        break;
                }
                Vector3 newPosition = MeldObject.transform.position - deltaPosition;
                MeldObject.transform.position = newPosition;
            }
        }
        MeldObject.SetActive(true);
    }

}