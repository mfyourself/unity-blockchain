using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections.Generic;

namespace MahJongController
{
    public class YaKuHint : MonoBehaviour
    {
        private const int MaxTileCount = 40;
        private const int MinHandLength = 14;

        public bool IsTenpai(int[] tiles)
        {
            var possibleTiles = new List<int>();
            for (int i = 1; i <= 9; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    char suit = (i == 0) ? 'm' : (i == 1) ? 'p' : 's';
                    possibleTiles.Add(ConvertTile(suit, i + 1));
                }
            }
            for (int i = 1; i <= 7; i++)
            {
                possibleTiles.Add(ConvertTile('z', i));
            }

            foreach (int tile in possibleTiles)
            {
                var tempHand = new int[tiles.Length + 1];
                Array.Copy(tiles, tempHand, tiles.Length);
                tempHand[tiles.Length] = tile;

                if (CanCompleteHand(tempHand))
                {
                    return true;
                }
            }
            return false;
        }

        private int ConvertTile(char type, int number)
        {
            return type switch
            {
                'm' => 0,
                'p' => 10,
                's' => 20,
                'z' => 30,
                _ => throw new ArgumentException("Invalid tile type", nameof(type)),
            } + number;
        }

        private bool CanCompleteHand(int[] tiles)
        {
            if (tiles.Length < MinHandLength) return false;

            var counts = new int[MaxTileCount];
            foreach (int tile in tiles)
            {
                counts[tile]++;
            }

            for (int i = 0; i < counts.Length; i++)
            {
                if (counts[i] >= 2)
                {
                    var countsCopy = (int[])counts.Clone();
                    countsCopy[i] -= 2;
                    if (IsValidHand(countsCopy))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsValidHand(int[] counts)
        {
            bool HasSequences = true;

            for (int i = 0; i < MaxTileCount; i++)
            {
                while (counts[i] >= 3)
                {
                    counts[i] -= 3;
                }
            }

            for (int i = 0; i < MaxTileCount; i++)
            {
                if (i % 10 <= 7 && counts[i] > 0 && counts[i + 1] > 0 && counts[i + 2] > 0)
                {
                    while (counts[i] > 0 && counts[i + 1] > 0 && counts[i + 2] > 0)
                    {
                        counts[i]--;
                        counts[i + 1]--;
                        counts[i + 2]--;
                    }
                    HasSequences = false;
                    break;
                }
            }

            for (int i = 0; i < MaxTileCount; i++)
            {
                if (counts[i] != 0)
                {
                    HasSequences = false;
                    break;
                }
            }

            return HasSequences;
        }
    }

}
