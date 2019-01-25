using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void Awake()
    {
        if(gameObject)
            Destroy(gameObject, 10);
    }
}
