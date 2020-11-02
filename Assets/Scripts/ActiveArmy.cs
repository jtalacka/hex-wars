using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveArmy : MonoBehaviour
{
    public static ActiveArmy instance;

    public Dictionary<Vector3, Army> activeArmy;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }
}
