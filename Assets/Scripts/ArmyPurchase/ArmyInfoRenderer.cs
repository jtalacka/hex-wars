using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyInfoRenderer : MonoBehaviour
{
    public Text armyInfoText;
    public static Army armyTemplate;

    void Start()
    {
        ChooseArmyType(0);
    }
    public void ChooseArmyType(int val)
    { 
        string type = ArmyFactory.ConvertArmyTypeFromIntToString(val);
        armyTemplate = ArmyFactory.GetArmyTemplate(type);
        if ((armyInfoText != null))
        {
            armyInfoText.text = $"{armyTemplate.Type} \nPrice: {armyTemplate.price}\nMovement: " +
                $"{armyTemplate.movement}\nSupply: {armyTemplate.supply}";
        }
    }
}
