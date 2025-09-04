using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using TMPro;

// by nt.Dev93
namespace ntDev
{
    public class EasyButton : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler, IPointerClickHandler
    {
        public Image img;

        [HideInInspector] public Sprite normalSprite;
        [HideInInspector] public Sprite pressedSprite;
        [HideInInspector] public Sprite disableSprite;

        public TextMeshProUGUI txt;
        public string Text
        {
            get => txt == null ? "" : txt.text;
            set { if (txt != null) txt.text = value; }
        }
        float posY;
        [HideInInspector] public GameObject Down;
        [HideInInspector] public int DownPixel = 0;

        [SerializeField] bool Sound = true;
        [HideInInspector] public bool enableGrayFX = false;
        [HideInInspector] public bool enableScaleFX = false;
        [HideInInspector] public bool enableSpriteFX = false;

        void Awake()
        {
            if (Down != null) posY = Down.Lpos().y;
            scaleBase = transform.localScale.x;
        }

        public Action act;
        public void OnClick(Action act) => this.act = act;

        public Action<object> actO;
        public object o;
        public void OnClick(Action<object> actO, object e = null)
        {
            this.actO = actO;
            this.o = e;
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || disable) return;
            if (act != null) act();
            else if (actO != null) actO(o);
            // if (Sound) ManagerSound.PlayBGMStory(ManagerSound._instance.aClick);
        }

        float scaleBase = 1f;
        float scaleRatio = 0.95f;
        int scaleFlow = 0;
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || disable) return;
            if (enableSpriteFX && pressedSprite != null)
                img.sprite = pressedSprite;
            if (Down != null) Down.LposY(posY - DownPixel);
            scaleFlow = -1;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || disable) return;
            if (enableSpriteFX && normalSprite != null)
                img.sprite = normalSprite;
            if (Down != null) Down.LposY(posY);
            scaleFlow = 1;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (scaleFlow != -1) return;
            if (enableSpriteFX && normalSprite != null)
                img.sprite = normalSprite;
            if (Down != null) Down.LposY(posY);
            scaleFlow = 1;
        }

        bool disable = false;
        public bool Disable
        {
            get => disable;
            set
            {
                if (img != null)
                {
                    if (enableSpriteFX && disableSprite != null)
                        img.sprite = value ? disableSprite : normalSprite;
                    else if (enableGrayFX) img.color = value ? new Color(130 / 255f, 130 / 255f, 130 / 255f) : Color.white;
                }

                disable = value;
            }
        }

        public void SetNativeSize() => img.SetNativeSize();

        void OnDisable()
        {
            OnPointerExit(null);
            transform.localScale = new Vector3(scaleBase, scaleBase, 1);
        }

        void Update()
        {
            if (enableScaleFX)
            {
                if (scaleFlow < 0)
                {
                    transform.localScale -= new Vector3(0.02f, 0.02f, 0);
                    if (transform.localScale.x < scaleBase * scaleRatio)
                    {
                        transform.localScale = new Vector3(scaleBase, scaleBase, 1) * scaleRatio;
                        scaleFlow = 0;
                    }
                }
                else if (scaleFlow > 0)
                {
                    transform.localScale += new Vector3(0.02f, 0.02f, 0);
                    if (transform.localScale.x > scaleBase)
                    {
                        transform.localScale = new Vector3(scaleBase, scaleBase, 1);
                        scaleFlow = 0;
                    }
                }
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(EasyButton)), CanEditMultipleObjects]
    public class EasyButton_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            serializedObject.Update();
            EasyButton button = (EasyButton)target;

            if (!button.enabled) return;
            if (button.img == null)
            {
                Image img = button.GetComponent<Image>();
                if (img != null)
                {
                    button.img = img;
                    if (button.normalSprite == null) button.normalSprite = img.sprite;
                }
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enableGrayFX"), new GUIContent("Gray FX"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enableScaleFX"), new GUIContent("Scale FX"));
            if (button.enableSpriteFX)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Down"), new GUIContent("Down Object When Press"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("DownPixel"), new GUIContent("Low Pixel When Press"));
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("img"), new GUIContent("Image"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enableSpriteFX"), new GUIContent("Sprite FX"));

            if (button.enableSpriteFX)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("normalSprite"), new GUIContent("Normal Sprite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pressedSprite"), new GUIContent("Pressed Sprite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("disableSprite"), new GUIContent("Disable Sprite"));
                EditorGUI.indentLevel--;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}