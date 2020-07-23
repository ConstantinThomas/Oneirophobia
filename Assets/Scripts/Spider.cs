using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Spider : MonoBehaviour
{
    public AimConstraint acSpider;
    private ConstraintSource pl;
    private GameObject player;
    private int spiderSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        pl.sourceTransform = player.transform;
        pl.weight = 1;
        acSpider.AddSource(pl);
        spiderSpeed = 6;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    
    void Move()
    {
        transform.Translate(new Vector3(0, spiderSpeed * Time.deltaTime, 0));
    }
    
    IEnumerator StopChasing()
    {
        spiderSpeed = 4;
        yield return new WaitForSeconds(2);
        spiderSpeed = 6;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            player.GetComponent<Player>().health -= 1;
            StartCoroutine(StopChasing());
            //spiderSpeed = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            //StartCoroutine(StopChasing());
        }
    }
}
