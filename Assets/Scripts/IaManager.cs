using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Random = UnityEngine.Random;

public class IaManager : MonoBehaviour
{
    [Header("references")]
    public GameObject EnemyBullet;
    public GameObject Spider;
    private GameObject player;
    public Vector2 AimDir;
    public AimConstraint ac;
    private ConstraintSource pl;
    [Header("variables")] 
    public int ennemyHealth;
    public State state;
    public int weight;
    private float damageTime;
    private float ShootTimer;
    private float slowTime;
    public float slowTimer;
    public float DamageOnTime;
    private bool isDetected;

    public enum State
    {
        SandWorm,
        ArmoredSandWorm,
        Hornet,
        SpiderWeb,
        SpiderEgg,
        MaturingSpiderEgg
    }
    
    void Start()
    {
        Health();
        player = GameObject.FindWithTag("Player");
        pl.sourceTransform = player.transform;
        pl.weight = 1;
        ac.AddSource(pl);
        slowTime = 3;
    }

    void Health()
    {
        if (state == State.ArmoredSandWorm || state == State.SpiderWeb)
        {
            ennemyHealth = 1000000;
        }
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
            case State.SandWorm:
                ennemyHealth = 1;
                FixeRanged();
                weight = 1;
                break;
            case State.ArmoredSandWorm:
                FixeRanged();
                weight = 2;
                break;
            case State.Hornet:
                ennemyHealth = 4;
                Hornet();
                break;
            case State.SpiderWeb:
                ennemyHealth = 1000000;
                SlowingTrap();
                weight = 1;
                break;
            case State.SpiderEgg:
                ennemyHealth = 1;
                DamagingTrap();
                weight = 1;
                break;
            case State.MaturingSpiderEgg:
                ennemyHealth = 1;
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
            if(transform.rotation.eulerAngles.z > 45 && transform.rotation.eulerAngles.z < 135)
            {
                AimDir = new Vector2(0, 1);
            }
            else if (transform.rotation.eulerAngles.z > 315 && transform.rotation.eulerAngles.z > 45)
            {
                AimDir = new Vector2(1, 0);
            }
            else if (transform.rotation.eulerAngles.z > 225 && transform.rotation.eulerAngles.z < 315)
            {
                AimDir = new Vector2(0, -1);
            }
            else if (transform.rotation.eulerAngles.z > 135 && transform.rotation.eulerAngles.z < 225)
            {
                AimDir = new Vector2(-1, 0);
            }
            ShootTimer = 0;
            GameObject bullet = Instantiate(EnemyBullet, transform.position, Quaternion.identity);
            bullet.transform.parent = transform;
        }
    }

    void TrapSpawner()
    {
        int spiderAmount = Random.Range(3, 6);
        for (int i = 0; i < spiderAmount; i++)
        {
            if (spiderAmount < 3)
            {
                Instantiate(Spider, new Vector3(i, 0, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(Spider, new Vector3(1, i, 0), Quaternion.identity);
            }
        }
    }

    void Hornet()
    {
        transform.Translate(new Vector3(0, 2 * Time.deltaTime, 0));
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
        if (other.collider.CompareTag("Player") && (state == State.SpiderEgg || state == State.SpiderWeb))
        {
            isDetected = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = null;
        }
        if (other.collider.CompareTag("Player") && state == State.Hornet)
        {
            player.GetComponent<Player>().health -= 4;
        }
        if (other.collider.CompareTag("Player") && state == State.MaturingSpiderEgg)
        {
            TrapSpawner();
            Destroy(gameObject);
        }
    }
}
