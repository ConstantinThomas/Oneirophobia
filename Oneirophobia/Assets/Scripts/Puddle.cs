using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    private int charge;
    public State size;
    [SerializeField] private BoxCollider2D col;
    [SerializeField]private SpriteRenderer srPuddle;
    private Sprite currentSprite;
    [SerializeField] private Sprite large;
    [SerializeField]private Sprite medium;
    [SerializeField]private Sprite small;
    
    public enum State
    {
        Large,
        Medium,
        Small
    }

    private void Start()
    {
        switch (size)
        {
            case State.Large:
                currentSprite = large;
                col.size = new Vector2(9, 4.2f);
                break;
            case State.Medium:
                currentSprite = medium;
                col.size = new Vector2(7, 2.5f);
                break;
            case State.Small:
                currentSprite = small;
                col.size = new Vector2(3, 1.8f);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        srPuddle.sprite = currentSprite;
    }

    public IEnumerator Sprite()
    {
        Debug.Log("called");
        if (currentSprite == large)
        {
            currentSprite = medium;
            srPuddle.sprite = currentSprite;
        }
        else if (currentSprite == medium)
        {
            currentSprite = small;
            srPuddle.sprite = currentSprite;
        }
        else if (currentSprite == small)
        {
            Destroy(gameObject);
        }
        yield break;
    }
}
