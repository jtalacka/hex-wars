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
                army.audio= Resources.Load("TankAudio") as AudioClip;
                break;
            case "Plane":
                army = ScriptableObject.CreateInstance<Army>();
                army.movement = 12;
                army.movementLeft = 12;
                army.price = 40;
                army.supply = 7;
                army.audio = Resources.Load("PlaneAudio") as AudioClip;
                break;
            case "Ship":
                army = ScriptableObject.CreateInstance<Army>();
                army.movement = 10;
                army.movementLeft = 10;
                army.price = 100;
                army.supply = 10;
                army.audio = Resources.Load("ShipAudio") as AudioClip;
                break;
            case "Infantry":
            default:
                army = ScriptableObject.CreateInstance<Army>();
                army.movement = 40;
                army.movementLeft = 40;
                army.price = 1;
                army.supply = 1;
                army.audio = Resources.Load("InfantryAudio") as AudioClip;
                break;
        }
        army.Type = type;
        return army;
    }
    public static void InstantiateArmy(GameObject defaultArmy, Vector3 position)
    {
        Instantiate(defaultArmy, position, Quaternion.identity);    
    }

    private static void CopyFromTemplate(Army defaultArmy, Army armyTemplate)
    {
        defaultArmy.movement = armyTemplate.movement;
        defaultArmy.movementLeft = armyTemplate.movementLeft;
        defaultArmy.price = armyTemplate.price;
        defaultArmy.supply = armyTemplate.supply;
        defaultArmy.Type = armyTemplate.Type;
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
