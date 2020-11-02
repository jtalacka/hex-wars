using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Player : ScriptableObject
{
    public int id;
    public List<Army> armies;
    public int money_amount;
    public int province_nr;
}
