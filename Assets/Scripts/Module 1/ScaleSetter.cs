using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleSetter : MonoBehaviour
{
   public Vector3 scaleToSet = Vector3.one;

   public void SetScale()
   {
      transform.localScale = scaleToSet;
   }

   IEnumerator DoSetScale()
   {
      yield return null;
      transform.localScale = scaleToSet;
   }
}
