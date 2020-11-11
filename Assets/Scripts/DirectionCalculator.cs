using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionCalculator : MonoBehaviour
{
    public static DirectionCalculator instance;
    public static List<Vector3> direction;
    public Grid grid;

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
        Vector3 returnCoords=grid.CellToWorld(new Vector3Int(Mathf.FloorToInt(current.x), Mathf.FloorToInt(current.y), 0));
        List<Vector3> surroundingCoordinates = new List<Vector3>();
        direction.ForEach(coordinate=>
            {
                surroundingCoordinates.Add(coordinate+ returnCoords);

             }   );
        return surroundingCoordinates;

    }
    private static void setPositionCoordinates()
    {
        direction = new List<Vector3>();
        direction.Add(new Vector3(-0.8f,0,0));
        direction.Add(new Vector3(-0.4f,0.6f,0));
        direction.Add(new Vector3(0.4f,0.6f,0));
        direction.Add(new Vector3(0.8f, 0,0));
        direction.Add(new Vector3(0.4f, -0.6f,0));
        direction.Add(new Vector3(-0.4f, -0.6f, 0));

    }


}
