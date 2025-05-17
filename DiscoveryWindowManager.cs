// DiscoveryWindowManager.cs
using UnityEngine;
using UnityEngine.UI;

namespace O2Game // Add this namespace
{
public class DiscoveryWindowManager : MonoBehaviour
{
    public Button closeButton; // Reference to the "Close" button on the discovery window

    private void Awake()
    {
        // Validate the close button
        if (closeButton == null)
        {
            Debug.LogWarning("CloseButton is not assigned in DiscoveryWindowManager! Attempting to find it.");
            closeButton = GetComponentInChildren<Button>();
            if (closeButton == null)
            {
                Debug.LogError("No CloseButton found in DiscoveryWindowManager!");
                return;
            }
        }

        // Add listener to the close button
        closeButton.onClick.AddListener(CloseWindow);
    }

    private void CloseWindow()
    {
        // Hide the window
        gameObject.SetActive(false);

        // Resume timers via ResourceTimerManager
        if (ResourceTimerManager.instance != null)
        {
            ResourceTimerManager.instance.PauseTimers(false);
            Debug.Log("Discovery window closed, timers resumed.");
        }
        else
        {
            Debug.LogWarning("ResourceTimerManager instance not found! Timers not resumed.");
        }
    }
}
}