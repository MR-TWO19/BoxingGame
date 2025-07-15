using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHitBox : HitBox
{
    [SerializeField] Collider collider;

    public void DissableColide() => collider.enabled = false;
    public void EnabledColide() => collider.enabled = true;
}
