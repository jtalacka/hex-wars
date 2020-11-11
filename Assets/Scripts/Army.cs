using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class Army : ScriptableObject
{
    public string Type;
    public List<int> TileMaps=new List<int>();
    public Vector3 position;
    public Player player;
    public int movement;
    public int movementLeft;
    public int quantity;
    public int supply;
    public int price;
    public Sprite icon;

}
