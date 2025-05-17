using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScavengeDropdownManager : MonoBehaviour
{
    [System.Serializable]
    public struct DropdownOption
    {
        public string optionName;
        public Sprite iconSprite;
        public float cooldownDuration; // Cooldown duration for this option
        [HideInInspector] public bool isUnlocked; // Whether this option is unlocked
    }

    public TMP_Dropdown scavengeDropdown; // Reference to the TMP_Dropdown component
    private CanvasGroup dropdownCanvasGroup; // For fading the dropdown

    [SerializeField] private DropdownOption[] options = new DropdownOption[7]; // Array to hold the 7 options, their icons, and cooldowns

    private void Awake()
    {
        // Validate the dropdown reference
        if (scavengeDropdown == null)
        {
            Debug.LogError("ScavengeDropdown is not assigned in ScavengeDropdownManager!");
            return;
        }

        // Set up the CanvasGroup for fading
        dropdownCanvasGroup = scavengeDropdown.GetComponent<CanvasGroup>();
        if (dropdownCanvasGroup == null)
        {
            dropdownCanvasGroup = scavengeDropdown.gameObject.AddComponent<CanvasGroup>();
        }

        // Initialize the dropdown as hidden
        dropdownCanvasGroup.alpha = 0f;
        scavengeDropdown.gameObject.SetActive(false);

        // Initialize the unlocked state (only Copper and Steel are unlocked initially)
        for (int i = 0; i < options.Length; i++)
        {
            DropdownOption option = options[i];
            // Assuming Copper is at index 0 and Steel is at index 1
            option.isUnlocked = (i == 0 || i == 1); // Unlock Copper and Steel
            options[i] = option;
        }

        // Populate the dropdown with unlocked options
        InitializeDropdown();

        // Update the caption text when the dropdown value changes
        scavengeDropdown.onValueChanged.AddListener(delegate { UpdateCaptionText(); });
    }

    private void InitializeDropdown()
    {
        // Clear any existing options
        scavengeDropdown.ClearOptions();

        // Create a list of options with their names, icons, and cooldowns (only for unlocked options)
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();

        foreach (var option in options)
        {
            if (!option.isUnlocked)
            {
                continue; // Skip locked options
            }

            if (string.IsNullOrEmpty(option.optionName))
            {
                Debug.LogWarning("An option in ScavengeDropdownManager has an empty name!");
                continue;
            }

            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData
            {
                text = option.optionName,
                image = option.iconSprite
            };
            dropdownOptions.Add(optionData);
        }

        // Add the options to the dropdown
        scavengeDropdown.AddOptions(dropdownOptions);

        // Set the default selection to the first unlocked option (Copper)
        if (dropdownOptions.Count > 0)
        {
            scavengeDropdown.value = 0;
            scavengeDropdown.RefreshShownValue();
            UpdateCaptionText(); // Set the initial caption
            Debug.Log($"After InitializeDropdown, captionText is: {(scavengeDropdown.captionText != null ? scavengeDropdown.captionText.text : "null")}");
        }
        else
        {
            Debug.LogWarning("No unlocked options available to initialize the dropdown!");
        }

        Debug.Log("Scavenge dropdown initialized with unlocked options and icons.");
    }

    // Public method to trigger the fade-in of the dropdown
    public void ShowDropdown()
    {
        if (scavengeDropdown != null && dropdownCanvasGroup != null)
        {
            scavengeDropdown.gameObject.SetActive(true);
            StartCoroutine(FadeCanvasGroup(dropdownCanvasGroup, 0f, 1f, 1f));
            // Update the caption after a short delay to ensure the dropdown is fully initialized
            StartCoroutine(UpdateCaptionAfterDelay());
        }
    }

    // Coroutine to update the caption after a short delay
    private System.Collections.IEnumerator UpdateCaptionAfterDelay()
    {
        // Wait for a short delay to ensure the dropdown is fully rendered
        yield return new WaitForSeconds(0.1f);
        UpdateCaptionText();
        Debug.Log($"After ShowDropdown (delayed), captionText is: {(scavengeDropdown.captionText != null ? scavengeDropdown.captionText.text : "null")}");
    }

    // Public method to get the currently selected option
    public string GetSelectedOption()
    {
        if (scavengeDropdown != null && scavengeDropdown.options.Count > 0)
        {
            return scavengeDropdown.options[scavengeDropdown.value].text;
        }
        Debug.LogWarning("ScavengeDropdown is not properly initialized or has no options!");
        return "";
    }

    // Public method to get the cooldown duration of the currently selected option
    public float GetSelectedOptionCooldown()
    {
        if (scavengeDropdown != null && scavengeDropdown.options.Count > 0)
        {
            string selectedOptionName = scavengeDropdown.options[scavengeDropdown.value].text;
            foreach (var option in options)
            {
                if (option.optionName == selectedOptionName)
                {
                    return option.cooldownDuration;
                }
            }
        }
        Debug.LogWarning("Could not find cooldown for the selected option! Returning default value.");
        return 1f; // Default cooldown if something goes wrong
    }

    // Public method to unlock an option by name
    public void UnlockOption(string optionName)
    {
        bool found = false;
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i].optionName == optionName)
            {
                DropdownOption option = options[i];
                if (!option.isUnlocked)
                {
                    option.isUnlocked = true;
                    options[i] = option;
                    found = true;
                    Debug.Log($"Unlocked option: {optionName}");
                    break;
                }
            }
        }

        if (!found)
        {
            Debug.LogWarning($"Option '{optionName}' not found or already unlocked!");
            return;
        }

        // Reinitialize the dropdown to include the newly unlocked option
        InitializeDropdown();
    }

    // Update the dropdown caption to show the name and cooldown (e.g., "Copper - 1.0s")
    private void UpdateCaptionText()
    {
        if (scavengeDropdown != null && scavengeDropdown.options.Count > 0 && scavengeDropdown.captionText != null)
        {
            string selectedOptionName = scavengeDropdown.options[scavengeDropdown.value].text;
            float cooldown = 1f; // Default value

            // Find the cooldown for the selected option
            foreach (var option in options)
            {
                if (option.optionName == selectedOptionName)
                {
                    cooldown = option.cooldownDuration;
                    break;
                }
            }

            // Update the caption text
            scavengeDropdown.captionText.text = $"{selectedOptionName} - {cooldown:F1}s";
            Debug.Log($"Updated dropdown caption to: {scavengeDropdown.captionText.text}");
        }
        else
        {
            Debug.LogWarning("Cannot update dropdown caption: Dropdown or captionText is not properly initialized!");
        }
    }

    // Coroutine for fading the CanvasGroup
    private System.Collections.IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        canvasGroup.alpha = startAlpha;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}