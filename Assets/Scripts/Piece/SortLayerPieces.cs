using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortLayerPieces : MonoBehaviour
{
    private BoxCollider2D box;
    private Rigidbody2D rigi;
    private void Start()
    {
        box = gameObject.GetComponent<BoxCollider2D>();
        rigi = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tile")
        {
            SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sortingLayerName = "Pieces";
                renderer.sortingOrder = 5;
            }
            if (box != null) Destroy(box);
            if (rigi != null) Destroy(rigi);
            Destroy(this);
        }
    }
}