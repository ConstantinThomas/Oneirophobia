using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class IaManager : MonoBehaviour
{
    [Header("references")]
    public GameObject EnemyBullet;
    private GameObject player;
    public Vector2 AimDir;
    public Transform trP;
    public AimConstraint Ac;
    private ConstraintSource pl;
    [Header("variables")]
    public State state;
    public int weight;
    private float damageTime;
    private float ShootTimer;
    private float slowTime;
    public float slowTimer;
    public float DamageOnTime;
    private bool isDetected;
    private bool isTrap;
    
    public enum State
    {
        FixeRanged,
        Hornet,
        SlowingTrap,
        DamagingTrap,
        TrapSpawner
    }
    
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        slowTime = 3;
    }

    void Update()
    {
        SelectIa(state);
    }

    void SelectIa(State ia)
    {
        state = ia;
        switch (state)
        {
            case State.FixeRanged:
                FixeRanged();
                weight = 1;
                break;
            case State.Hornet:
                
                break;
            case State.SlowingTrap:
                isTrap = true;
                SlowingTrap();
                weight = 1;
                break;
            case State.DamagingTrap:
                DamagingTrap();
                weight = 1;
                break;
            case State.TrapSpawner:
                
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void FixeRanged()
    {
        ShootTimer += Time.deltaTime;
        if (ShootTimer >= 2)
        {
            ShootTimer = 0;
            /*if (transform.rotation.z < 45 && transform.rotation.z > -45 && transform.rotation.z < 0)
            {
                AimDir = new Vector2(0, 1);
                ShootTimer = 0;
            }
            else if (transform.rotation.z < -45 && transform.rotation.z > -135)
            {
                AimDir = new Vector2(1, 0);
                ShootTimer = 0;
            }
            else if (transform.rotation.z > 135 && transform.rotation.z < -135)
            {
                AimDir = new Vector2(0, -1);
                ShootTimer = 0;
            }
            else if(transform.rotation.z > 45 && transform.rotation.z < 135)
            {
                AimDir = new Vector2(-1, 0);
                ShootTimer = 0;
            }*/
            GameObject bullet = Instantiate(EnemyBullet, transform.position, Quaternion.identity);
            bullet.transform.parent = transform;
        }
    }

    void SlowingTrap()
    {
        if (isDetected)
        {
            slowTimer += Time.deltaTime;
            player.GetComponent<Player>().speed = 2;
            if (slowTime < slowTimer)
            {
                isDetected = false;
                player.GetComponent<Player>().speed = 5;
                Destroy(gameObject);
            }
        }
    }

    void DamagingTrap()
    {
        if (isDetected)
        {
            DamageOnTime = 1;
            StartCoroutine(Damage());
            isDetected = false;
        }
    }

    IEnumerator Damage()
    {
        for (int i = 0; i < DamageOnTime; i++)
        {
            player.GetComponent<Player>().health -= 0.1f;
            yield return new WaitForSeconds(1);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player") && state != State.FixeRanged)
        {
            isDetected = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}
