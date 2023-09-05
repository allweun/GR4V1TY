using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetCamera : MonoBehaviour
{ 
   public GameObject target;
   public float speedRate=45f;
   private float zPlayer;
   private float yPlayer;
   private float xPlayer;
   
   private void Update()
   {
      transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, speedRate * Time.deltaTime);
   }
   void LateUpdate(){
      transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -5);
   }
}
