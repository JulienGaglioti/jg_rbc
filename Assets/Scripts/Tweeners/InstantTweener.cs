using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantTweener : MonoBehaviour, IObjectTweener
{
    public void MoveTo(Transform transform1, Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }
}
