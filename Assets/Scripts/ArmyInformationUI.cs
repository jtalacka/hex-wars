using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArmyInformationUI : MonoBehaviour
{

    private Test armyMovement;
    public TextMeshProUGUI textField;
    // Start is called before the first frame update
    void Start()
    {
        armyMovement = gameObject.GetComponent<Test>();
    }

    // Update is called once per frame
    void Update()
    {
        if (armyMovement.objectPressed)
        {
            textField.text = "Type: " + armyMovement.army.Type + "\n";
            textField.text += "Quantity " + armyMovement.army.quantity + "\n";
            textField.text += "Movement " + armyMovement.army.movementLeft + "/" + armyMovement.army.movement + "\n";

        }
        else
        {
            textField.text = "";
        }
        
    }
}
