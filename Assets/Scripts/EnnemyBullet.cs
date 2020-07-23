using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBullet : MonoBehaviour
{
    private Vector3 ShootDir;
    private Transform Ia;
    
    private void Start()
    {
        Ia = transform.parent;
        ShootDir = Ia.GetComponent<IaManager>().AimDir;
        transform.parent = null;
    }
    
    void Update()
    {
        transform.position += ShootDir * 1.5f * Time.deltaTime;
    }
}
