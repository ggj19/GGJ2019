using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatSound : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] float interval = 1f;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(Playing());
    }

    IEnumerator Playing()
    {
        while(true)
        {
            yield return new WaitForSeconds(interval);
            AudioManager.PlaySound(clip);
        }
    }
}
