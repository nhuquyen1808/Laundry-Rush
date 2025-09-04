using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class ShadowControl : MonoBehaviour
    {
        [SerializeField] int speed;
        public bool isEmpty = true;
        public float timer;
        public TextMeshPro timeText;
        public itemGame1 pieceGame1;
        public static ShadowControl ins;
        public GameObject posHold, grabObject;
        public int countPieceExist;
        public SpriteRenderer _spriteShadow;
        public GameObject model;
        public bool isCanMove;
        [SerializeField] private LogicUiGame1 logicUiGame1;
        [SerializeField] private Rigidbody2D _rb, _rbHandMachine;
        [SerializeField] float _moveSpeed;
        [SerializeField] FixedJoystick _joystick;

        [Header("Ui")] [SerializeField] private Image upArrow;
        [SerializeField] private Image downArrow;
        [SerializeField] private Image leftArrow;
        [SerializeField] private Image rightArrow;
        [SerializeField] private Sprite defaultSprite, usedSprite;

        public FocusTutorial focusTutorial;
        public bool isDropTutorial;

        private void Awake()
        {
            if (ins != null && ins != this)
            {
                Destroy(gameObject);
            }
            else
            {
                ins = this;
            }
        }

        private void Start()
        {
            timeText.rectTransform.localScale = Vector3.zero;
            Observer.AddObserver(EventAction.EVENT_DROPBUTTON_CLICKED, DropButtonClicked);
        }

        private void OnDestroy()
        {
            Observer.RemoveObserver(EventAction.EVENT_DROPBUTTON_CLICKED, DropButtonClicked);
        }

        private void DropButtonClicked(object obj)
        {
            if (LogicGame1.instance.isTutorial)
            {
                OutOfTime();
            }
            else
            {
                isDropTutorial = true;
                LogicGame1.instance.handPress.gameObject.SetActive(false);
                LogicGame1.instance.shadowTutorial.gameObject.SetActive(false);
                focusTutorial.gameObject.SetActive(false);

                DOVirtual.DelayedCall(1f, () =>
                {
                    focusTutorial.transform.position = LogicUiGame1.instance.starBar.transform.position;
                    focusTutorial.gameObject.SetActive(true);

                    LogicUiGame1.instance.SetPosStarBar(0.2f);
                    LogicGame1.instance.RunScoretutorial();
                    StartCoroutine(DoneTutorial());
                });
                isEmpty = true;
                grabObject.transform.rotation = Quaternion.identity;
                pieceGame1.transform.DOMove(_spriteShadow.transform.position, 0.3f).SetDelay(0.2f).OnComplete(() =>
                {
                    _spriteShadow.sprite = null;
                    pieceGame1.transform.SetParent(model.transform);
                    pieceGame1.transform.localScale = Vector3.one;
                    pieceGame1.transform.rotation = Quaternion.identity;
                });
                //     MoveGrabToOriginPos();
                LogicGame1.instance.handMachineHandle.AnimationState.SetAnimation(0, "drop", false);
            }
        }

        private void FixedUpdate()
        {
            if (isCanMove)
            {
                if (_joystick.Direction.y != 0)
                {
                    _rb.velocity = new Vector2(_joystick.Direction.x * _moveSpeed * Duck.TimeMod,
                        _joystick.Direction.y * _moveSpeed * Duck.TimeMod);
                    _rbHandMachine.velocity = new Vector2(_joystick.Direction.x * _moveSpeed * Duck.TimeMod,
                        _joystick.Direction.y * 0 * Duck.TimeMod);
                    //  Debug.Log(_joystick.Direction);
                    SetTwoDirectionUI();
                    SetOneDirectionUI();
                    if (focusTutorial.hand.gameObject.activeSelf)
                    {
                        focusTutorial.hand.gameObject.SetActive(false);
                        focusTutorial.gameObject.SetActive(false);
                        LogicGame1.instance.isPressControl = true;
                    }
                }
                else
                {
                    _rb.velocity = Vector2.zero;
                    _rbHandMachine.velocity = Vector2.zero;
                    SetDefaultUI();
                }
            }
        }


        private void Update()
        {
            if (!isEmpty && LogicGame1.instance.isTutorial)
            {
                timer -= Time.deltaTime;
                timeText.text = /*"0" + */Mathf.RoundToInt(timer).ToString();
                if (timer <= 0f)
                {
                    OutOfTime();
                }
            }
        }

        public void SetDiection()
        {
            _joystick.input = Vector2.zero;
        }

        private void OutOfTime()
        {
            timer = 21;
            isEmpty = true;
            timeText.text = "";
            grabObject.transform.rotation = Quaternion.identity;
            pieceGame1.transform.DOMove(_spriteShadow.transform.position, 0.3f).OnComplete(() =>
            {
                _spriteShadow.sprite = null;
                pieceGame1.transform.SetParent(model.transform);
                pieceGame1.transform.localScale = new Vector3(1.09f, 1.09f, 0);
                pieceGame1.transform.rotation = Quaternion.identity;
                LogicGame1.instance.CalculateScore(pieceGame1);
            });
            MoveGrabToOriginPos();
            LogicGame1.instance.handMachineHandle.AnimationState.SetAnimation(0, "drop", false);
        }

        public void MoveLeft()
        {
            if (!isCanMove) return;
            grabObject.transform.position = new Vector3(transform.position.x, grabObject.transform.position.y,
                grabObject.transform.position.z);
            transform.Translate(Vector3.left * speed * Duck.TimeMod);
            if (transform.position.x < -3.6f)
            {
                transform.position = new Vector3(transform.position.x, -3.6f, transform.position.z);
            }
        }

        public void MoveRight()
        {
            if (!isCanMove) return;
            grabObject.transform.position = new Vector3(transform.position.x, grabObject.transform.position.y,
                grabObject.transform.position.z);
            transform.Translate(Vector3.right * speed * Duck.TimeMod);
            if (transform.position.x > 3.6f)
            {
                transform.position = new Vector3(transform.position.x, 3.6f, transform.position.z);
            }
        }

        public void MoveUp()
        {
            if (!isCanMove) return;
            transform.Translate(Vector3.up * speed * Duck.TimeMod);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            if (transform.position.y > 0)
            {
                grabObject.transform.position = new Vector3(transform.position.x, 7f, transform.position.z);
            }
            /*else
            {
                grabObject.transform.position =
                    new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
            }*/
        }

        public void MoveDown()
        {
            if (!isCanMove) return;
            transform.Translate(Vector3.down * speed * Duck.TimeMod);
            //    grabObject.transform.localPosition = new Vector3(transform.position.x, 6.5f,transform.position.z);
            if (transform.position.y < -3.7f)
            {
                transform.position = new Vector3(transform.position.x, -3f, transform.position.z);
            }
        }

        public void StandStill()
        {
            LogicGame1.instance.clawMachineHandle.AnimationState.SetAnimation(0, "nothing", false);
        }

        void MoveGrabToDefaultPos()
        {
            Debug.Log("type  : " + pieceGame1.type);
            float posY = yPosItem(pieceGame1);
            transform.DOMove(new Vector3(-1, posY, 0), 0.5f).SetDelay(1.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                timeText.rectTransform.localScale = Vector3.one;
                isCanMove = true;
                logicUiGame1.dropButton.GetComponent<Image>().raycastTarget = true;
            });
            grabObject.transform.DOMove(new Vector3(-1, 7f, 0), 0.5f).SetDelay(1.5f).SetEase(Ease.OutBack);
        }

        void MoveGrabToOriginPos()
        {
            isCanMove = false;
            timeText.rectTransform.localScale = Vector3.zero;
            grabObject.transform.DOMove(new Vector3(0, 15f, 0), 0.5f).SetDelay(0.7f).SetEase(Ease.InBack);
            transform.DOMove(new Vector3(0, 14f, 0), 0.5f).SetDelay(0.7f).SetEase(Ease.InBack).OnComplete(() =>
            {
                //  countPieceExist--;
                if (LogicGame1.instance.itemGame1s.Count > 0)
                {
                    LogicGame1.instance.InsPieceGame1();
                    MoveGrabToDefaultPos();
                    timer = 21;
                    isEmpty = false;
                    LogicGame1.instance.handMachineHandle.AnimationState.SetAnimation(0, "hold", false);
                }
                else
                {
                    Observer.Notify(EventAction.EVENT_POPUP_SHOW, null);
                }
            });
        }

        public void ShakeGrabObject()
        {
            grabObject.transform.DOShakeRotation(0.3f, new Vector3(0, 0, 3), 1).OnComplete(() =>
            {
                grabObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            });
        }

        public void SetTwoDirectionUI()
        {
            if (_joystick.Direction.y > 0.2f && _joystick.Direction.x > 0.2f)
            {
                rightArrow.sprite = usedSprite;
                upArrow.sprite = usedSprite;
                leftArrow.sprite = defaultSprite;
                downArrow.sprite = defaultSprite;
            }
            else if (_joystick.Direction.y > 0.2f && _joystick.Direction.x < -0.2f)
            {
                leftArrow.sprite = usedSprite;
                upArrow.sprite = usedSprite;
                downArrow.sprite = defaultSprite;
                rightArrow.sprite = defaultSprite;
            }
            else if (_joystick.Direction.y < -0.2f && _joystick.Direction.x < -0.2f)
            {
                leftArrow.sprite = usedSprite;
                downArrow.sprite = usedSprite;
                rightArrow.sprite = defaultSprite;
                upArrow.sprite = defaultSprite;
            }
            else if (_joystick.Direction.x > 0.2f && _joystick.Direction.y < -0.2f)
            {
                rightArrow.sprite = usedSprite;
                downArrow.sprite = usedSprite;
                leftArrow.sprite = defaultSprite;
                upArrow.sprite = defaultSprite;
            }
        }

        public void SetOneDirectionUI()
        {
            if (_joystick.Direction.y <= 0.2f && _joystick.Direction.x > 0.2f && _joystick.Direction.y >= 0f)
            {
                rightArrow.sprite = usedSprite;
            }
            else if (_joystick.Direction.y <= 0.2f && _joystick.Direction.x < -0.2f && _joystick.Direction.y >= 0f)
            {
                leftArrow.sprite = usedSprite;
            }
            else if (_joystick.Direction.y < -0.2f && _joystick.Direction.x >= -0.2f && _joystick.Direction.x <= 0f)
            {
                downArrow.sprite = usedSprite;
            }
            else if (_joystick.Direction.y > 0.2f && _joystick.Direction.x <= -0.2f && _joystick.Direction.x <= 0f)
            {
                upArrow.sprite = usedSprite;
            }
        }

        private void SetDefaultUI()
        {
            upArrow.sprite = defaultSprite;
            leftArrow.sprite = defaultSprite;
            rightArrow.sprite = defaultSprite;
            downArrow.sprite = defaultSprite;
        }

        IEnumerator DoneTutorial()
        {
            PlayerPrefs.SetInt("isTutorial", 1);
            LogicGame1.instance.isTutorial = true;
            LogicGame1.instance.isInSceneTutorial = true;
            yield return new WaitForSeconds(1f);
            LogicGame1.instance.tutorialItem.gameObject.SetActive(false);
            focusTutorial.gameObject.SetActive(false);
            LogicGame1.instance.SetData();
            MoveGrabToOriginPos();
        }

        public float yPosItem(itemGame1 currentItem)
        {
            switch (currentItem.type)
            {
                case Type.HAT:
                    return 2.2f;
                    break;
                case Type.TOP:
                    return 0.32f;
                    break;
                case Type.BOT:
                    return -0.7f;
                    break;
                case Type.NECK:
                    return 1.1f;
                    break;
                case Type.OVERALL:
                    return 0.22f;
                    break;
                
                case Type.SHOES:
                    return -3.2f;
                    break;
                case Type.HAND:
                    return -0.5f;
                    break;
                case Type.FACE:
                    return 1.6f;
                    break;
                default:
                    return 0;
            }
        }
    }
}