using UnityEngine;
using UnityEngine.UI;

namespace NQ_DEV
{


    [RequireComponent(typeof(RawImage))]
    public class UIBackgroundLoop : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed = 0.1f;

        private RawImage rawImage;
        private Rect uvRect;

        private void Awake()
        {
            rawImage = GetComponent<RawImage>();
            uvRect = rawImage.uvRect;
        }

        private void Update()
        {
            uvRect.y = Mathf.Repeat(
                uvRect.y + scrollSpeed * Time.deltaTime,
                1f
            );

            rawImage.uvRect = uvRect;
        }
    }

}
