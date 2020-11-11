using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ArmyBuyHandler : MonoBehaviour
{
    private Province province;
    private Army army;
    private Vector3 lastMouseCoordinate = Vector3.zero;

    public static bool provinceTilesColored = false;
    public GameObject newArmyCoin;
    public void BuyArmy()
    {
        army = ArmyInfoRenderer.armyTemplate;
        int quantity = ArmyQuantityHandler.quantity;
        if (quantity > 1 && quantity < 50000)
        {
            int totalPrice = quantity * army.price;
            army.quantity = quantity;
            Debug.Log(totalPrice);
        }
        province = ArmyPurchasePanelHandler.instance;
        if(province != null)
        {
            TileColorHandler.ColorTiles(province.teritories, Color.red);
            provinceTilesColored = true;
        }
    }

    void Update()
    {
        if (provinceTilesColored && Input.GetMouseButtonDown(0))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var current = GameTiles.instance.tilemap[0].WorldToCell(point);
            if (current != lastMouseCoordinate)
            {
                lastMouseCoordinate = current;
                var tiles = GameTiles.instance.tiles;
                WorldTile tile;
                if (tiles.TryGetValue(current, out tile))
                {
                    if (province != null)
                    {
                        if (province.teritories.Contains(tile.LocalPlace))
                        {
                            TileColorHandler.RecolorTiles(province.teritories);
                            provinceTilesColored = false;
                            PlaceNewArmy(tile.WorldLocation);
                        }
                    }
                }
                Input.ResetInputAxes();
            }
        }
    }

    private void PlaceNewArmy(Vector3 position)
    {
        if(newArmyCoin != null)
        { 
            ArmyFactory.InstantiateArmy(newArmyCoin, position);
            if(army != null)
            {
                var worldTiles = GameTiles.instance.tiles;
                WorldTile tile;
                if (worldTiles.TryGetValue(position, out tile))
                {
                    tile.Army = army;
                } 
            }
        }
        
    }

    

}
