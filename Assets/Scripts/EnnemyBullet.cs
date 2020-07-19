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
        Ia = gameObject.transform.parent;
        FindPlayerDir();
        Debug.Log(ShootDir);
        transform.parent = null;
    }

    void FindPlayerDir()
    {
        if (Math.Abs(Ia.rotation.z - 90) < 45)
        {
            ShootDir = new Vector2(-1, 0);
        }
        if (Ia.rotation.z < -45 && Ia.rotation.z > -135)
        {
            ShootDir = new Vector2(1, 0);
        }
        if (Ia.rotation.z > 135 && Ia.rotation.z < -135)
        {
            ShootDir = new Vector2(0, -1);
        }
        if(Ia.rotation.z > 45 && Ia.rotation.z < 135)
        {
            ShootDir = new Vector2(-1, 0);
        }
    }

    void Update()
    {
        transform.position += ShootDir * 1.5f * Time.deltaTime;
    }
}
