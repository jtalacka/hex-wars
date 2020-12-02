using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ArmyBuyHandler : MonoBehaviour
{
    private Province province;
    private Army army;
    private int totalPrice;
    private GameObject newArmyCoin;

    public Text moneyText;
    public Text supplyText;

    public static List<Vector3Int> freeSurroudingTiles = new List<Vector3Int>();
    public static bool provinceTilesColored = false;
    public GameObject newInfantryCoin;
    public GameObject newTankCoin;
    public GameObject newPlaneCoin;

    public void BuyArmy()
    {
        var tiles = GameTiles.instance.tiles;
        army = Object.Instantiate(ArmyInfoRenderer.armyTemplate);
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
            var center = province.center;
            WorldTile _tile;
            var surroudingCoords = DirectionCalculator.instance.getSurroundingCoordinates(center);
            surroudingCoords.ForEach((coordinate) => {
                if (tiles.TryGetValue(GameTiles.instance.tilemap[0].WorldToCell(coordinate), out _tile) && 
                (_tile.army == null)) 
                {
                    freeSurroudingTiles.Add(_tile.LocalPlace);
                }
            });
            TileColorHandler.ColorTiles(freeSurroudingTiles, Color.black);
            provinceTilesColored = true;

            if (Tutorial.tutorial && Tutorial.tutorialCount == 1)
            {
                GameObject go = GameObject.Find("Tutorial-text").gameObject;
                go.GetComponent<TMP_Text>().text = "Put your created army on one of the available spots";
                Tutorial.tutorialCount++;
            }
        }
    }

    void Update()
    {
        if (provinceTilesColored && Input.GetMouseButtonDown(0))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var current = GameTiles.instance.tilemap[0].WorldToCell(point);
            var tiles = GameTiles.instance.tiles;
            WorldTile tile;
            if (tiles.TryGetValue(current, out tile))
            {
                if (province != null)
                {
                    if (freeSurroudingTiles.Contains(tile.LocalPlace))
                    {
                        TileColorHandler.ColorTiles(freeSurroudingTiles, Players.currentPlayer.color);
                        freeSurroudingTiles.Clear();
                        provinceTilesColored = false;
                        PlaceNewArmy(tile);
                    }
                }
            }
            Input.ResetInputAxes();
        }
    }

    private void PlaceNewArmy(WorldTile tile)
    {
        Vector3 position = tile.WorldLocation;
        if (army != null)
        {
            switch (army.Type)
            {
                case "Infantry":
                    if(newInfantryCoin != null)
                    {
                        newArmyCoin = newInfantryCoin;
                    }
                    break;
                case "Tank":
                    if(newTankCoin != null)
                    {
                        newArmyCoin = newTankCoin;
                    }
                    break;
                case "Plane":
                    if(newPlaneCoin != null)
                    {
                        newArmyCoin = newPlaneCoin;
                    }
                    break;
            }
        }
        if(newArmyCoin != null)
        { 
            
            ArmyFactory.InstantiateArmy(newArmyCoin, position);
            army.positionInGrid = tile.LocalPlace;
            army.player = Players.currentPlayer;
            Players.currentPlayer.armies.Add(army);
            tile.army = army;


            //Debug.Log("m1:" + Players.currentPlayer.money);
            Players.currentPlayer.money -= totalPrice;
            //Debug.Log("tp:" + totalPrice);
            //Debug.Log("m2:"+Players.currentPlayer.money);
            Players.currentPlayer.supply -= army.supply;
            moneyText.text = "Money: " + Players.currentPlayer.money;
            supplyText.text = "Supply: " + Players.currentPlayer.supply + "/turn";
            if (ArmyPurchasePanelHandler.panelInstance != null)
            {
                ArmyPurchasePanelHandler.panelInstance.SetActive(false);
            }
        }
        if (Tutorial.tutorial&&Tutorial.tutorialCount==2)
        {
            GameObject go = GameObject.Find("Tutorial-text").gameObject;
            go.GetComponent<TMP_Text>().text = "Press on your newly created army to begin moving";
            Tutorial.tutorialCount++;
        }

    }

    

}
