using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    [SerializeField]
    private GameObject ExploEffect;
    private Rigidbody2D rb;
    [SerializeField]
    private float _x;
    [SerializeField]
    private float _y;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(_x, _y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 10)
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        GameObject effect = GameObject.Instantiate(ExploEffect, transform.position, transform.rotation) as GameObject;
        Destroy(gameObject);
        Destroy(effect, 0.3f);
    }
}
