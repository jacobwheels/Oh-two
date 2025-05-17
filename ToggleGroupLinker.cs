using UnityEngine;
using UnityEngine.UI;

public class ToggleGroupLinker : MonoBehaviour
{
    [Header("Toggle Groups")]
    public ToggleGroup sourceToggleGroup; // The toggle group to listen to (e.g., "Settings")
    public ToggleGroup targetToggleGroup; // The toggle group to deselect (e.g., "Tabs")

    private Toggle[] sourceToggles; // Toggles in the source group
    private Toggle[] targetToggles; // Toggles in the target group

    private void Start()
    {
        // Get all toggles in the source toggle group
        if (sourceToggleGroup != null)
        {
            sourceToggles = FindTogglesInGroup(sourceToggleGroup);
            if (sourceToggles.Length == 0)
            {
                Debug.LogWarning($"No toggles found in source toggle group {sourceToggleGroup.gameObject.name}!");
            }

            // Add listeners to each toggle in the source group
            foreach (var toggle in sourceToggles)
            {
                toggle.onValueChanged.AddListener((isOn) => OnSourceToggleChanged(toggle, isOn));
            }
        }
        else
        {
            Debug.LogError("Source ToggleGroup is not assigned in ToggleGroupLinker!");
        }

        // Get all toggles in the target toggle group
        if (targetToggleGroup != null)
        {
            targetToggles = FindTogglesInGroup(targetToggleGroup);
            if (targetToggles.Length == 0)
            {
                Debug.LogWarning($"No toggles found in target toggle group {targetToggleGroup.gameObject.name}!");
            }
        }
        else
        {
            Debug.LogError("Target ToggleGroup is not assigned in ToggleGroupLinker!");
        }
    }

    private void OnSourceToggleChanged(Toggle changedToggle, bool isOn)
    {
        if (isOn)
        {
            // A toggle in the source group was selected, deselect all toggles in the target group
            DeselectTargetGroup();
            Debug.Log($"Toggle {changedToggle.gameObject.name} in {sourceToggleGroup.gameObject.name} was selected. Deselected all toggles in {targetToggleGroup.gameObject.name}.");
        }
    }

    private void DeselectTargetGroup()
    {
        if (targetToggles == null || targetToggles.Length == 0)
        {
            return;
        }

        foreach (var toggle in targetToggles)
        {
            if (toggle != null)
            {
                toggle.isOn = false;
            }
        }
    }

    private Toggle[] FindTogglesInGroup(ToggleGroup group)
    {
        // Find all toggles in the scene and filter by the specified toggle group
        Toggle[] allToggles = Resources.FindObjectsOfTypeAll<Toggle>();
        System.Collections.Generic.List<Toggle> groupToggles = new System.Collections.Generic.List<Toggle>();

        foreach (var toggle in allToggles)
        {
            if (toggle != null && toggle.gameObject.scene.name != null && toggle.group == group)
            {
                groupToggles.Add(toggle);
            }
        }

        return groupToggles.ToArray();
    }
}