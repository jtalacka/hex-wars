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
    private int totalPrice;

    public Text moneyText;
    public Text supplyText;

    public static bool provinceTilesColored = false;
    public GameObject newArmyCoin;
    public void BuyArmy()
    {
        army = ArmyInfoRenderer.armyTemplate;
        int quantity = ArmyQuantityHandler.quantity;
        if (quantity > 1 && quantity < 50000)
        {
            totalPrice = quantity * army.price;
            army.quantity = quantity;
            //Debug.Log("Total price: " + totalPrice);
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
                            PlaceNewArmy(tile);
                        }
                    }
                }
                Input.ResetInputAxes();
            }
        }
    }

    private void PlaceNewArmy(WorldTile tile)
    {
        Vector3 position = tile.WorldLocation;
        if(newArmyCoin != null && army != null)
        { 
            
            ArmyFactory.InstantiateArmy(newArmyCoin, position);
            army.positionInGrid = tile.LocalPlace;
            army.player = Players.currentPlayer;
            Players.currentPlayer.armies.Add(army);
            tile.Army = army;
            //Debug.Log("m1:" + Players.currentPlayer.money);
            Players.currentPlayer.money -= totalPrice;
            //Debug.Log("tp:" + totalPrice);
            //Debug.Log("m2:"+Players.currentPlayer.money);
            Players.currentPlayer.supply -= army.supply;
            moneyText.text = "Money: " + Players.currentPlayer.money;
            supplyText.text = "Supply: " + Players.currentPlayer.supply + "/turn";

        }
        
    }

    

}
