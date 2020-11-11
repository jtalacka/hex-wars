using System.Collections;
using System.Collections.Generic;
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
    
    // Start is called before the first frame update
    void Start()
    {
        CalculateIncome();
        CalculateSupply();
        ShowPlayerInfo();
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
        Players.currentPlayer.money += supply;
    }

    private void ShowPlayerInfo()
    {
        moneyText.text = "Money: " + Players.currentPlayer.money;
        supplyText.text = "Supply: " + Players.currentPlayer.supply + "/turn";
        incomeText.text = "Income: " + income + "/turn";
        playerIdText.text = "Player nr.: " + Players.currentPlayer.id;
    }


}
