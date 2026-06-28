using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuitGame : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        bool escape = Input.GetKeyDown(KeyCode.Escape);

        if (escape)
        {
            Application.Quit();
        }
    }
}
