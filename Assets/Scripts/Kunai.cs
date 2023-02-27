using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float speed = 5f;

    [SerializeField] private GameObject hitVFX;
    // Start is called before the first frame update
    void Start()
    {
       OnInit();
    }

    public void OnInit()
    {
        rb.velocity = transform.right * speed;
        Invoke(nameof(OnDespawn), 4f);
    }

    public void OnDespawn()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            col.GetComponent<Character>().OnHit(30f);
            Instantiate(hitVFX, transform.position, transform.rotation);
            OnDespawn();
        }
    }
}
