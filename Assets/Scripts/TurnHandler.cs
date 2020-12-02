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
        }
        AssignProvince();
        Players.players[0].color = Color.red;
        Players.players[1].color = Color.yellow;
        Players.currentPlayer = Players.players[0];
        Debug.Log("Start Player id: " + Players.currentPlayer.id);
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
        province.player = Players.players[1];
    }


}
