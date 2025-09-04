using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class thing : IdComponent
{
   private Rigidbody rb;

   public Rigidbody Rigidbody
   {
      get
      {
         if (rb == null)
         {
            rb = GetComponent<Rigidbody>();
         }
         return rb;
      }
   }
   public Vector3 defaultRotation;

   public void Move(Vector3 destination)
   {
      transform.DOJump(destination, 0.2f, 1, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
      {
         Rigidbody.isKinematic = true;
      });
   }
}
