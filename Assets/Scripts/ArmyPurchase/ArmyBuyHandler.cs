using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyBuyHandler : MonoBehaviour
{
    public void BuyArmy()
    {
        Army armyTemplate = ArmyInfoRenderer.armyTemplate;
        int quantity = ArmyQuantityHandler.quantity;
        Debug.Log(quantity);
        if (quantity > 1 && quantity < 50000)
        {
            int totalPrice = quantity * armyTemplate.price;
            Debug.Log(totalPrice);
        }
        
    }
   
}
