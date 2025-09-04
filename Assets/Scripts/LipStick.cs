using DevDuck;
using UnityEngine;

public class LipStick : MonoBehaviour

{
    public int id = -1;
    public Rigidbody2D rb;
    CapsuleCollider2D circleCollider;
    [SerializeField] PooledObject pooledObject;

    void Start()
    {
        circleCollider = GetComponent<CapsuleCollider2D>();
        pooledObject = GetComponent<PooledObject>();
    }

    public virtual void HitLipStick()
    {
        rb.isKinematic = false;
        if (id == -1)
        {
            rb.AddForce(Vector3.up * 45, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector3.down * 45, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TargetLipstickHit t = collision.gameObject.GetComponent<TargetLipstickHit>();
        if (t && (id == -1 || id == -2))
        {
            if (id == -1)
            {
                KnifeHitMiniGame.instance.playerScore++;
            }

            if (id == -2)
            {
                KnifeHitMiniGame.instance.botScore++;
            }

            rb.velocity = Vector2.zero;
            transform.SetParent(t.transform);

            rb.bodyType = RigidbodyType2D.Static;
            Vector3 direction = KnifeHitMiniGame.instance.target.transform.position - transform.position;
            // T�nh g�c quay
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // G�n g�c quay
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            transform.localScale = new Vector3(0.714f, 0.714f, 0.714f);

            transform.position = t.transform.position + (transform.position - t.transform.position).normalized * 2.8f;
            Observer.Notify(EventAction.EVENT_HITTARGET_MINIGAMEHITLIPSTICK, null);
            this.id = 1;
        }

        LipStick l = collision.gameObject.GetComponent<LipStick>();
        if (l)
        {
            if (id == -2)
            {
                // Debug.Log("bot lose point");
            }

            DirectionWhenCollision();
            if (id != 1)
            {
                this.id = -3;
                if (circleCollider != null)
                {
                    circleCollider.isTrigger = true;
                }
            }
        }
    }

    public virtual void DirectionWhenCollision()
    {
        if (TargetLipstickHit.instance.isLeft)
        {
            if (id == -1)
            {
                rb.gravityScale = 1;

                rb.velocity = Vector2.zero; 
                rb.AddForce(Vector2.down * 15, ForceMode2D.Impulse);
                rb.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
            }
            if (id == -2)
            {
                rb.gravityScale = -1;

                rb.velocity = Vector2.zero; 

                rb.AddForce(Vector2.down * 15, ForceMode2D.Impulse);
                rb.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
            }
        }
        else
        {
            if (id == -1)
            {
                Debug.Log("????????");
                rb.velocity = Vector2.zero; 
                rb.gravityScale = 1;
                rb.AddForce(Vector2.down * 15);
                rb.AddForce(Vector2.right * 15);
            }
            if (id == -2)
            {
                rb.gravityScale = -1;
                rb.velocity = Vector2.zero; 
                rb.AddForce(Vector2.down * 15, ForceMode2D.Impulse);
                rb.AddForce(Vector2.right * 15, ForceMode2D.Impulse);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            pooledObject.ReturnToPool();
        }
    }
}