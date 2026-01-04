using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosCharacters : MonoBehaviour
{
    public Vector3 GetPos(int idx)
    {
        Transform posTransform = transform.Find("Pos" + idx);
        if (posTransform != null)
        {
            return posTransform.position;
        }
        else
        {
            Debug.LogWarning("Not found Pos" + idx);
            return Vector3.zero;
        }
    }

    public Quaternion GetRotation(int idx)
    {
        Transform posTransform = transform.Find("Pos" + idx);
        if (posTransform != null)
        {
            return posTransform.rotation;
        }
        else
        {
            Debug.LogWarning("Not found Pos" + idx);
            return Quaternion.identity;
        }
    }

    public Transform GetTransform(int idx)
    {
        Transform posTransform = transform.Find("Pos" + idx);
        if (posTransform != null)
        {
            return posTransform;
        }
        else
        {
            Debug.LogWarning("Not found Pos" + idx);
            return null;
        }
    }
}