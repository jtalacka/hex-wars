using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionCalculator : MonoBehaviour
{
    public static DirectionCalculator instance;
    public List<Vector3> direction;

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
        setPositionCoordinates();
    }
    public List<Vector3> getSurroundingCoordinates(Vector3 current)
    {
        List<Vector3> surroundingCoordinates = new List<Vector3>();
        direction.ForEach(coordinate=>
            {
                surroundingCoordinates.Add(current + coordinate);

             }   );

        return surroundingCoordinates;

    }
    private void setPositionCoordinates()
    {
        direction = new List<Vector3>();
        direction.Add(new Vector3(-1,0));
        direction.Add(new Vector3(0,1));
        direction.Add(new Vector3(1,1));
        direction.Add(new Vector3(1,0));
        direction.Add(new Vector3(1,-1));
        direction.Add(new Vector3(0,-1));

    }


}
