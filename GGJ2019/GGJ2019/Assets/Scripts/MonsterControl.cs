using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControl : MonoBehaviour
{
    public float Speed;
    public GameObject Player;
    Vector3 Direction;

    // Update is called once per frame
    void Update()
    {
        Direction = Vector3.Normalize(Player.transform.position - transform.position);
        float rad = Mathf.Atan2(Direction.x, -Direction.y);
        float degree = rad * Mathf.Rad2Deg;
        transform.rotation = transform.rotation = Quaternion.Euler(0, 0, degree);
        transform.position += Direction * Speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D Col)
    {
        if (Speed != 0)
        {
            if (Col.tag == "Wall" || Col.tag == "Player")
            {
                Speed = 0f;
                StartCoroutine("DeadAlarm");
            }
        }
    }

    IEnumerator DeadAlarm()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
        StopCoroutine("DeadAlarm");
    }
}
