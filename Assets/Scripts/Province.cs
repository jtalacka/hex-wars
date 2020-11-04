using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Province : ScriptableObject
{
   public Vector3Int center;
   public List<Vector3Int> teritories;
}
