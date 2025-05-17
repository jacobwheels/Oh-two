using UnityEngine;
using UnityEngine.EventSystems;

public class TimerHoverPanelActivator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject panelToActivate; // The GameObject to activate/deactivate

    private void Awake()
    {
        // Ensure the panel is initially inactive
        if (panelToActivate != null)
        {
            panelToActivate.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"PanelToActivate is not assigned on {gameObject.name}!");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Activate the panel when the mouse enters the image
        if (panelToActivate != null)
        {
            panelToActivate.SetActive(true);
            Debug.Log($"Activated {panelToActivate.name} on hover over {gameObject.name}");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Deactivate the panel when the mouse exits the image
        if (panelToActivate != null)
        {
            panelToActivate.SetActive(false);
            Debug.Log($"Deactivated {panelToActivate.name} on exit from {gameObject.name}");
        }
    }
}