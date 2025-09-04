using UnityEngine;

namespace DevDuck
{
    public class CheckIsInGround : MonoBehaviour
    {
        public bool CheckInGround;
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Finish"))
            {
                this.CheckInGround = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Finish"))
            {
                this.CheckInGround = false;
            }
        }
    }
}
