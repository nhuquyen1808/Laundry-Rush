using UnityEngine;

namespace DevDuck
{
    public enum MOVE_TYPE
    {
        RIGHT_TO_LEFT,UPRIGHT_TO_LEFTDOWN,TOPTOBOT
    }
    public class ParalaxEffect : MonoBehaviour
    {
        public MOVE_TYPE moveType;
        public float speed = 0.1f; 
        private Material mat;
        private Vector2 offset;

        void Start()
        {
            mat = GetComponent<Renderer>().material;
        }

        void Update()
        {
            offset += SetOffset();
            mat.mainTextureOffset = offset;
        }


        public Vector2 SetOffset()
        {
            Vector2 tempOffset = Vector2.zero;
            switch (moveType)
            {
                case MOVE_TYPE.RIGHT_TO_LEFT:
                    tempOffset = new Vector2(speed * Duck.TimeMod, 0); 
                    break;
                case MOVE_TYPE.UPRIGHT_TO_LEFTDOWN:
                    tempOffset = new Vector2(speed * Duck.TimeMod, speed * Duck.TimeMod); 
                    break;
                case MOVE_TYPE.TOPTOBOT:
                    tempOffset = new Vector2(/*speed * Time.deltaTime*/0, speed * Duck.TimeMod); 
                    break;
                default:
                     tempOffset = Vector2.zero;
                    break;
            }
           return tempOffset;
        }
    }
}
