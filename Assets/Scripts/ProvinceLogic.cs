using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProvinceLogic : MonoBehaviour
{
    private WorldTile _tile;
    public bool objectPressed = false;
    private Vector3 lastMouseCoordinate = Vector3.zero;
    public Province province;
    public bool remove = false;


    // Update is called once per frame

    private void Start()
    {
        var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles
        province.teritories.ForEach((position) =>
        {
            if (tiles.TryGetValue(position, out _tile))
            {
                _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
                Color color = Color.green;
                color.a = 0.5f;
                _tile.TilemapMember.SetColor(_tile.LocalPlace, color);
            }
        }
        );

    }
    private void Update()
    {

        if (objectPressed)
            {
            if (Input.GetKeyDown("space"))
            {
                remove = !remove;
            }
                Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var current = GameTiles.instance.tilemap[0].WorldToCell(point);
                if (current != lastMouseCoordinate)
                {
                lastMouseCoordinate = current;
                var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles
                var tilemap = GameTiles.instance.tilemap[0];
                print(current);
                if (tiles.TryGetValue(current, out _tile))
                {
                    if (!province.teritories.Contains(_tile.LocalPlace))
                    {
                        if (!remove)
                        {
                            _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
                            Color color = Color.green;
                            color.a = 0.5f;
                            _tile.TilemapMember.SetColor(_tile.LocalPlace, color);
                            province.teritories.Add(_tile.LocalPlace);
                        }
                    }
                    else
                    {
                        if (remove)
                        {
                            _tile.TilemapMember.SetColor(current, new Color(1, 1, 1, 1));
                            province.teritories.RemoveAt(province.teritories.FindIndex(t=>t==current));
                        }
                    }
                }
                }
            }

    }
    public Vector3Int getDirection(Vector3Int current){
 
        current = current - province.teritories[province.teritories.Count - 1];
        if (current.x != 0)
            current.x = current.x / Mathf.Abs(current.x);
        if (current.y != 0)
            current.y = current.y / Mathf.Abs(current.y);
        current += province.teritories[province.teritories.Count - 1];
        return current;
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Input.ResetInputAxes();
            if (!objectPressed)
            {
                print("testmouse");

                objectPressed = true;
                var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles
                Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lastMouseCoordinate = GameTiles.instance.tilemap[0].WorldToCell(point);
                var worldPoint = GameTiles.instance.tilemap[0].WorldToCell(point);

                if (tiles.TryGetValue(worldPoint, out _tile))
                {
                    print("Tile " + _tile.Name + " costs: " + _tile.Cost);
                    _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
                    Color color = Color.green;
                    color.a = 0.5f;
                    print(_tile.TilemapMember.color);
                    _tile.TilemapMember.SetColor(_tile.LocalPlace, color);
                    province.center=_tile.LocalPlace;
                }

                objectPressed = true;
            }
            else
            {
                objectPressed = false;
            }
            }
        }

    
    private void resetColorFromSelected(int begining, int end)
    {

    }
}
