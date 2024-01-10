
using System.Collections.Generic;
using UnityEngine;

public class IngredientiMove : MonoBehaviour
{
    private GameObject ingredientiObj;
    private Vector2 touchStart;
    private Vector2 touchEnd;
    private bool swipe = false;
    public float distanza = 1.0f;
    public float move = 1.0f;
    public LayerMask ingredientLayer;
    private Stack<IngredientAction> actions = new Stack<IngredientAction>();



    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStart = touch.position;
                Ray ray = Camera.main.ScreenPointToRay(touchStart);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    ingredientiObj = hit.transform.gameObject;
                    swipe = false;
                }
            }
            else if (touch.phase == TouchPhase.Ended && ingredientiObj != null)
            {
                touchEnd = touch.position;
                if (!swipe)
                {
                    Swipe();
                }
            }
        }
    }
    private void Swipe()
    {
        float xDifference = touchEnd.x - touchStart.x;
        float yDifference = touchEnd.y - touchStart.y;
        float swipeThreshold = 100f;

        Vector3 direction = Vector3.zero;
        if (Mathf.Abs(xDifference) > Mathf.Abs(yDifference))
        {
            if (Mathf.Abs(xDifference) > swipeThreshold)
            {
                direction = xDifference > 0 ? Vector3.right : Vector3.left;
            }
        }
        else
        {
            if (Mathf.Abs(yDifference) > swipeThreshold)
            {
                direction = yDifference > 0 ? Vector3.forward : Vector3.back;
            }
        }

        if (direction != Vector3.zero)
        {
            if (ingredientiRay(direction))
            {
                RotazioneEMovimento(direction);
            }
        }
    }

    private bool ingredientiRay(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(ingredientiObj.transform.position, direction, out hit, distanza, ingredientLayer))
        {
            Ingredient ingredientBelow = hit.collider.GetComponent<Ingredient>();
            if (ingredientBelow != null)
            {
                float heightBelow = ingredientBelow.TotalHeight();

                Ingredient topIngredientOfStack = ingredientiObj.GetComponent<Ingredient>();
                float stackHeight = topIngredientOfStack != null ? topIngredientOfStack.TotalHeight() : 0;

                Vector3 newPosition = hit.collider.bounds.center + Vector3.up * (heightBelow + stackHeight - topIngredientOfStack.GetComponent<Collider>().bounds.size.y);
                MoveIngredient(ingredientiObj, newPosition, hit.transform);

                return true;
            }
        }
        return false;
    }


    private void RotazioneEMovimento(Vector3 direction)
    {
        ingredientiObj.transform.Rotate(0, 0, 180);

        ingredientiObj.transform.Translate(direction * move, Space.World);
    }



    private void MoveIngredient(GameObject ingredient, Vector3 newPosition, Transform newParent)
    {
        actions.Push(new IngredientAction(ingredient, ingredient.transform.position, ingredient.transform.parent));

        ingredient.transform.position = newPosition;
        ingredient.transform.SetParent(newParent);
    }


    public void Flip(GameObject topIngredient)
    {
        GameObject stackParent = new GameObject("IngredientStack");
        stackParent.transform.position = topIngredient.transform.position;

        while (topIngredient != null)
        {
            Ingredient ingredient = topIngredient.GetComponent<Ingredient>();
            if (ingredient == null) break;

            topIngredient.transform.SetParent(stackParent.transform);
            topIngredient = ingredient.transform.GetChild(0).gameObject;
        }
    }

}