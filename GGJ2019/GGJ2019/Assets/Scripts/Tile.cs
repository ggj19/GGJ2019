using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject Map;
    public Sprite[] tile;
    public GameObject wallPrefab;

    private SpriteRenderer Sprite;
    //int HomeTile = 1;
    private bool isPlayer = false;
    //bool isWall = false;

    // Start is called before the first frame update
    void Awake()
    {
        Sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D Col)
    {
        if (Col.tag == "Player")
        {
            isPlayer = true;
            Map.gameObject.SetActive(true);
            Map.gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.sprite;
        }

        if (Sprite.sprite != tile[0])
        {
            if (Col.tag == "Wall" || Col.tag == "Food")
            {
                Sprite.sprite = tile[0];
                Map.gameObject.SetActive(true);
                Map.gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.sprite;
            }
        }
    }

    void OnTriggerStay2D(Collider2D Col)
    {
        /*if (Sprite.sprite != tile[0])
        {
            if (Col.tag == "Wall")
            {
                Sprite.sprite = tile[0];
            }
        }*/
    }

    void OnTriggerExit2D(Collider2D Col)
    {
        if (Col.tag == "Wall")
        {
            Sprite.sprite = tile[3];
            Map.gameObject.SetActive(true);
            Map.gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.sprite;
        }

        if (Col.tag == "Player")
        {
            isPlayer = false;
        }
    }
}
