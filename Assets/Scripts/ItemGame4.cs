using DG.Tweening;
using UnityEngine;

public enum TypeItem
{
    SHRIMP,SALMON,BENTO
}
namespace DevDuck
{
    public class ItemGame4 : MonoBehaviour
    {
        public TypeItem TYPE;

        public Vector2 des;
        Rigidbody2D rb;
        CircleCollider2D circleCollider;
        public bool isItem;
        public int id;
        bool isOnDes;

        private void OnEnable()
        {
            isOnDes = false;
        }
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            circleCollider = GetComponent<CircleCollider2D>();
        }
        private void FixedUpdate()
        {
            if (!isItem) return;
            transform.position = Vector2.MoveTowards(transform.position, des, 3 * Time.deltaTime);
            if (Vector2.Distance(rb.position, des) < 0.1f && !isOnDes)
            {
                isOnDes = true;
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                LogicGame4.instance.HideShadow(id);
                SpriteRenderer spr = this.GetComponent<SpriteRenderer>();
                spr.DOFade(0.7f, 0.2f).OnComplete(() =>
                {
                    spr.DOFade(1, 0.2f).OnComplete(() =>
                    {
                        spr.DOFade(0.7f, 0.2f).OnComplete(() =>
                        {
                            spr.DOFade(1, 0.2f).OnComplete(() =>
                            {
                                this.GetComponent<PooledObject>().ReturnToPool();
                                LogicGame4.instance.itemsIns.Remove(this);
                            });
                        });
                    });
                });
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Tag T = collision.gameObject.GetComponent<Tag>();
            if (T != null)
            {
                if (T.tagName == "Tray")
                {
                    this.GetComponent<PooledObject>().ReturnToPool();

                    LogicGame4.instance.GetPoint(id);
                    LogicGame4.instance.itemsIns.Remove(this);
                }
            }
        }
    }
}
