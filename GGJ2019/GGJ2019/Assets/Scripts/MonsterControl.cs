using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControl : MonoBehaviour
{
    public float Speed;
    public GameObject Player;
    public Sprite[] sprite;
    public SpriteRenderer SpriteR;
    Vector3 Direction;
    private Animator dieAnimation;

    public Action OnDead;
    private GameObject target;

    public AudioClip deadSound;

    void Awake()
    {
        SpriteR = gameObject.GetComponent<SpriteRenderer>();
        dieAnimation = GetComponent<Animator>();
        int r = UnityEngine.Random.Range(0, sprite.Length);
        SpriteR.sprite = sprite[r];
        Speed = r;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player)
        {
            if (!target)
                TowardTarget(Player);
            else
                TowardTarget(target);
        }
    }

    private void TowardTarget(GameObject target)
    {
        if (target)
        {
            Direction = Vector3.Normalize(target.transform.position - transform.position);
            float rad = Mathf.Atan2(Direction.x, -Direction.y);
            float degree = rad * Mathf.Rad2Deg;
            transform.rotation = transform.rotation = Quaternion.Euler(0, 0, degree);
            transform.position += Direction * Speed * Time.deltaTime;
        }
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
            else if (Col.tag == "Fire")
            {
                target = Col.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D Col)
    {
        if (Col.tag == "Fire" && target == Col?.gameObject)
        {
            target = null;
        }
    }

    IEnumerator DeadAlarm()
    {
        if(dieAnimation) dieAnimation.SetTrigger("Fire");
        AudioManager.PlayMusic(deadSound);
        yield return new WaitForSeconds(0.25f);
        SpriteR.enabled = false;
        yield return new WaitForSeconds(1f);
        OnDead?.Invoke();
        Destroy(gameObject);
        StopCoroutine("DeadAlarm");
    }
}
