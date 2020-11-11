using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyPurchasePanelHandler : MonoBehaviour
{
    public GameObject panel;
    public Province province;
    public static Province instance;
    
    
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
            instance = province;
            if (ArmyBuyHandler.provinceTilesColored)
            {
                TileColorHandler.RecolorTiles(province.teritories);
                ArmyBuyHandler.provinceTilesColored = false;
            }
        }
    }



}
