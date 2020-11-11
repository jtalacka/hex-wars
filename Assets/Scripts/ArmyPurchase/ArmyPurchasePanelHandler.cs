using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyPurchasePanelHandler : MonoBehaviour
{
    public GameObject panel;
    public Province province;
    public static Province instance;
    public static GameObject panelInstance;
    
    
    void OnMouseDown()
    {
        if (Players.currentPlayer.provinces.Contains(province))
        {
            if (panel != null)
            {
                panelInstance = panel;
                bool isActive = panel.activeSelf;
                panel.SetActive(!isActive);
                if (!isActive)
                {
                    Component[] textComponents = panel.GetComponentsInChildren<Text>(true);
                    foreach (Text text in textComponents)
                    {
                        if (text.name == "ProvinceInfo")
                        {
                            text.text = $"Income: {province.income}/turn \nSize: {province.teritories.Count}";
                        }
                    }
                    instance = province;
                }
                if (ArmyBuyHandler.provinceTilesColored)
                {
                    TileColorHandler.RecolorTiles(province.teritories);
                    ArmyBuyHandler.provinceTilesColored = false;
                }
                
            }
        }
    }



}
