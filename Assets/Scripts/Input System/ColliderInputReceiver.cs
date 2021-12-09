using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderInputReceiver : InputReceiver
{
    private Vector3 clickPosition;
    private bool buttonDown;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                clickPosition = hit.point;
                buttonDown = true;
                OnInputReceived();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                clickPosition = hit.point;
                buttonDown = false;
                OnInputReceived();
            }
        }
    }
    
    public override void OnInputReceived()
    {
        foreach (var handler in inputHandlers)
        {
            handler.ProcessInput(clickPosition, null, null, buttonDown);
        }
    }
}
