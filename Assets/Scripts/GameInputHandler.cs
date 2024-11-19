using UnityEngine;
using UnityEngine.EventSystems;

public class GameInputHandler : MonoBehaviour
{
    void Update()
    {
        if (IsPointerOverUIObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            HandleGameInput();
        }
    }

    private bool IsPointerOverUIObject()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return true;

        return false;
    }

    private void HandleGameInput()
    {
        
    }
}