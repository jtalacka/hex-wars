using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    public Army attacker;
    public Army defender;
    public float speed;
    public float time = 5f;
    private float timeRemaining;
    private GameObject panel;
    private Text attacker_player;
    private Text defender_player;
    private Text attacker_type;
    private Text defender_type;
    private Text attacker_quantity;
    private Text defender_quantity;
    private Text timeleft;
    private Army retreat;
    public WorldTile retreatTile;
    public WorldTile initialTile;
    public AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = time;
        InvokeRepeating("attackAction", 1.0f, 1.0f);
        panel = GameObject.Find("Canvas").transform.Find("Panel").gameObject;
        panel.SetActive(true);
        attacker_player = panel.transform.Find("Player_Name").GetComponent<Text>();
        defender_player = panel.transform.Find("Enemy_Name").GetComponent<Text>();
        attacker_type = panel.transform.Find("Attacker_Army").GetComponent<Text>();
        defender_type = panel.transform.Find("Defender_Army").GetComponent<Text>();
        attacker_quantity = panel.transform.Find("Attacker_Quantity").GetComponent<Text>();
        defender_quantity = panel.transform.Find("Defender_Quantity").GetComponent<Text>();
        timeleft = panel.transform.Find("TimeLeft").GetComponent<Text>();


        attacker_type.text = attacker.Type;
        defender_type.text = defender.Type;
        attacker_quantity.text = attacker.quantity.ToString();
        defender_quantity.text = defender.quantity.ToString();
        timeleft.text = timeRemaining.ToString();
        audio.loop=true;
        audio.Play();
        GameObject.Find("EndTurnBtn").GetComponent<Button>().interactable = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (attacker.quantity <= 0)
        {
            CancelInvoke();
            Players.currentPlayer.armies.Remove(attacker);
            Destroy(attacker);
            panel.SetActive(false);
            audio.Stop();
            GameObject.Find("EndTurnBtn").GetComponent<Button>().interactable = true;
            Destroy(this.GetComponent("Battle"));
            Destroy(this.gameObject);

        }
        if (defender.quantity <= 0)
        {
            CancelInvoke();
            foreach (var go in GameObject.FindGameObjectsWithTag("army") as GameObject[])
            {
                if (go.GetComponent<Test>().army == defender)
                {
                    Destroy(go.gameObject);
                    break;
                }
            }
            audio.Stop();
            GameObject.Find("EndTurnBtn").GetComponent<Button>().interactable = true;
            if (Tutorial.tutorial)
            {
                GameObject go = GameObject.Find("Tutorial-text").gameObject;
                go.GetComponent<TMP_Text>().text = "Occupy enemy province to increase your suply, for this tutorial, to win the game";
                Tutorial.tutorialCount++;
            }
            Destroy(this.GetComponent("Battle"));
            defender.player.armies.Remove(defender);
            Destroy(defender);

            panel.SetActive(false);
        }

        //print(retreatTile);
        if (retreat != null && retreatTile != null)

        {
            Transform trans = null;
            foreach (var go in GameObject.FindGameObjectsWithTag("army") as GameObject[])
            {
                if (go.GetComponent<Test>().army == retreat)
                {
                    trans = go.GetComponent<Test>().transform;
                    break;
                }
            }

            if (Vector2.Distance(trans.transform.position, retreatTile.WorldLocation) >= 0.001f)
            {


                float step = speed * Time.deltaTime;
                var temVector = retreatTile.WorldLocation;
                trans.transform.position = Vector2.MoveTowards(trans.transform.position, temVector, step);
                // print(tile[0].army);

            }
            else
            {
                retreat.positionInGrid = retreatTile.LocalPlace;
                initialTile.army = null;
                retreatTile.army = retreat;
                panel.SetActive(false);
                audio.Stop();
                GameObject.Find("EndTurnBtn").GetComponent<Button>().interactable = true;
                Destroy(this.GetComponent("Battle"));
            }

        }
        else if(retreat!=null)
        {
            print("helpo");
            if (retreat == attacker)
                {
                CancelInvoke();
                Destroy(this.GetComponent("Battle"));
                    Destroy(this.gameObject);

                }
                else if(retreat==defender)
                {
                CancelInvoke();
                foreach (var go in GameObject.FindGameObjectsWithTag("army") as GameObject[])
                {
                    if (go.GetComponent<Test>().army == defender)
                    {
                        Destroy(go.gameObject);
                    }
                }
                Destroy(this.GetComponent("Battle"));
                }
        }

    }
    private void attackAction()
    {
        if (retreat == null&&(attacker!=null&&defender!=null))
        {
            if (timeRemaining > 0)
            {
                timeRemaining--;
                int ad = damageCalculator(attacker.quantity);
                int dd = damageCalculator(defender.quantity);
                print(ad + " " + dd);
                defender.quantity -= ad;
                attacker.quantity -= dd;
                attacker_quantity.text = attacker.quantity.ToString();
                defender_quantity.text = defender.quantity.ToString();
                timeleft.text = timeRemaining.ToString();

                if (attacker.quantity < defender.quantity / 2)
                {
                //    retreat = attacker;
                //    enemyNearby(retreat.positionInGrid);
                }
                else if (attacker.quantity/2 > defender.quantity)
                {
                 //   retreat = defender;
                 //   enemyNearby(retreat.positionInGrid);

                }
            }
            else
            {
                if (attacker.quantity <= defender.quantity)
                {
                    retreat = attacker;
                }
                else if (attacker.quantity > defender.quantity)
                {
                    retreat = defender;
                }
                enemyNearby(retreat.positionInGrid);
            }
        }
    }
    private int damageCalculator(int quantity)
    {
        return Random.Range(0,(int)(quantity/3+1.75));

    }

    private Vector3Int locationInGrid(Vector3 position)
    {
        var tilemap = GameTiles.instance.tilemap;
        return tilemap[0].WorldToCell(position);
    }

    private void enemyNearby(Vector3 currentPosition)
    {
        print("retreating");
        var tiles = GameTiles.instance.tiles;
        WorldTile _tile;
        if (tiles.TryGetValue(retreat.positionInGrid, out _tile))
        {
            initialTile = _tile;
        }

            WorldTile escapePath=null;
        DirectionCalculator.instance.getSurroundingCoordinates(currentPosition).ForEach((coordinate) =>
        {
            if (tiles.TryGetValue(locationInGrid(coordinate), out _tile))
            {
                // print(coordinate);
                if (!_tile.army)
                {
                    if (!enemyNearbyCheck(locationInGrid(coordinate)))
                    {
                        escapePath = _tile;
                   }
                }
            }

        }
        );
        if (escapePath == null)
        {
            retreat = null;
        }
        else
        {
            retreatTile = escapePath;
        }
    }

    private bool enemyNearbyCheck(Vector3 currentPosition)
    {
        bool nearby = false;
        var tiles = GameTiles.instance.tiles;
        WorldTile _tile;
        DirectionCalculator.instance.getSurroundingCoordinates(currentPosition).ForEach((coordinate) =>
        {
            if (tiles.TryGetValue(locationInGrid(coordinate), out _tile))
            {
                // print(coordinate);

                if (_tile.army&&_tile.army!=this.retreat)
                {
                    nearby = true;
                }
            }

        }
        );
        return nearby;
    }
}
