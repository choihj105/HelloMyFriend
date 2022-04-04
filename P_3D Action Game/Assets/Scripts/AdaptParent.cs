using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptParent : MonoBehaviour
{
    Player target;

    private void Awake()
    {
        target = transform.GetComponentInParent<Player>();
    }
    void adaptReloadOut()
    {
        target.ReloadOut();
    
    }
}
