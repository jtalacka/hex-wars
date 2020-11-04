using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnHandler : MonoBehaviour
{
    public Text supplyText;
    public static Player currentPlayer;
    private List<Player> players = new List<Player>();
    private int currentPlayerIndex = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        GoToNextPlayer();
        CalculateIncome();
        CalculateSupply();
        supplyText.text = "Supply: " + currentPlayer.supply + "/turn";
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
        foreach (Army army in currentPlayer.armies)
        {
            army.movementLeft = army.movement;
        }
    }

    private void GoToNextPlayer()
    {
        if (players.Count - 1 > currentPlayerIndex)
        {
            currentPlayerIndex++;
            currentPlayer = players[currentPlayerIndex];
            
        }
        else if (players.Count > 0)
        {
            currentPlayerIndex = 0;
            currentPlayer = players[currentPlayerIndex];
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
        foreach (Army army in currentPlayer.armies)
        {
            supply += army.supply;
        }
        currentPlayer.supply = -supply;
        currentPlayer.money += supply;
    }


}
