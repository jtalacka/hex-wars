using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerSelection : MonoBehaviour
{
    public static List<PlayerSelectionType> players;
    public static int playerCount;
    public static int startingMoney;
    public List<String> provinces;
    public List<Color> colors;
    public GameObject player;
    // Start is called before the first frame update
    public void Start()
    {
        playerCount = 2;
        startingMoney = 1000;
        provinces = new List<string>();
        provinces.Add("Bordo");
        provinces.Add("Marseille");
        provinces.Add("province3");
        provinces.Add("province4");
        colors = new List<Color>();
        colors.Add(Color.red);
        colors.Add(Color.blue);
        colors.Add(Color.magenta);
        colors.Add(Color.yellow);
        players = new List<PlayerSelectionType>();
    }
    public void Generate()
    {
        Button button = GameObject.Find("Start").gameObject.GetComponent<Button>();
        GameObject  goDelete= GameObject.Find("Players").gameObject;
        int count = goDelete.transform.childCount;
        if (playerCount>1&&playerCount<5) {
            button.interactable = false;
            for (int i = 1; i < count; i++)
            {
                Destroy(goDelete.transform.GetChild(i).gameObject);
                players.Clear();
            }

            for (int i = 0; i < playerCount; i++)
            {
                players.Add(new PlayerSelectionType());
                GameObject go = Instantiate(player);
                go.name = i.ToString();
                go.transform.SetParent(GameObject.Find("Players").transform);
                go.transform.position = GameObject.Find("Players").transform.position;
                go.transform.localScale = new Vector3(1, 1, 1);
                go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y-200f- i*70f, go.transform.position.z);
                go.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = provinces[i];
                go.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().color = colors[i];
                go.transform.GetChild(0).gameObject.GetComponent<TMP_InputField>().text = "Player-" + i.ToString();
            }
            button.interactable = true;
        }
    }
    public void onPlayers(string m)
    {
        playerCount = int.Parse(m);
    }
    public void onMoney(string m)
    {
        startingMoney = int.Parse(m);
    }
}
