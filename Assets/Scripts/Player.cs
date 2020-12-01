using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public int id;
    public List<Army> armies = new List<Army>();
    public List<Province> provinces = new List<Province>();
    public Color color;
    public int supply;
    public int money;
    public int province_nr;
}
