using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Close : MonoBehaviour
{
    public GameObject close;
    public AudioClip uiButtonSound;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("joystick button 1"))
        {
            if (close.activeSelf)
            {
                AudioManager.PlaySound(uiButtonSound);
                close.SetActive(false);
            }
        }
    }
}
