using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnHandler : MonoBehaviour
{
    public Text moneyText;
    public Text supplyText;
    public Text incomeText;
    public Text playerIdText;
    public GameObject lossPanel;
    public GameObject winPanel;
    public GameObject armyForTutorialEnemy;

    private int currentPlayerIndex = -1;
    private int income = 0;
    public bool tutorial = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!Tutorial.tutorial)
        {
            Players.players = CreatePlayers(PlayerSelection.players.Count, PlayerSelection.startingMoney);
        }
        else
        {
            this.GetComponent<Button>().interactable = false;
            Players.players = CreatePlayers(2, 1000);
            CreateArmyToTotorialEnemy(Players.players[1], new Vector3Int(1, 14, 0), armyForTutorialEnemy, "Infantry");
        }
        AssignProvince();
        Players.players[0].color = Color.red;
        Players.players[1].color = new Color(1, 0.69f, 0, 1);
        if (!Tutorial.tutorial)
        {
            if (PlayerSelection.players.Count == 3)
            {
                Players.players[2].color = Color.blue;
            }
            else if (PlayerSelection.players.Count == 4)
            {
                Players.players[3].color = Color.grey;
            }
        }
        Players.currentPlayer = Players.players[0];
        Debug.Log("Start Player id: " + Players.currentPlayer.id);
        ColorProvinces.ColorAllProvinces();
        GoToNextPlayer();
        CalculateIncome();
        CalculateSupply();
        ShowPlayerInfo();
    }
    public void Update()
    {
        if (Tutorial.tutorial && Tutorial.tutorialCount == 6)
            {
            this.GetComponent<Button>().interactable = true;
            Tutorial.tutorialCount++;
            }
    }

    public void EndTurn()
    {
        Debug.Log(currentPlayerIndex);
        RestoreArmyMovement();
        GoToNextPlayer();
        CheckForLoss();
        CheckForWin();
        CalculateIncome();
        CalculateSupply();
        ShowPlayerInfo();
        if(ArmyPurchasePanelHandler.panelInstance != null)
        {
            ArmyPurchasePanelHandler.panelInstance.SetActive(false);
        }
    }

    private void RestoreArmyMovement()
    {
        foreach (Army army in Players.currentPlayer.armies)
        {
            army.movementLeft = army.movement;
        }
    }

    private void GoToNextPlayer()
    {
        if (Players.players.Count - 1 > currentPlayerIndex)
        {
            currentPlayerIndex++;

            if (Tutorial.tutorial)
            {
                currentPlayerIndex = 0;
            }

            Players.currentPlayer = Players.players[currentPlayerIndex];
            
        }
        else if (Players.players.Count > 0)
        {
            currentPlayerIndex = 0;
            Players.currentPlayer = Players.players[currentPlayerIndex];
        }
        Debug.Log(currentPlayerIndex);
        Debug.Log("Army count: " + Players.currentPlayer.armies.Count);


    }

    private void CalculateIncome()
    {
        income = 0;
        Players.currentPlayer.provinces.ForEach((province) => income += province.income);
        Players.currentPlayer.money += income;
    }

    private void CalculateSupply()
    {
        int supply = 0;
        Players.currentPlayer.armies.ForEach((army) => supply += army.supply);
        Players.currentPlayer.supply = -supply;
        Players.currentPlayer.money -= supply;
    }

    private void ShowPlayerInfo()
    {
        Debug.Log("CP:" + Players.currentPlayer.id);
        moneyText.text = "Money: " + Players.currentPlayer.money;
        supplyText.text = "Supply: " + Players.currentPlayer.supply + "/turn";
        incomeText.text = "Income: " + income + "/turn";
        playerIdText.text = "Player nr.: " + Players.currentPlayer.id;
    }

    private List<Player> CreatePlayers(int count,int startingMoney)
    {
        List<Player> players = new List<Player>();
        Player player;
        for (int i = 0; i < count; i++)
        {
            player = new Player()
            {
                id = i,
                supply = 0,
                money = startingMoney,
                province_nr = 1
            };
            players.Add(player);
        }
        return players;
    }

    private void AssignProvince()
    {

        GameObject provinceObject = GameObject.Find("center-Recovered");
        var apph = provinceObject.GetComponent<ArmyPurchasePanelHandler>();
        Province province = apph.province;
        Players.players[0].provinces.Add(province);
        province.player = Players.players[0];


        GameObject provinceObject1 = GameObject.Find("center2");
        var apph1 = provinceObject1.GetComponent<ArmyPurchasePanelHandler>();
        Province province1 = apph1.province;
        Players.players[1].provinces.Add(province1);
        province1.player = Players.players[1];
        List<Province> provinces = new List<Province>();
        provinces.Add(province);
        provinces.Add(province1);

        SetAllProvinces(provinces);

        if (!Tutorial.tutorial)
        {
            if (PlayerSelection.players.Count == 3)
            {
                GameObject provinceObject2 = GameObject.Find("center3");
                var apph2 = provinceObject2.GetComponent<ArmyPurchasePanelHandler>();
                Province province2 = apph2.province;
                Players.players[2].provinces.Add(province2);
                province2.player = Players.players[2];
                provinces.Add(province2);
            }
            else if (PlayerSelection.players.Count == 4)
            {
                GameObject provinceObject3 = GameObject.Find("center4");
                var apph3 = provinceObject3.GetComponent<ArmyPurchasePanelHandler>();
                Province province3 = apph3.province;
                Players.players[3].provinces.Add(province3);
                province3.player = Players.players[3];
                provinces.Add(province3);
            }
        }
    }

    private void CheckForLoss()
    {
        if(Players.losserId > -1)
        {
            var losser = Players.players[Players.losserId];
            Players.players.Remove(losser);
            Players.losserId = -1;
        }
        
        var player = Players.currentPlayer;
        Debug.Log(player.id + " provinces count: " + player.provinces.Count + " armies count: " + player.armies.Count);
        if((player.provinces.Count == 0) && (player.armies.Count == 0))
        {
            Players.losserId = Players.currentPlayer.id;
            if(lossPanel != null)
            {
                lossPanel.SetActive(true);
            }
        }
        else
        {
            if (lossPanel != null)
            {
                lossPanel.SetActive(false);
            }
        }
    }
    private void CheckForWin()
    {

        if (Players.winnerFound)
        {
            this.gameObject.GetComponent<SwitchScene>().switchScene();
            // change scene into menu
        }
        if(Players.players.Count == 1)
        {
            if(winPanel != null)
            {
                winPanel.SetActive(true);
            }
            Players.winnerFound = true;
        }
        if (Tutorial.tutorial)
        {
            var player = Players.players[1];
            Debug.Log(player.id + " provinces count: " + player.provinces.Count + " armies count: " + player.armies.Count);
            if ((player.provinces.Count == 0) && (player.armies.Count == 0))
            {
                if (winPanel != null)
                {
                    winPanel.SetActive(true);
                }
                Players.winnerFound = true;

            }
        }
    }

    private void SetAllProvinces(List<Province> provinces)
    {
        var tiles = GameTiles.instance.tiles;
        WorldTile _tile;
        foreach (var province in provinces)
        {
            foreach(var territory in province.teritories)
            {
                tiles.TryGetValue(territory, out _tile);
                _tile.Province = province;
            }
        }
        if (!Tutorial.tutorial)
        {
            Player player = new Player();
            player.color = new Color(1, 1, 1, 1);

            for (int i = 3; i <= 4; i++)
            {
                GameObject provinceObject = GameObject.Find("center" + i);
                var apph1 = provinceObject.GetComponent<ArmyPurchasePanelHandler>();
                Province province = apph1.province;
                province.player = player;
                foreach (var territory in province.teritories)
                {
                    tiles.TryGetValue(territory, out _tile);
                    _tile.Province = province;
                }
            }
        }
    }

    private void CreateArmyToTotorialEnemy(Player player, Vector3Int tilePosition, GameObject newArmyCoin, string type)
    {
        var tiles = GameTiles.instance.tiles;
        WorldTile _tile;
        if (tiles.TryGetValue(tilePosition, out _tile))
        {
            var position = _tile.WorldLocation;
            var army = ArmyFactory.GetArmyTemplate(type);
            if (newArmyCoin != null)
            {

                ArmyFactory.InstantiateArmy(newArmyCoin, position);
                army.positionInGrid = _tile.LocalPlace;
                army.player = player;
                army.quantity = 10;
                player.armies.Add(army);
                _tile.army = army;

                foreach (var arm in GameObject.FindGameObjectsWithTag("army") as GameObject[])
                {
                    if (arm.GetComponent<Test>().army == army)
                    {
                        print("AAAA");
                        arm.GetComponent<Test>().tutorialEnemyColor();
                        break;
                    }
                }

            }
        }
    }


}
