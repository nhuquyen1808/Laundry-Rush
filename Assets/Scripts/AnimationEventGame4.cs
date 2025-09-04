using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class AnimationEventGame4 : MonoBehaviour
    {
        [Header("Lose : ")]
        [SerializeField] Sprite face1, face2;
        public Image rightStarLose, pieceRightStarLose, bento;
        [Header("Win : ")]
        
        public Animator animator;
  
        [SerializeField] private ParticleSystem starExplosion1,starExplosion2,starExplosion3;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }
        public  virtual void FallStarLose()
        {
            animator.enabled = false;
            rightStarLose.GetComponent<RectTransform>().DOJump(rightStarLose.transform.position - new Vector3(-100, 1300, 0), 700, 1, 1.3f).SetDelay(0.5f).OnComplete(() =>
            {
                if (bento != null && face2 != null)
                {
                 bento.sprite = face2;
                    
                }
                Observer.Notify(EventAction.EVENT_POPUP_SHOW_LOSE_DONE, "");
            });
            pieceRightStarLose.GetComponent<RectTransform>().DOJump(pieceRightStarLose.transform.position - new Vector3(-100, 1200, 0), 700, 1, 1).SetDelay(0.5f);
        }

        public void ShowParticle1()
        {
            starExplosion1.gameObject.SetActive(true);
            starExplosion1.Play();
        }
        public void ShowParticle2()
        {
            starExplosion2.gameObject.SetActive(true);

            starExplosion2.Play();
        }
        public void ShowParticle3()
        {
            starExplosion3.gameObject.SetActive(true);
            starExplosion3.Play();
            Observer.Notify(EventAction.EVENT_POPUP_SHOW_WIN_DONE, "");
        }

      
        
    }
}
