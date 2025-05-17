using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class CursorHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Selectable selectable; // Reference to the Selectable component (Button, Toggle, etc.)

    private void Awake()
    {
        // Get the Selectable component (Button, Toggle, etc.)
        selectable = GetComponent<Selectable>();
        if (selectable == null)
        {
            Debug.LogWarning($"No Selectable component found on {gameObject.name}. CursorHoverHandler requires a Selectable component.");
        }
    }

    private void OnEnable()
    {
        // Ensure the default cursor is set when this element is enabled
        if (CursorManager.Instance != null && !CursorManager.Instance.IsHovering())
        {
            CursorManager.Instance.SetDefaultCursor();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Only change the cursor if the element is interactable
        if (selectable != null && selectable.interactable && CursorManager.Instance != null)
        {
            CursorManager.Instance.SetPointingHandCursor();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Revert to the default cursor when exiting
        if (selectable != null && selectable.interactable && CursorManager.Instance != null)
        {
            CursorManager.Instance.SetDefaultCursor();
        }
    }

    private void OnDisable()
    {
        // Revert to the default cursor when this element is disabled, if it was being hovered
        if (CursorManager.Instance != null && CursorManager.Instance.IsHovering())
        {
            CursorManager.Instance.SetDefaultCursor();
        }
    }
}