using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnHandler : MonoBehaviour
{
    public Text supplyText;
    private int currentPlayerIndex = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        GoToNextPlayer();
        CalculateIncome();
        CalculateSupply();
        supplyText.text = "Supply: " + Players.currentPlayer.supply + "/turn";
    }

    public void EndTurn()
    {
        Debug.Log(currentPlayerIndex);
        RestoreArmyMovement();
        GoToNextPlayer();
        CalculateIncome();
        CalculateSupply();
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
    }

    private void CalculateIncome()
    {
        //Not Implemented need provinces to do that
    }

    private void CalculateSupply()
    {
        int supply = 0;
        foreach (Army army in Players.currentPlayer.armies)
        {
            supply += army.supply;
        }
        Players.currentPlayer.supply = -supply;
        Players.currentPlayer.money += supply;
    }


}
