using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyQuantityHandler : MonoBehaviour
{
    public static int quantity;
   
    public void HandleChange(string value)
    {
        //if (value > Players.currentPlayer.money) than bad
    }

    public void HandleEditEnd(string value)
    {
        int.TryParse(value, out quantity);
    }
}
