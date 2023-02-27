using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSign : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<Player>().SavePoint();
        }
    }
}
