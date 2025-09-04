using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace DevDuck
{
    public class PlayerGame15 : MonoBehaviour
    {
        public LayerMask obstacle;
        public SkeletonAnimation _playerSkeletonAnimation;
        public SkeletonAnimation prveventAnimation;
        public GameObject arrow;
        
        private void Start()
        {
            ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"idle",true);
        }
        public void MoveLeft()
        {
            arrow.SetActive(true);
            arrow.transform.rotation = Quaternion.Euler(0,0,-90);
            RaycastHit2D hit = Physics2D.Raycast(transform.position , Vector2.left, Mathf.Infinity, obstacle);
            if (hit.collider != null)
            {
                Tag tag = hit.collider.gameObject.GetComponent<Tag>();
                if (tag == null) return;
                if (tag.tagName == "Obstacle")
                {
                    if (hit.distance > 1)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                        ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"run",true);
                        transform.DOMove(hit.point + new Vector2(0.61f, 0),
                            Duck.GetDistance(transform.position, hit.point) / 5).SetEase(Ease.Linear).OnComplete((() =>
                        {
                            ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"idle",true);
                            Observer.Notify(EventAction.EVENT_PLAYER_MOVE_DONE, true);
                        }));
                    }
                    else
                    {
                        Observer.Notify(EventAction.EVENT_PLAYER_MOVE_DONE, true);
                    }
                }
                if (tag.tagName == "ObstacleCat")
                {
                    if (hit.distance > 1)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"run",true);
                        transform.DOMove(hit.point - new Vector2(0.61f, 0),
                            Duck.GetDistance(transform.position, hit.point) / 5).SetEase(Ease.Linear).OnComplete((() =>
                        {
                            ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"idle",true);
                            PlayPreventEffect(270);
                            Observer.Notify(EventAction.EVENT_PLAYER_MOVE_DONE, true);
                        }));
                    }
                    else
                    {
                        PlayPreventEffect(270);
                        Observer.Notify(EventAction.EVENT_PLAYER_MOVE_DONE, true);
                    }
                }
                else if (tag.tagName == "Destination")
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"run",true);
                    Vector3 des = hit.point + new Vector2(-0.5f, 0f);
                    transform.DOMove(des, Duck.GetDistance(transform.position, des) / 3).OnComplete((
                        () =>
                        {
                            ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"idle",true);
                        }));
                }
            }
        }
        public void MoveRight()
        {
            arrow.SetActive(true);
            arrow.transform.rotation = Quaternion.Euler(0,0,90);
            arrow.GetComponent<Animator>().Play("ArrowGame15",0,0);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, obstacle);
            if (hit.collider != null)
            {
                Tag tag = hit.collider.gameObject.GetComponent<Tag>();
                if (tag == null) return;

                if (tag.tagName == "Obstacle")
                {
                    if (hit.distance > 1)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);

                        ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"run",true);
                        transform.DOMove(hit.point - new Vector2(0.61f, 0),
                            Duck.GetDistance(transform.position, hit.point) / 5).SetEase(Ease.Linear).OnComplete((() =>
                        {
                            ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"idle",true);
                            Observer.Notify(EventAction.EVENT_PLAYER_MOVE_DONE, true);
                        }));
                    }
                    else
                    {
                        Observer.Notify(EventAction.EVENT_PLAYER_MOVE_DONE, true);
                    }
                }
                if (tag.tagName == "ObstacleCat")
                {
                    if (hit.distance > 1)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"run",true);
                        transform.DOMove(hit.point - new Vector2(0.61f, 0),
                            Duck.GetDistance(transform.position, hit.point) / 5).SetEase(Ease.Linear).OnComplete((() =>
                        {
                            ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"idle",true);
                            PlayPreventEffect(90);
                            Observer.Notify(EventAction.EVENT_PLAYER_MOVE_DONE, true);
                        }));
                    }
                    else
                    {
                        PlayPreventEffect(90);
                        Observer.Notify(EventAction.EVENT_PLAYER_MOVE_DONE, true);
                    }
                }
                else if (tag.tagName == "Destination")
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"run",true);
                    Vector3 des = hit.point + new Vector2(0.5f, 0f);
                    transform.DOMove(des, Duck.GetDistance(transform.position, des) / 3).OnComplete((
                        () =>
                        {
                            ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"idle",true);
                        }));
                }
            }
        }

        public void MoveUp()
        {
            arrow.SetActive(true);
            arrow.transform.rotation = Quaternion.Euler(0,0,0);
            arrow.GetComponent<Animator>().Play("ArrowGame15",0,0);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, obstacle);
            if (hit.collider != null)
            {
                Tag tag = hit.collider.gameObject.GetComponent<Tag>();
                if (tag == null) return;
                if (tag.tagName == "Obstacle")
                {
                    if (hit.distance > 1)
                    {
                        ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"run",true);
                        transform.DOMove(hit.point - new Vector2(0,  0.61f),
                            Duck.GetDistance(transform.position, hit.point) / 5).SetEase(Ease.Linear).OnComplete((() =>
                        {
                            ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"idle",true);
                            Observer.Notify(EventAction.EVENT_PLAYER_MOVE_DONE, true);
                        }));
                    }
                    else
                    {
                       Observer.Notify(EventAction.EVENT_PLAYER_MOVE_DONE, true);
                    }
                }

                if (tag.tagName == "ObstacleCat")
                {
                    if (hit.distance > 1)
                    {
                        ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"run",true);
                        transform.DOMove(hit.point - new Vector2(0,  0.61f),
                            Duck.GetDistance(transform.position, hit.point) / 5).SetEase(Ease.Linear).OnComplete((() =>
                        {
                            ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"idle",true);
                            PlayPreventEffect(180);
                            Observer.Notify(EventAction.EVENT_PLAYER_MOVE_DONE, true);
                        }));
                    }
                    else
                    {
                        Observer.Notify(EventAction.EVENT_PLAYER_MOVE_DONE, true);
                        PlayPreventEffect(180);
                    }
                }
                else if (tag.tagName == "Destination")
                {
                    ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"run",true);
                    Vector3 des = hit.point + new Vector2(0f, 0.5f);
                    transform.DOMove(des, Duck.GetDistance(transform.position, des) / 3).OnComplete((
                        () =>
                        {
                            ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"idle",true);
                        }));
                }
            }
        }

        public void MoveDown()
        {
            arrow.SetActive(true);
            arrow.transform.rotation = Quaternion.Euler(0,0,180);
            arrow.GetComponent<Animator>().Play("ArrowGame15",0,0);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, obstacle);
            if (hit.collider != null)
            {
                Tag tag = hit.collider.gameObject.GetComponent<Tag>();
                if (tag == null) return;
                if (tag.tagName == "Obstacle")
                {
                    if (hit.distance > 1)
                    {
                        ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"run",true);
                        transform.DOMove(hit.point + new Vector2(0, 0.61f),
                            Duck.GetDistance(transform.position, hit.point) / 5).SetEase(Ease.Linear).OnComplete((() =>
                        {
                            Observer.Notify(EventAction.EVENT_PLAYER_MOVE_DONE, true);
                            ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"idle",true);
                        }));
                    }
                   else
                    {
                        Observer.Notify(EventAction.EVENT_PLAYER_MOVE_DONE, true);
                    }
                }
                if (tag.tagName == "ObstacleCat")
                {
                    if (hit.distance > 1)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"run",true);
                        transform.DOMove(hit.point + new Vector2( 0,0.61f),
                            Duck.GetDistance(transform.position, hit.point) / 5).SetEase(Ease.Linear).OnComplete((() =>
                        {
                            ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"idle",true);
                            PlayPreventEffect(0);
                            Observer.Notify(EventAction.EVENT_PLAYER_MOVE_DONE, true);
                        }));
                    }
                    else
                    {
                        PlayPreventEffect(0);
                        Observer.Notify(EventAction.EVENT_PLAYER_MOVE_DONE, true);
                    }
                }
                else if (tag.tagName == "Destination")
                {
                    ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"run",true);
                    Vector3 des = hit.point - new Vector2(0f, 0.5f);
                    transform.DOMove(des, Duck.GetDistance(transform.position, des) / 3).OnComplete((
                        () =>
                        {
                            ManagerSpine.PlaySpineAnimation(_playerSkeletonAnimation,"idle",true);
                        }));
                }
            }
        }

        private void OnDrawGizmos()
        {
            Debug.DrawRay(transform.position+ new Vector3(0,0.5f,0), Vector2.left * 1.25f, Color.red);
            Debug.DrawRay(transform.position+ new Vector3(0,0.5f,0), Vector2.right * 1.25f, Color.red);
            Debug.DrawRay(transform.position+ new Vector3(0,0.5f,0), Vector2.up * 1.25f, Color.red);
            Debug.DrawRay(transform.position+ new Vector3(0,0.5f,0), Vector2.down * 1.25f, Color.red);
        }

        private void PlayPreventEffect(int angle)
        {
            prveventAnimation.transform.eulerAngles = new Vector3(0, 0, angle);
            ManagerSpine.PlaySpineAnimation(prveventAnimation,"animation",false);

        }
    }
}