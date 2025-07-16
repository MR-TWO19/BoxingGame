using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreSelfCollision : MonoBehaviour
{
    public bool autoIgnoreOnStart = true;

    private void Start()
    {
        if (autoIgnoreOnStart)
        {
            Ignore();
        }
    }

    public void Ignore()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        for (int i = 0; i < colliders.Length; i++)
        {
            for (int j = i + 1; j < colliders.Length; j++)
            {
                Physics.IgnoreCollision(colliders[i], colliders[j]);
            }
        }

        Debug.Log($"[IgnoreSelfCollision] Đã bỏ va chạm giữa {colliders.Length} collider trong {name}");
    }
}
