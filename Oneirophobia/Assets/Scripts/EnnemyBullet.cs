using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBullet : MonoBehaviour
{
    private int dmgToPlayer;
    private Vector3 ShootDir;
    private Transform Ia;
    
    private void Start()
    {
        Ia = transform.parent;
        dmgToPlayer = Ia.GetComponent<IaManager>().dmg;
        ShootDir = Ia.GetComponent<IaManager>().AimDir;
        transform.parent = null;
    }
    
    void Update()
    {
        transform.position += ShootDir * 1.5f * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().health -= dmgToPlayer;
            Destroy(gameObject);
        }
    }
}
