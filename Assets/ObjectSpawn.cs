using System;
using System.Collections;
using System.Collections.Generic;
using DevDuck;
using UnityEngine;

public class ObjectSpawn : IdComponent
{
    private Rigidbody2D rb;
    public int Score;
    public ParticleSystem exploreVfx;
    public bool isDropped = false;
    public Rigidbody2D RB2D
    {
        get
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }
            return rb;
        }
    }
    private CircleCollider2D circle;

    public CircleCollider2D Collider
    {
        get
        {
            if (circle == null)
            {
                circle = GetComponent<CircleCollider2D>();
            }
            return circle;
        }
        
    }
    private SpriteRenderer spriteRenderer;

    public SpriteRenderer SpriteRenderer
    {
        get
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
            return spriteRenderer;
        }
    }

    public void SetData()
    {
        switch (ID)
        {
            case 0:
               // Collider.radius = 0.5f;
               transform.localScale = new Vector3(0.5f, 0.5f, 1);
                Score = 10;
                SpriteRenderer.color = Color.white;
                break;
            case 1:
               // Collider.radius = 1f;
               transform.localScale = new Vector3(1f, 1f, 1);
                Score = 20;
                SpriteRenderer.color = new Color32(219,129,129,255);
                break;
            case 2:
               // Collider.radius = 1.2f;
                transform.localScale = new Vector3(1.2f, 1.2f, 1f);
                Score = 30;
                SpriteRenderer.color = new Color32(120,204,95,255);
                break;
            case 3:
               // Collider.radius = 1.4f;
                transform.localScale = new Vector3(1.4f, 1.4f, 1f);

                Score = 40;
                SpriteRenderer.color = new Color32(231,67,62,255);
                break;
            case 4:
              //  Collider.radius = 1.6f;
                transform.localScale = new Vector3(1.6f, 1.6f, 1f);

                Score = 60;
                SpriteRenderer.color = new Color32(62,130,231,255);
                break;
            case 5:
              //  Collider.radius = 1.6f;
                transform.localScale = new Vector3(2f, 2, 1f);
                Score = 60;
                SpriteRenderer.color = new Color32(231,62,189,255);
                break;
        }
        RB2D.isKinematic = true;
    }

    public void Hide()
    {
        Duck.PlayParticle(exploreVfx);
       // Observer.Notify(EventAction.EVENT_GET_SCORE, Score);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider != null)
        {
            Vector2 dir = (transform.position - other.transform.position).normalized;
          //  Duck.Log("Addd force here");
            RB2D.AddForce(dir*0.5f, ForceMode2D.Impulse);
        }
    }
}
