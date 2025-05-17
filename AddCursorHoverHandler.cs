using UnityEngine;
using UnityEngine.UI;

public class AddCursorHoverHandler : MonoBehaviour
{
    [ContextMenu("Add CursorHoverHandler to All Selectables")]
    public void AddToAllSelectables()
    {
        // Find all Selectable components, including inactive ones
        Selectable[] selectables = Resources.FindObjectsOfTypeAll<Selectable>();

        int addedCount = 0;
        foreach (Selectable selectable in selectables)
        {
            // Skip if the selectable is null or not in the scene (e.g., in a prefab)
            if (selectable == null || selectable.gameObject.scene.name == null)
            {
                continue;
            }

            // Add the CursorHoverHandler if it doesn't already exist
            if (!selectable.gameObject.GetComponent<CursorHoverHandler>())
            {
                selectable.gameObject.AddComponent<CursorHoverHandler>();
                addedCount++;
                Debug.Log($"Added CursorHoverHandler to {selectable.gameObject.name} (Type: {selectable.GetType().Name})");
            }
        }

        Debug.Log($"Added CursorHoverHandler to {addedCount} selectable UI elements.");
    }
}