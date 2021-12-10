using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderInputReceiver : InputReceiver
{
    private Vector3 clickPosition;
    private bool buttonDown;
    private RaycastHit hit;
    private Ray ray;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                clickPosition = hit.point;
                hitGameObject = hit.collider.gameObject;
                buttonDown = true;
                OnInputReceived();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                clickPosition = hit.point;
                hitGameObject = hit.collider.gameObject;
                buttonDown = false;
                OnInputReceived();
            }
        }
    }
    
    public override void OnInputReceived()
    {
        inputHandlers = hitGameObject.transform.GetComponents<IInputHandler>();
        // print(hitGameObject.name);
        foreach (var handler in inputHandlers)
        {
            handler.ProcessInput(clickPosition, null, null, buttonDown);
        }
    }
}
