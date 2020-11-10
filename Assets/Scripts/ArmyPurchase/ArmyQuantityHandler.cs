using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyQuantityHandler : MonoBehaviour
{
    public static int quantity;
   
    public void HandleChange(string value)
    {
        
        //check if it is not greater than sum;
    }

    public void HandleEditEnd(string value)
    {
        int.TryParse(value, out quantity);
    }
}
