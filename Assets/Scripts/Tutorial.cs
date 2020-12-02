using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static bool tutorial;
    public static int tutorialCount = 0;
    public bool tutorialSet=false;
    public void Start()
    {
        tutorial = tutorialSet;
    }
}
