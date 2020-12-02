using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileColorHandler : MonoBehaviour
{
    public static void ColorTiles(List<Vector3Int> tiles, Color color)
    {
        WorldTile _tile;
        var worldTiles = GameTiles.instance.tiles; // This is our Dictionary of tiles
        tiles.ForEach((position) =>
        {
            if (worldTiles.TryGetValue(position, out _tile))
            {
                _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
                _tile.TilemapMember.SetColor(_tile.LocalPlace, color);
            }
        }
        );

    }

    public static void RecolorTiles(List<Vector3Int> tiles)
    {
        WorldTile tile;
        var worldTiles = GameTiles.instance.tiles;
        tiles.ForEach((position) =>
        {
            if (worldTiles.TryGetValue(position, out tile))
            {
                tile.TilemapMember.SetColor(tile.LocalPlace, new Color(1, 1, 1, 1));
            }
        }
      );
    }
}
