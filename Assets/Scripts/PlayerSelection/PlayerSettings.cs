using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public void setName(string m)
    {
        print(m);
        if (this.gameObject.name != "Player")
        {
            //  print(int.Parse(this.gameObject.name));
             PlayerSelection.players[int.Parse(this.gameObject.name)].name = m;
        }
    }
}
