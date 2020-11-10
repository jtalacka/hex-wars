using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyPurchasePanelHandler : MonoBehaviour
{
    public GameObject panel;
    public Province province;
    public static Province Pprovince;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void OnMouseDown()
    {
        if (panel != null)
        {
            bool isActive = panel.activeSelf;
            panel.SetActive(!isActive);
            Component[] textComponents = panel.GetComponentsInChildren<Text>(true);
            foreach(Text text in textComponents)
            {
                if (text.name == "ProvinceInfo")
                {
                    text.text = $"Income: {province.income}/turn \nSize: {province.teritories.Count}"; 
                }
            }
            Pprovince = province;
        }
    }



}
