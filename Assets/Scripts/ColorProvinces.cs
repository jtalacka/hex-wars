using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorProvinces : MonoBehaviour
{
    public static void ColorAllProvinces()
    {
        foreach (var player in Players.players)
        {
            foreach(var province in player.provinces)
            {
                TileColorHandler.ColorTiles(province.teritories, player.color);
            }
        }
    }
}
