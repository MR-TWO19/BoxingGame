using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonMono<CameraManager>
{
    public CameraFollow cameraFollow;

    public void SetUpCammeraFollow(GameObject target)
    {
        cameraFollow.SetTarget(target.transform);
    }
}
