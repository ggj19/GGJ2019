using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool inCam;
    private void OnBecameVisible() => inCam = true;
    private void OnBecameInvisible() => inCam = false;

    private Vector3 angle;
    private void Awake()
    {
        angle = Vector3.forward * Random.Range(1, 10f);

        if (gameObject)
            Destroy(gameObject, 10);
    }
    public void Update()
    {
        if (gameObject)
        {
            if (inCam)
            {
                gameObject.SetActive(true);
                transform.Rotate(angle);
            }
            else
                gameObject.SetActive(false);
        }
    }
}
