using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeIngredienti : MonoBehaviour
{
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    private float minDistanceForSwipe = 20f;
    private GameObject selectedObject;

    void Update()
    {
        HandleInput();
        DetectSwipe();
    }

    void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && selectedObject == null)
            {
                SelectObject(touch.position);
            }
        }
    }

    void SelectObject(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            selectedObject = hit.collider.gameObject;
        }
    }

    void DetectSwipe()
    {
        if (selectedObject != null)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    fingerUpPosition = touch.position;
                    MoveObject();
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    fingerUpPosition = touch.position;
                    MoveObject();
                    selectedObject = null; // Deseleziona l'oggetto dopo lo swipe
                }
            }
        }
    }

    void MoveObject()
    {
        float deltaX = fingerUpPosition.x - fingerDownPosition.x;
        float deltaY = fingerUpPosition.y - fingerDownPosition.y;

        if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
        {
            // Spostamento orizzontale
            if (Mathf.Abs(deltaX) > minDistanceForSwipe)
            {
                if (deltaX > 0)
                    selectedObject.transform.Translate(Vector3.right, Space.World);
                else if (deltaX < 0)
                    selectedObject.transform.Translate(Vector3.left, Space.World);
            }
        }
        else
        {
            // Spostamento verticale
            if (Mathf.Abs(deltaY) > minDistanceForSwipe)
            {
                if (deltaY > 0)
                    selectedObject.transform.Translate(Vector3.up, Space.World);
                else if (deltaY < 0)
                    selectedObject.transform.Translate(Vector3.down, Space.World);
            }
        }

        fingerDownPosition = fingerUpPosition;
    }
}
