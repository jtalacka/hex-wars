using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public string scene;
    // Start is called before the first frame update
    public void switchScene()
    {
        if (scene=="quit")
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }

    }
}
