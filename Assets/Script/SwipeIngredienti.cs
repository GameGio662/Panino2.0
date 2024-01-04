using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeIngredienti : MonoBehaviour
{
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    private float minDistanceForSwipe = 20f;
    private GameObject selectedObject;
    private bool canMove = true;

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

            if (touch.phase == TouchPhase.Began && canMove)
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
            canMove = false;
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
                    canMove = true;
                }
            }
        }
    }

    void MoveObject()
    {
        float deltaX = fingerUpPosition.x - fingerDownPosition.x;
        float deltaY = fingerUpPosition.y - fingerDownPosition.y;

        if (Mathf.Abs(deltaX) > minDistanceForSwipe)
        {
            // Spostamento orizzontale
            if (deltaX > 0)
            {
                selectedObject.transform.Translate(Vector3.right, Space.World);
                selectedObject.transform.Rotate(Vector3.up, 180f);
            }
            else if (deltaX < 0)
            {
                selectedObject.transform.Translate(Vector3.left, Space.World);
                selectedObject.transform.Rotate(Vector3.up, 180f);
            }
        }

        fingerDownPosition = fingerUpPosition;
    }
}
