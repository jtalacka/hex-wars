using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyFactory : MonoBehaviour
{
    public static Army GetArmyTemplate(string type)
    {
        Army army = null;
        switch (type)
        { 
            case "Tank":
                army = ScriptableObject.CreateInstance<Army>();
                army.movement = 7;
                army.movementLeft = 7;
                army.price = 30;
                army.supply = 5;
                break;
            case "Plane":
                army = ScriptableObject.CreateInstance<Army>();
                army.movement = 12;
                army.movementLeft = 12;
                army.price = 40;
                army.supply = 7;
                break;
            case "Ship":
                army = ScriptableObject.CreateInstance<Army>();
                army.movement = 10;
                army.movementLeft = 10;
                army.price = 100;
                army.supply = 10;
                break;
            case "Infantry":
            default:
                army = ScriptableObject.CreateInstance<Army>();
                army.movement = 4;
                army.movementLeft = 4;
                army.price = 1;
                army.supply = 1;
                break;
        }
        return army;
    }
    public static Army CreateArmy(string type, Player player, int quantity)
    {
        Army army = GetArmyTemplate(type);
        army.player = player;
        army.quantity = quantity;
        return army;       
    }

    public static string ConvertArmyTypeFromIntToString(int type)
    {
        switch (type)
        {
            case 0:
                return "Infantry";
            case 1:
                return "Tank";
            case 2:
                return "Plane";
            case 3:
                return "Ship";
            default:
                return "Infantry";
        }
    }
}
