using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHitBox : HitBox
{
    [SerializeField] Collider handCollider;

    public void DissableColide() => handCollider.enabled = false;
    public void EnabledColide() => handCollider.enabled = true;
}
