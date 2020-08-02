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
    private AimConstraint ac;
    private ConstraintSource pl;
    public GameObject trAim;
    private GameObject trAimClone;
    [SerializeField] private SpriteRenderer sr;
    [Header("variables")] 
    public int ennemyHealth;
    public State state;
    public int weight;
    public int dmg;
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
        Stats();
        if (state == State.SandWorm || state == State.ArmoredSandWorm)
        { 
            trAimClone = Instantiate(trAim, transform.position, Quaternion.identity);
            ac = trAimClone.GetComponent<AimConstraint>();
            ac.AddSource(pl);
        }
        else
        {
            ac = gameObject.GetComponent<AimConstraint>();
            ac.AddSource(pl);
        }
        player = GameObject.FindWithTag("Player");
        pl.sourceTransform = player.transform;
        pl.weight = 1;
        ac.AddSource(pl);
        slowTime = 3;
    }

    void Stats()
    {
        switch (state)
        {
            case State.ArmoredSandWorm:
                dmg = 1;
                ennemyHealth = 1000000;
                break;
            case State.SpiderWeb:
                ennemyHealth = 1000000;
                break;
            case State.MaturingSpiderEgg:
            case State.SpiderEgg:
                ennemyHealth = 1;
                break;
            case State.SandWorm:
                ennemyHealth = 1;
                dmg = 2;
                break;
            case State.Hornet:
                ennemyHealth = 4;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void Update()
    {
        SelectIa(state);
        if (trAimClone.transform.rotation.eulerAngles.z > 90 && trAimClone.transform.rotation.eulerAngles.z < 270 && state == State.SandWorm || state == State.ArmoredSandWorm)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
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
            if(trAimClone.transform.rotation.eulerAngles.z > 45 && trAimClone.transform.rotation.eulerAngles.z < 135)
            {
                AimDir = new Vector2(0, 1);
            }
            else if (trAimClone.transform.rotation.eulerAngles.z > 225 && trAimClone.transform.rotation.eulerAngles.z < 315)
            {
                AimDir = new Vector2(0, -1);
            }
            else if (trAimClone.transform.rotation.eulerAngles.z > 135 && trAimClone.transform.rotation.eulerAngles.z < 225)
            {
                AimDir = new Vector2(-1, 0);
            }
            else
            {
                AimDir = new Vector2(1, 0);
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
        transform.Translate(new Vector3(2 * Time.deltaTime, 0, 0));
        if (transform.rotation.eulerAngles.z > 90)
        {
            sr.flipY = true;
        }
        else
        {
            sr.flipY = false;
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
