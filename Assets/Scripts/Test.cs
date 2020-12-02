using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    private WorldTile _tile;
    public float speed;
    public Army army;
    public bool objectPressed = false;
    private bool moving = false;
    private bool enemy = false;
    private List<WorldTile> tile = new List<WorldTile>();
    private Vector3 lastMouseCoordinate = Vector3.zero;
    private WorldTile tempTile;
    private Army enemyArmy;
    private bool merge=false;
    public AudioSource battleSound;
    // Update is called once per frame
    List<Tilemap> tilemap;
    private void Start()
    {
        tilemap = new List<Tilemap>();
        var map = GameTiles.instance.tilemap[0];
        var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles
        tilemap.Add(map);
        this.gameObject.GetComponent<SpriteRenderer>().color = Players.currentPlayer.color;


        if (tiles.TryGetValue(locationInGrid(transform.position), out _tile))
        {
            army = _tile.army;
            army.positionInGrid = _tile.LocalPlace;
        }
        print(army.positionInGrid);
        // tilemap = army.tilemap[0];
    }
    private void Update()
    {
        if (!moving)
        {

            if (objectPressed)
            {
                checkMovement();
            }


            if (objectPressed)
            {
                Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
               // print(locationInGrid(point));
                var current = tilemap[0].WorldToCell(point);
                if (current != lastMouseCoordinate)
                {
                    lastMouseCoordinate = current;
                    var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles
                    var tilemap = GameTiles.instance.tilemap[0];
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
                                if (tile.Count < army.movementLeft + 1 && !enemy)
                                {
                                    _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
                                    Color color = Color.green;
                                    if (enemyNearby(current))
                                    {
                                        color = Color.red;
                                        enemy = true;
                                    }

                                    color.a = 0.5f;
                                    //  print(_tile.TilemapMember.color);
                                    _tile.TilemapMember.SetColor(_tile.LocalPlace, color);
                                    tile.Add(_tile);
                                }
                            }
                        }
                        //print(_tile);
                        if (tiles.TryGetValue(lastMouseCoordinate, out _tile))
                        {
                            if (tile.Contains(_tile))
                            {
                                int index = tile.FindIndex(t => t == _tile);
                                if (index != tile.Count - 1)
                                {
                                    enemy = false;
                                }
                                resetColorFromSelected(index + 1, tile.Count - 1);
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
                    if (Vector2.Distance(transform.position, tile[0].WorldLocation) >= 0.001f)
                {
                  //  print(transform.position + "----------"+tile[0].WorldLocation);
                    var temVector = tile[0].WorldLocation;
                    transform.position = Vector2.MoveTowards(transform.position, temVector, step);
                    // print(tile[0].army);
                        tile[0].army = this.army;
                    
                    army.positionInGrid = tile[0].LocalPlace;

                }
                else
                {
                    Color color;
                    tempTile.army = null;
                    if ((tile[0].Province != null) && (tile[0].Province.player.id != null))
                    {
                        color = tile[0].Province.player.color;
                    }
                    else
                    {
                        color = new Color(1, 1, 1, 1);
                    }
                    tile[0].TilemapMember.SetColor(tile[0].LocalPlace, color);
                    tempTile = tile[0];
                    tile.RemoveAt(0);
                    army.movementLeft -= 1;
                    if (tile.Count == 0&&enemy)
                    {
                        print("there's an enemy");
                        this.gameObject.AddComponent<Battle>();
                        this.GetComponent<Battle>().attacker = army;
                        this.GetComponent<Battle>().defender = enemyArmy;
                        this.GetComponent<Battle>().speed = speed;
                        this.GetComponent<Battle>().audio = battleSound;
                        enemyArmy = null;
                        enemy = false;

                    }
                }
            }
            else
            {
                print("move test");
                var tiles = GameTiles.instance.tiles;
                CheckForProvinceOccupation();
                Players.currentPlayer.armies.ForEach(armies =>
                {
                    if (armies != this.army)
                    {
                        if (armies != this.army && this.army != null)
                        {
                            if (armies.positionInGrid == this.army.positionInGrid && this.army.Type == armies.Type)
                            {
                                if (tiles.TryGetValue(army.positionInGrid, out _tile))
                                {
                                    _tile.army = armies;
                                    _tile.army.quantity += army.quantity;
                                    _tile.army.movementLeft = (_tile.army.movementLeft + this.army.movementLeft) / 2;
                                    Players.currentPlayer.armies.Remove(this.army);
                                    if (Tutorial.tutorial && Tutorial.tutorialCount == 5)
                                    {
                                        GameObject go = GameObject.Find("Tutorial-text").gameObject;
                                        go.GetComponent<TMP_Text>().text = "Move your army near the enemy player to attack him. If you're out of movement or want to get more Money from your provinces press, End Turn";
                                        Tutorial.tutorialCount++;
                                    }
                                    Destroy(this.gameObject);
                                }

                            }

                        }

                    }
                });
                if (Tutorial.tutorial && Tutorial.tutorialCount == 4)
                {
                    GameObject go = GameObject.Find("Tutorial-text").gameObject;
                    go.GetComponent<TMP_Text>().text = "Create another army of the same type,place it and try to move it on top of your previously created army to join them. Their quantity will add up";
                    Tutorial.tutorialCount++;
                }
                GameObject.Find("MovementAudio").GetComponent<AudioSource>().Stop();
                moving = false;
                objectPressed = false;
                objectPressed = false;
            }
        }
    }
    private void checkMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
           // print("testMOve");
            var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var worldPoint = tilemap[0].WorldToCell(point);
            if (tiles.TryGetValue(worldPoint, out _tile))
            {
                if (tile.Contains(_tile))
                {
                    int index = tile.FindIndex(t => t == _tile);
                    resetColorFromSelected(index + 1, tile.Count - 1);
                    moving = true;
                    Color color;
                    if((tile[0].Province != null) && (tile[0].Province.player.id != null))
                    {
                        color = tile[0].Province.player.color;
                    }
                    else
                    {
                        color = new Color(1, 1, 1, 1);
                    }
                    tile[0].TilemapMember.SetColor(tile[0].LocalPlace, color);
                    tempTile=tile[0];
                    tile.RemoveAt(0);
                }
            }

        }
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)&&army.player.id==Players.currentPlayer.id)
        {
            GameObject.Find("MovementAudio").GetComponent<AudioSource>().clip = army.audio;
            if (Tutorial.tutorial&&Tutorial.tutorialCount==3)
            {
                GameObject go = GameObject.Find("Tutorial-text").gameObject;
                go.GetComponent<TMP_Text>().text = "Drag your mouse to create a path for your army to move and click left mouse button.\n On the bottom left you can see information about your current army";
                Tutorial.tutorialCount++;
            }

            bool otherArmyPressed = false;
            foreach (var go in GameObject.FindGameObjectsWithTag("army") as GameObject[])
            {
                if (go.GetComponent<Test>().objectPressed==true)
                {
                    otherArmyPressed = true;
                    break;
                }
            }
            if (otherArmyPressed == false)
            {
                print(army.player.id);
                Input.ResetInputAxes();
                if (!moving)
                {
                    if (!objectPressed)
                    {
                        // print("testmouse");

                        objectPressed = true;
                        var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles
                        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        lastMouseCoordinate = tilemap[0].WorldToCell(point);
                        var worldPoint = tilemap[0].WorldToCell(point);

                        if (tiles.TryGetValue(worldPoint, out _tile))
                        {
                            //  print("Tile " + _tile.Name + " costs: " + _tile.Cost);
                            _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
                            Color color = Color.green;
                            color.a = 0.5f;
                            // print(_tile.TilemapMember.color);
                            _tile.TilemapMember.SetColor(_tile.LocalPlace, color);
                            tile.Add(_tile);
                        }

                        objectPressed = true;
                    }
                    else
                    {
                        resetColorFromSelected(0, tile.Count - 1);
                        objectPressed = false;
                        objectPressed = false;
                    }
                }
            }
        }

    }

    private void CheckForProvinceOccupation()
    {
        var tiles = GameTiles.instance.tiles;
        var end_tile_position = this.army.positionInGrid;
        WorldTile surroundingTile;
        var coords = DirectionCalculator.instance.getSurroundingCoordinates(end_tile_position);
        foreach (var coordinate in coords)
        {
            if (tiles.TryGetValue(locationInGrid(coordinate), out surroundingTile) &&
            (surroundingTile.Province != null) && (surroundingTile.Province.center == surroundingTile.LocalPlace))
            {
                bool enemyFound = false;
                var centerSurroundingTiles = DirectionCalculator.instance.getSurroundingCoordinates(surroundingTile.Province.center);
                WorldTile centerSurroundingWorldTile = null;
                foreach (var centerSurroundingTile in centerSurroundingTiles)
                {
                    if (tiles.TryGetValue(locationInGrid(centerSurroundingTile), out centerSurroundingWorldTile) && 
                        (centerSurroundingWorldTile.army != null ) && (centerSurroundingWorldTile.army.player.id != Players.currentPlayer.id))
                           
                    {
                        enemyFound = true;
                        break;
                    }
                }
                if (!enemyFound)
                {
                    foreach(var player in Players.players)
                    {
                        if (player.provinces.Contains(surroundingTile.Province))
                        {
                            player.provinces.Remove(surroundingTile.Province);
                            break;
                        }
                    }
                    surroundingTile.Province.player = Players.currentPlayer;
                    Players.currentPlayer.provinces.Add(surroundingTile.Province);
                    TileColorHandler.ColorTiles(surroundingTile.Province.teritories, Players.currentPlayer.color);
                }
                break;
            }
        }
    }
    private void resetColorFromSelected(int begining,int end)
    {

        for (int i = end; i >= begining; i--)
        {
            Color color;
            var province = tile[i].Province;
            if((province != null) && (province.player.id != null))
            {
                color = province.player.color;
            }
            else
            {
                color = new Color(1, 1, 1, 1);
            }
            tile[i].TilemapMember.SetColor(tile[i].LocalPlace, color);
            tile.RemoveAt(i);
        }
    }

    private Vector3Int locationInGrid(Vector3 position)
    {
        return tilemap[0].WorldToCell(position);
    }


    private bool enemyNearby(Vector3 currentPosition)
    {
        bool nearby = false;
        var tiles = GameTiles.instance.tiles;
        WorldTile _tile;
        bool changes = false;
        enemyArmy = null;
        DirectionCalculator.instance.getSurroundingCoordinates(currentPosition).ForEach((coordinate) =>
        {
            if (tiles.TryGetValue(locationInGrid(coordinate), out _tile))
            {
                if (_tile.army&&_tile.army!=this.army&&_tile.army.player.id!=army.player.id)
                {
                    nearby = true;
                    print("nearby");
                    enemyArmy = _tile.army;
                    changes = true;
                }
            }

        }
        );
        return nearby;
    }

}
