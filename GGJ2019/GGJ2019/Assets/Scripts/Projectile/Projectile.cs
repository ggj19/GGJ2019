using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject Effect;

    public AudioClip throwSound, bombSound;
    //private bool inCam;
    //private void OnBecameVisible() => inCam = true;
    //private void OnBecameInvisible() => inCam = false;
    private Vector3 angle;
    bool isDead = false;

    private void Awake()
    {
        angle = Vector3.forward * Random.Range(-10f, 10f);
        if (gameObject)
        {
            //StartCoroutine(PlaySound());
            Destroy(gameObject, 10);
        }
    }
    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(1);
        AudioManager.PlaySound(throwSound);
    }
    public void FixedUpdate()
    {
        if (!isDead)
        {
            if (gameObject)
            {
                //if (inCam)
                //{
                //    gameObject.SetActive(true);
                //    transform.Rotate(angle);
                //}
                //else
                //    gameObject.SetActive(false);
                transform.Rotate(angle);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D Col)
    {
        if (Col.tag == "Wall")
        {
            AudioManager.PlaySound(bombSound);
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            isDead = true;
            var effect = Instantiate(Effect, transform.position, Quaternion.identity);
            effect.GetComponent<Animator>().SetTrigger("Fire");

            Destroy(effect, 0.333f);
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            StartCoroutine("DeadAlarm");
        }
    }

    IEnumerator DeadAlarm()
    {
        yield return new WaitForSeconds(0.333f);
        //gameObject.SetActive(false);
        Destroy(gameObject, 0f);
        StopCoroutine("DeadAlarm");
    }
}
