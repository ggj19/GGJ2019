using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject Projectile;
    // 생성 범위 (정사각형 모양)
    private int maxRange;

    private Vector2 GetRandomPos(Vector2 PlayerPos)
    {

    }

    public void Launch(Vector2 PlayerPos)
    {
        //Instantiate(Projectile, GetRandomPos(PlayerPos), Quatertion.Identity());
    }
}
