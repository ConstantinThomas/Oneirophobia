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
    public int ammo;
    public float health;
    [Header("references")] 
    public Animator PlayerAnimator;
    private Vector3 worldPosition;
    public Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] Transform trSight;
    [SerializeField] Transform trP;
    [SerializeField] Transform ShootPoint;

    void Start()
    {
        Physics2D.gravity = new Vector3(0, 0, -9.8f);
        health = 15;
        speed = 5;
        fireRate = 1;
        ammo = 100;
    }

    void Update()
    {
        timer += Time.deltaTime;
        Sight();
        Inputs();
        Move();
        if (Input.GetMouseButton(0) && timer >= fireRate)
        {
            PlayerAnimator.SetBool("shoot", true);
            if (ammo > 0)
            {
                Shoot();
            }
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

    void Sight()
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
        if (!jumpButtonPressed || jumping) return;
        speed = 3;
        StartCoroutine(JumpRoutine());
    }

    private IEnumerator JumpRoutine()
    {
        rb.gravityScale = 0;
        rb.velocity = new Vector3(0, 0, 1) * 5;
        float timer = 0f;
        while (jumpButtonPressed && timer < jumpTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        jumping = true;
        walled = false;
        grounded = false;
        rb.gravityScale = 1;
    }*/

    IEnumerator Heal()
    {
        health += 5;
        yield break;
    }

    IEnumerator BounceBack()
    {
        //if ()
        yield break;
    }

    void Shoot()
    {
        
        //envoyer les degats au script de l'ennemi
        if (timer > fireRate)
        {
            timer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("puddle"))
        {
            canHeal = true;
            Destroy(other);
        }
        if (other.CompareTag("tantacle"))
        {
            StartCoroutine(BounceBack());
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
