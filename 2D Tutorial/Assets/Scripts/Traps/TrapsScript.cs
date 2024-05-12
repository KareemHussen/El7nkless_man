using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            // fall the ground of this trap down
            Rigidbody2D rb = transform.parent.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.down * 15, ForceMode2D.Impulse);
        }
    }
}
