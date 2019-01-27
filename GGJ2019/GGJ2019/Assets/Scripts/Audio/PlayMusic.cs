using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    private void Awake()
    {
        AudioManager.PlayMusic(clip);
    }
}
