using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Sprite[] tile;

    SpriteRenderer Sprite;
    int HomeTile = 1;

    // Start is called before the first frame update
    void Awake()
    {
        Sprite = gameObject.GetComponent<SpriteRenderer>();
        if (transform.position.x > -HomeTile && transform.position.x < HomeTile && transform.position.y > -HomeTile && transform.position.y < HomeTile)
        {
            Sprite.sprite = tile[0];
        }
        else
        {
            Sprite.sprite = tile[3];
        }
    }
}
