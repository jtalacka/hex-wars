using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    private WorldTile _tile;
    public float speed;
    public float length;
    public GameObject Army;
    private bool initPressed;
    private bool moving = false;
    private bool objectPressed = false;
    private List<WorldTile> tile = new List<WorldTile>();
    Vector3 lastMouseCoordinate = Vector3.zero;
    // Update is called once per frame
    Tilemap tilemap;
    private void Start()
    {
        tilemap = GameTiles.instance.Tilemap;

    }
    private void Update()
    {
        if (!moving)
        {

            if (objectPressed)
            {
                checkMovement();

            }


            if (initPressed)
            {
                Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var current = this.tilemap.WorldToCell(point);
                if (current.x != lastMouseCoordinate.x || current.y != lastMouseCoordinate.y)
                {
                    lastMouseCoordinate = current;
                    var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles
                    var tilemap = GameTiles.instance.Tilemap;
                    current = current - tile[tile.Count - 1].LocalPlace;
                    if (current.x != 0)
                        current.x = current.x / Mathf.Abs(current.x);
                    if (current.y != 0)
                        current.y = current.y / Mathf.Abs(current.y);
                    current += tile[tile.Count - 1].LocalPlace;

                    if (tiles.TryGetValue(current, out _tile))
                    {
                        if (Vector3.Distance(tile[tile.Count - 1].WorldLocation, _tile.WorldLocation) < 1f)
                        {
                            if (!tile.Contains(_tile))
                            {
                                if (tile.Count < length)
                                {
                                    _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
                                    Color color = Color.green;
                                    color.a = 0.5f;
                                    print(_tile.TilemapMember.color);
                                    _tile.TilemapMember.SetColor(_tile.LocalPlace, color);
                                    tile.Add(_tile);
                                }
                            }
                        }
                        print(_tile);
                        if (tiles.TryGetValue(lastMouseCoordinate, out _tile))
                        {
                            if (tile.Contains(_tile))
                            {
                                int index = tile.FindIndex(t => t == _tile);
                                resetColorFromSelected(index + 1);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            float step = speed * Time.deltaTime;
            if (tile.Count > 0)
            {
                if (Vector2.Distance(transform.position, tile[0].WorldLocation) > 0.001f)
                {
                    print(transform.position + "----------"+tile[0].WorldLocation);
                    var temVector = tile[0].WorldLocation;
                    transform.position = Vector2.MoveTowards(transform.position, temVector, step);

                }
                else
                {
                    tile[0].TilemapMember.SetColor(tile[0].LocalPlace, new Color(1, 1, 1, 1));
                    tile.RemoveAt(0);
                }
            }
            else
            {
                moving = false;
                objectPressed = false;
                initPressed = false;
            }
        }
    }
    private void checkMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("testMOve");
            var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var worldPoint = tilemap.WorldToCell(point);
            if (tiles.TryGetValue(worldPoint, out _tile))
            {
                if (tile.Contains(_tile))
                {
                    int index = tile.FindIndex(t => t == _tile);
                    resetColorFromSelected(index + 1);
                    moving = true;
                    tile[0].TilemapMember.SetColor(tile[0].LocalPlace, new Color(1, 1, 1, 1));
                    tile.RemoveAt(0);
                }
            }

        }
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Input.ResetInputAxes();
            if (!objectPressed)
            {
                print("testmouse");

                if (initPressed == false)
                {
                    initPressed = true;
                    var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles
                    clearColor();
                    Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    lastMouseCoordinate = tilemap.WorldToCell(point);
                    var worldPoint = tilemap.WorldToCell(point);

                    if (tiles.TryGetValue(worldPoint, out _tile))
                    {
                        print("Tile " + _tile.Name + " costs: " + _tile.Cost);
                        _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
                        Color color = Color.green;
                        color.a = 0.5f;
                        print(_tile.TilemapMember.color);
                        _tile.TilemapMember.SetColor(_tile.LocalPlace, color);
                        tile.Add(_tile);
                    }
                }
                else if (initPressed)
                {
                    var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles

                    Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    lastMouseCoordinate = tilemap.WorldToCell(point);
                    var worldPoint = tilemap.WorldToCell(point);

                }
                objectPressed = true;
            }
            else
            {
                resetColorFromSelected(0);
                objectPressed = false;
                initPressed = false;
            }
        }

    }
    private void resetColorFromSelected(int index)
    {
        int length = tile.Count - 1;
        for (int i = length; i >= index; i--)
        {
            tile[i].TilemapMember.SetColor(tile[i].LocalPlace, new Color(1, 1, 1, 1));
            tile.RemoveAt(i);
        }
    }
    private void clearColor()
    {
        tile.ForEach((tile)=>{
            tile.TilemapMember.SetColor(tile.LocalPlace, new Color(1, 1, 1, 1));
        });
        tile.Clear();
    }
}
