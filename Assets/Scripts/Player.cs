using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("variables")] 
    [SerializeField] float horizontal, vertical;
    public float speed;
    private bool isBelow;
    private bool canHeal;
    private bool isDetected;
    private bool jumping;
    private float timer;
    private float fireRate;
    float jumpTime;
    public int ammo;
    public float health;
    [Header("references")] 
    private GameObject ennemy;
    public GameObject Sight;
    public LineRenderer lr;
    private Transform trSight;
    public Animator PlayerAnimator;
    private Vector3 worldPosition;
    public Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] Transform trP;
    [SerializeField] Transform ShootPoint;

    void Start()
    {
        ennemy = GameObject.FindWithTag("ennemy");
        GameObject instantiatedSight = Instantiate(Sight);
        trSight = instantiatedSight.transform;
        Physics2D.gravity = new Vector3(0, 0, -9.8f);
        health = 15;
        speed = 5;
        fireRate = 1;
        ammo = 100;
    }

    void Update()
    {
        timer += Time.deltaTime;
        Aim();
        Inputs();
        Move();
        LineRenderer();
        if (Input.GetMouseButton(0) && timer >= fireRate)
        {
            PlayerAnimator.SetBool("shoot", true);
            Shoot();
        }
        else
        {
            PlayerAnimator.SetBool("shoot", false);
        }
        if (Input.GetKeyDown(KeyCode.A) && canHeal)
        {
            StartCoroutine(Heal());
        }
        if (horizontal != 0 || vertical != 0)
        {
            PlayerAnimator.SetBool("idle", false);
        }
        else
        {
            PlayerAnimator.SetBool("idle", true);
        }
        //Jump();
    }

    void LineRenderer()
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, trSight.position);
        if (Input.GetMouseButton(0))
        {
            lr.enabled = true;
        }
    }

    void Shoot()
    {
        Debug.Log("ennemy take dmg");
        ennemy.GetComponent<IaManager>().ennemyHealth -= 1;
        if (timer > fireRate)
        {
            timer = 0;
        }
    }

    void Aim()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        float dist = Vector3.Distance(transform.position, worldPosition);
        if (dist <= 12)
        {
            trSight.position = worldPosition;
        }
    }

    void Inputs()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void Move()
    {
        trP.position += new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;
        if (horizontal > 0 || trSight.position.x > trP.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
    
    /*private void Jump()
    {
        if (!Input.GetKey("space") || jumping) return;
        speed = 3;
        StartCoroutine(JumpRoutine());
    }

    private IEnumerator JumpRoutine()
    {
        rb.gravityScale = 0;
        rb.velocity = new Vector3(0, 0, 1) * 5;
        float timer = 0f;
        while (Input.GetKey("space") && timer < jumpTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        jumping = true;
        rb.gravityScale = 1;
    }*/

    IEnumerator Heal()
    {
        health += 5;
        yield break;
    }

    float CheckCollisionOnSide(Collision2D collision)
    {
        if (collision.contacts.Length > 0)
        {
            Vector3 hit = collision.contacts[0].normal;
            var angle = Vector3.Angle(hit, Vector3.up);
            return angle;
        }
        return 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("puddle"))
        {
            canHeal = true;
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var angle = CheckCollisionOnSide(collision);
        if (collision.gameObject.CompareTag("tantacle"))
        {
            if (Mathf.Approximately(angle, 0f))
            {
                transform.position += new Vector3(0, 2, 0);
            }
            else if (Mathf.Approximately(angle, 90))
            {
                transform.position += new Vector3(2, 0, 0);
            }
            else if (Mathf.Approximately(angle, 180))
            {
                transform.position += new Vector3(0, -2, 0);
            }
            else if (Mathf.Approximately(angle, 270))
            {
                transform.position += new Vector3(-2, 0, 0);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("puddle"))
        {
            canHeal = false;
        }
    }
}
