﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    public static List<Player> players = new List<Player>();
    public static Player currentPlayer;
    public static int losserId = -1;
    public static bool winnerFound = false;
    
}
