using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torchlight : MonoBehaviour
{
    [SerializeField] private GameObject sprite;

    [SerializeField] private float throwTime, fireTime;
    [SerializeField] private float distance, rotateSpeed;
    [SerializeField] private Animator fireAnimator;

    private Vector3 targetPos;
    private float speed;
    private Vector3 direction;

    public bool isUsed { get; private set; }

    private Vector2 GetDirection(float angle)
    {
        Vector2 d = Vector2.one;
        if (angle == 0) d = Vector2.down;
        else if (angle == 90) d = Vector2.right;
        else if (angle == 180) d = Vector2.up;
        else if (angle == 270) d = Vector2.left;
        return d;
    }


    private void Awake()
    {
        Throw(Vector2.zero, 90);
    }

    // 던지는 것만 호출 하면 끗
    public void Throw(Vector2 playerPos, float angle)
    {
        // 던진다.
        targetPos = playerPos + GetDirection(angle) * distance;

        speed = Vector3.Distance(transform.position, targetPos) / throwTime;
        direction = (targetPos - transform.position).normalized;
        isUsed = true;
        Invoke(nameof(LetsFire), throwTime);
    }

        // 탄다.
    private void LetsFire()
    {
        isUsed = false;
        if(fireAnimator)
            fireAnimator.SetTrigger("Fire");
        Destroy(gameObject, fireTime);
    }

    // 던져지는 중임
    private void Update()
    {
        if (isUsed)
        {
            sprite.transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}
