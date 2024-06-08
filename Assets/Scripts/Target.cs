using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
  
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Target Hit");
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log("Target destroyed.");
        TargetDestroyedEvent.TargetDestroyed();
    }

}
