using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectTweener
{
    void MoveTo(Transform transform1, Vector3 targetPosition);
}
