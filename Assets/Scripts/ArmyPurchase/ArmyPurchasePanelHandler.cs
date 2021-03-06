﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
                if (Tutorial.tutorial&&Tutorial.tutorialCount==0)
                {
                    GameObject go = GameObject.Find("Tutorial-text").gameObject;
                    go.GetComponent<TMP_Text>().text = "Select an army type, quantity and press buy";
                    Tutorial.tutorialCount++;
                }

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
                    TileColorHandler.RecolorTiles(ArmyBuyHandler.freeSurroudingTiles);
                    ArmyBuyHandler.freeSurroudingTiles.Clear();
                    ArmyBuyHandler.provinceTilesColored = false;
                }
                
            }
        }
    }



}
