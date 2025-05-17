// StoryManager.cs
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

namespace O2Game // Add this namespace
{
public class StoryManager : MonoBehaviour
{
    public TMP_Text bodyText;

        // UI elements to reveal
    public UnityEngine.UI.Toggle bridgeTabToggle;
    private CanvasGroup bridgeTabCanvasGroup;

    public UnityEngine.UI.Toggle researchBayTabToggle;
    private CanvasGroup researchBayTabCanvasGroup;

    public UnityEngine.UI.Toggle fabricatorTabToggle;
    private CanvasGroup fabricatorTabCanvasGroup;

    public UnityEngine.UI.Toggle barracksTabToggle;
    private CanvasGroup barracksTabCanvasGroup;

    public TMPro.TMP_Text darkPassagesTitleText;
    private CanvasGroup darkPassagesCanvasGroup;

    public TMPro.TMP_Text actionTitleText;
    private CanvasGroup actionTitleCanvasGroup;

    public UnityEngine.UI.Button restoreHealthButton;
    private CanvasGroup restoreHealthCanvasGroup;

    public TMPro.TMP_Dropdown autoScavDropdown;
    private CanvasGroup autoScavCanvasGroup;

    public GameObject settings;
    private CanvasGroup settingsCanvasGroup;

    // Scavenge button
    public Button scavengeButton;
    private CanvasGroup scavengeButtonCanvasGroup;

    // Inventory view
    public GameObject inventoryPanel; // New field for inventory UI
    private CanvasGroup inventoryCanvasGroup; // New field for inventory CanvasGroup
    private bool hasInventoryAppeared = false; // New flag to track inventory visibility

    // Reference to the ScavengeDropdownManager
    public ScavengeDropdownManager scavengeDropdownManager;

    // Discovery windows
    public GameObject researchBayDiscoveryWindow;
    public GameObject fabricatorDiscoveryWindow;

    // Reference to ResearchBayManager
    public ResearchBayManager researchBayManager;

    // Reference to the Research Bay group
    public GameObject researchBayGroup;

    // Track scavenge events
    private bool hasScavenged = false;
    private bool hasBridgeTabAppeared = false;

    private void Awake()
    {
        InitializeUIElements();
        if (researchBayDiscoveryWindow == null)
        {
            Debug.LogWarning("ResearchBayDiscoveryWindow is not assigned in StoryManager!");
        }
        else
        {
            researchBayDiscoveryWindow.SetActive(false);
        }

        if (fabricatorDiscoveryWindow == null)
        {
            Debug.LogWarning("FabricatorDiscoveryWindow is not assigned in StoryManager!");
        }
        else
        {
            fabricatorDiscoveryWindow.SetActive(false);
        }

        if (researchBayManager == null)
        {
            Debug.LogWarning("ResearchBayManager is not assigned in StoryManager! Attempting to find it.");
            researchBayManager = FindObjectOfType<ResearchBayManager>();
            if (researchBayManager == null)
            {
                Debug.LogWarning("ResearchBayManager not found in the scene!");
            }
        }

        if (researchBayGroup == null)
        {
            Debug.LogWarning("ResearchBayGroup is not assigned in StoryManager!");
        }

        if (researchBayTabToggle != null && researchBayManager != null)
        {
            researchBayTabToggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    researchBayManager.OnTabOpened();
                    if (researchBayGroup != null)
                    {
                        LayoutRebuilder.ForceRebuildLayoutImmediate(researchBayGroup.GetComponent<RectTransform>());
                        Debug.Log("Forced layout rebuild on ResearchBayGroup after enabling.");
                    }
                    Debug.Log("Research Bay tab toggled on, called OnTabOpened on ResearchBayManager.");
                }
            });
        }
    }

    private void InitializeUIElements()
    {
        if (bodyText == null)
        {
            Debug.LogWarning("BodyText is not assigned in StoryManager!");
        }
        else
        {
            bodyText.text = "";
        }

        if (bridgeTabToggle == null) Debug.LogWarning("BridgeTabToggle is not assigned in StoryManager!");
        if (researchBayTabToggle == null) Debug.LogWarning("ResearchBayTabToggle is not assigned in StoryManager!");
        if (fabricatorTabToggle == null) Debug.LogWarning("FabricatorTabToggle is not assigned in StoryManager!");
        if (barracksTabToggle == null) Debug.LogWarning("BarracksTabToggle is not assigned in StoryManager!");
        if (darkPassagesTitleText == null) Debug.LogWarning("DarkPassagesTitleText is not assigned in StoryManager!");
        if (actionTitleText == null) Debug.LogWarning("ActionTitleText is not assigned in StoryManager!");
        if (restoreHealthButton == null) Debug.LogWarning("RestoreHealthButton is not assigned in StoryManager!");
        if (autoScavDropdown == null) Debug.LogWarning("AutoScavDropdown is not assigned in StoryManager!");
        if (settings == null) Debug.LogWarning("Settings is not assigned in StoryManager!");
        if (scavengeButton == null) Debug.LogWarning("ScavengeButton is not assigned in StoryManager!");
        if (scavengeDropdownManager == null) Debug.LogWarning("ScavengeDropdownManager is not assigned in StoryManager!");
        if (inventoryPanel == null) Debug.LogWarning("InventoryPanel is not assigned in StoryManager!"); // New warning

        if (bridgeTabToggle != null)
        {
            bridgeTabCanvasGroup = bridgeTabToggle.GetComponent<CanvasGroup>();
            if (bridgeTabCanvasGroup == null)
            {
                bridgeTabCanvasGroup = bridgeTabToggle.gameObject.AddComponent<CanvasGroup>();
            }
            bridgeTabCanvasGroup.alpha = 0f;
            bridgeTabToggle.gameObject.SetActive(true);
            bridgeTabToggle.interactable = false;
            Debug.Log("Initialized Bridge tab toggle (active but invisible).");
        }

        if (researchBayTabToggle != null)
        {
            researchBayTabCanvasGroup = researchBayTabToggle.GetComponent<CanvasGroup>();
            if (researchBayTabCanvasGroup == null)
            {
                researchBayTabCanvasGroup = researchBayTabToggle.gameObject.AddComponent<CanvasGroup>();
            }
            researchBayTabCanvasGroup.alpha = 0f;
            researchBayTabToggle.gameObject.SetActive(false);
            researchBayTabToggle.interactable = false;
            Debug.Log("Initialized Research Bay tab toggle (inactive).");
        }

        if (fabricatorTabToggle != null)
        {
            fabricatorTabCanvasGroup = fabricatorTabToggle.GetComponent<CanvasGroup>();
            if (fabricatorTabCanvasGroup == null)
            {
                fabricatorTabCanvasGroup = fabricatorTabToggle.gameObject.AddComponent<CanvasGroup>();
            }
            fabricatorTabCanvasGroup.alpha = 0f;
            fabricatorTabToggle.gameObject.SetActive(false);
            fabricatorTabToggle.interactable = false;
            Debug.Log("Initialized Fabricator tab toggle (inactive).");
        }

        if (barracksTabToggle != null)
        {
            barracksTabCanvasGroup = barracksTabToggle.GetComponent<CanvasGroup>();
            if (barracksTabCanvasGroup == null)
            {
                barracksTabCanvasGroup = barracksTabToggle.gameObject.AddComponent<CanvasGroup>();
            }
            barracksTabCanvasGroup.alpha = 0f;
            barracksTabToggle.gameObject.SetActive(false);
            barracksTabToggle.interactable = false;
            Debug.Log("Initialized Barracks tab toggle (inactive).");
        }

        if (darkPassagesTitleText != null)
        {
            darkPassagesCanvasGroup = darkPassagesTitleText.GetComponent<CanvasGroup>();
            if (darkPassagesCanvasGroup == null)
            {
                darkPassagesCanvasGroup = darkPassagesTitleText.gameObject.AddComponent<CanvasGroup>();
            }
            darkPassagesCanvasGroup.alpha = 0f;
            darkPassagesTitleText.gameObject.SetActive(false);
            Debug.Log("Initialized Dark Passages title text (hidden).");
        }

        if (actionTitleText != null)
        {
            actionTitleCanvasGroup = actionTitleText.GetComponent<CanvasGroup>();
            if (actionTitleCanvasGroup == null)
            {
                actionTitleCanvasGroup = actionTitleText.gameObject.AddComponent<CanvasGroup>();
            }
            actionTitleCanvasGroup.alpha = 0f;
            actionTitleText.gameObject.SetActive(false);
            Debug.Log("Initialized Action title text (hidden).");
        }

        if (restoreHealthButton != null)
        {
            restoreHealthCanvasGroup = restoreHealthButton.GetComponent<CanvasGroup>();
            if (restoreHealthCanvasGroup == null)
            {
                restoreHealthCanvasGroup = restoreHealthButton.gameObject.AddComponent<CanvasGroup>();
            }
            restoreHealthCanvasGroup.alpha = 0f;
            restoreHealthButton.gameObject.SetActive(false);
            restoreHealthButton.interactable = false;
            Debug.Log($"Initialized Restore Health button (hidden). Active: {restoreHealthButton.gameObject.activeSelf}, Alpha: {restoreHealthCanvasGroup.alpha}");
        }

        if (autoScavDropdown != null)
        {
            autoScavCanvasGroup = autoScavDropdown.GetComponent<CanvasGroup>();
            if (autoScavCanvasGroup == null)
            {
                autoScavCanvasGroup = autoScavDropdown.gameObject.AddComponent<CanvasGroup>();
            }
            autoScavCanvasGroup.alpha = 0f;
            autoScavDropdown.gameObject.SetActive(false);
            autoScavDropdown.interactable = false;
            Debug.Log($"Initialized Auto-SCAV dropdown (hidden). Active: {autoScavDropdown.gameObject.activeSelf}, Alpha: {autoScavCanvasGroup.alpha}");
        }

        if (settings != null)
        {
            settingsCanvasGroup = settings.GetComponent<CanvasGroup>();
            if (settingsCanvasGroup == null)
            {
                settingsCanvasGroup = settings.AddComponent<CanvasGroup>();
            }
            settingsCanvasGroup.alpha = 0f;
            settings.SetActive(false);
            Debug.Log($"Initialized Settings (hidden). Active: {settings.activeSelf}, Alpha: {settingsCanvasGroup.alpha}");
        }

        if (scavengeButton != null)
        {
            scavengeButtonCanvasGroup = scavengeButton.GetComponent<CanvasGroup>();
            if (scavengeButtonCanvasGroup == null)
            {
                scavengeButtonCanvasGroup = scavengeButton.gameObject.AddComponent<CanvasGroup>();
            }
            scavengeButtonCanvasGroup.alpha = 0f;
            scavengeButton.gameObject.SetActive(false);
            scavengeButton.interactable = false;
            Debug.Log("Initialized Scavenge button in StoryManager (hidden).");
        }

        // Initialize Inventory view (new)
        if (inventoryPanel != null)
        {
            inventoryCanvasGroup = inventoryPanel.GetComponent<CanvasGroup>();
            if (inventoryCanvasGroup == null)
            {
                inventoryCanvasGroup = inventoryPanel.AddComponent<CanvasGroup>();
            }
            inventoryCanvasGroup.alpha = 0f;
            inventoryPanel.SetActive(false);
            Debug.Log("Initialized Inventory panel (hidden).");
        }
    }

    public void OnOxygenRestart(int oxygenRestartCount)
    {
        if (bodyText == null)
        {
            Debug.LogWarning("BodyText is not assigned! Cannot update story messages.");
            return;
        }

        string newMessage = "Life support operational.";

        switch (oxygenRestartCount)
        {
            case 3:
                newMessage = "The lever is cold to the touch.";
                break;
            case 6:
                newMessage = "The system whirs and clatters.";
                break;
            case 8:
                newMessage = "There is a sound of rending metal.";
                break;
            case 10:
                newMessage = "You smell something burning.";
                break;
            case 11:
                newMessage = "Nothing happens.";
                break;
            case 12:
                newMessage = "Nothing happens.";
                break;
            case 15:
                newMessage = "You notice the nearby piles of scrap.";
                break;
            case 17:
                newMessage = "Maybe you can find something useful.";
                break;
            case 19:
                newMessage = "If you're quick.";
                if (scavengeButton != null && scavengeButtonCanvasGroup != null)
                {
                    scavengeButton.gameObject.SetActive(true);
                    StartCoroutine(FadeCanvasGroup(scavengeButtonCanvasGroup, 0f, 1f, 1f, () => scavengeButton.interactable = true));
                }
                if (scavengeDropdownManager != null)
                {
                    scavengeDropdownManager.ShowDropdown();
                }
                break;
        }

        string updatedText = newMessage;
        if (!string.IsNullOrEmpty(bodyText.text))
        {
            updatedText = newMessage + "\n" + bodyText.text;
        }

        bodyText.text = updatedText;
    }

    public void OnScavengeClicked()
    {
        if (!hasScavenged)
        {
            string newMessage = "You run for the nearest pile.";
            string updatedText = newMessage;
            if (!string.IsNullOrEmpty(bodyText.text))
            {
                updatedText = newMessage + "\n" + bodyText.text;
            }
            bodyText.text = updatedText;
            hasScavenged = true;
        }
        Debug.Log("Scavenge button clicked, story updated if necessary.");
    }

    public void OnScavengeCompleted()
    {
        OnScavengeCompleted("");
    }

    public void OnInventoryVisible()
    {
        Debug.Log("Inventory is now visible for the first time.");
    }

    public void OnScavengeCompleted(string scavengedResource)
    {
        if (bodyText == null)
        {
            Debug.LogWarning("BodyText is not assigned! Cannot update story messages.");
            return;
        }

        if (hasScavenged && !hasBridgeTabAppeared)
        {
            string newMessage = "This should come in handy.";
            string updatedText = newMessage;
            if (!string.IsNullOrEmpty(bodyText.text))
            {
                updatedText = newMessage + "\n" + bodyText.text;
            }
            bodyText.text = updatedText;
        }

        if (!hasBridgeTabAppeared && bridgeTabToggle != null && bridgeTabCanvasGroup != null)
        {
            StartCoroutine(FadeCanvasGroup(bridgeTabCanvasGroup, 0f, 1f, 1f, () => bridgeTabToggle.interactable = true));
            hasBridgeTabAppeared = true;

            if (ResourceTimerManager.instance != null && ResourceTimerManager.instance.exploreResearchBayButton != null)
            {
                Button exploreButton = ResourceTimerManager.instance.exploreResearchBayButton;
                CanvasGroup exploreCanvasGroup = exploreButton.GetComponent<CanvasGroup>();
                if (exploreCanvasGroup == null)
                {
                    exploreCanvasGroup = exploreButton.gameObject.AddComponent<CanvasGroup>();
                }
                exploreButton.gameObject.SetActive(true);
                StartCoroutine(FadeCanvasGroup(exploreCanvasGroup, 0f, 1f, 1f, () => exploreButton.interactable = true));
            }

            // Fade in inventory panel with a 3-second delay (new)
            if (inventoryPanel != null && inventoryCanvasGroup != null && !hasInventoryAppeared)
            {
                StartCoroutine(FadeInInventoryWithDelay(2f));
            }
        }

        Debug.Log($"Scavenge completed, story updated if necessary.");
    }

    // New coroutine for delayed inventory fade-in
    private IEnumerator FadeInInventoryWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        inventoryPanel.SetActive(true);
        StartCoroutine(FadeCanvasGroup(inventoryCanvasGroup, 0f, 1f, 1f, () =>
        {
            hasInventoryAppeared = true;
            OnInventoryVisible();
        }));
    }

    public void OnExploreResearchBayProgress(float progress)
    {
        if (bodyText == null)
        {
            Debug.LogWarning("BodyText is not assigned! Cannot update story messages.");
            return;
        }

        string newMessage = "";
        if (progress <= 0f)
        {
            newMessage = "You enter a dark room.";
        }
        else if (progress >= 0.25f && progress < 0.50f)
        {
            newMessage = "You knock your shin against something metal.";
        }
        else if (progress >= 0.50f && progress < 0.70f)
        {
            newMessage = "You limp on.";
        }
        else if (progress >= 0.70f && progress < 0.90f)
        {
            newMessage = "You locate a large lever and pull with all your might.";
        }
        else if (progress >= 0.90f)
        {
            newMessage = "It snaps into place. The room hums to life.";
        }

        if (!string.IsNullOrEmpty(newMessage))
        {
            string updatedText = newMessage;
            if (!string.IsNullOrEmpty(bodyText.text))
            {
                updatedText = newMessage + "\n" + bodyText.text;
            }
            bodyText.text = updatedText;
        }
    }

    public void OnExploreResearchBayCompleted()
    {
        if (researchBayTabToggle != null && researchBayTabCanvasGroup != null)
        {
            researchBayTabToggle.gameObject.SetActive(true);
            StartCoroutine(FadeCanvasGroup(researchBayTabCanvasGroup, 0f, 1f, 1f, () => researchBayTabToggle.interactable = true));
        }

        if (ResourceTimerManager.instance != null && ResourceTimerManager.instance.exploreResearchBayButton != null)
        {
            Button exploreButton = ResourceTimerManager.instance.exploreResearchBayButton;
            CanvasGroup exploreCanvasGroup = exploreButton.GetComponent<CanvasGroup>();
            if (exploreCanvasGroup != null)
            {
                StartCoroutine(FadeCanvasGroup(exploreCanvasGroup, 1f, 0f, 1f, () =>
                {
                    exploreButton.gameObject.SetActive(false);
                    exploreButton.interactable = false;
                }));
            }
        }

        if (ResourceTimerManager.instance != null)
        {
            ResourceTimerManager.instance.ShowResearchBayResearchButton();
        }

        if (researchBayDiscoveryWindow != null)
        {
            researchBayDiscoveryWindow.SetActive(true);
            if (ResourceTimerManager.instance != null)
            {
                ResourceTimerManager.instance.PauseTimers(true);
            }
        }
    }

    public void OnExploreFabricatorCompleted()
    {
        if (fabricatorTabToggle != null && fabricatorTabCanvasGroup != null)
        {
            fabricatorTabToggle.gameObject.SetActive(true);
            StartCoroutine(FadeCanvasGroup(fabricatorTabCanvasGroup, 0f, 1f, 1f, () => fabricatorTabToggle.interactable = true));
        }

        if (ResourceTimerManager.instance != null && ResourceTimerManager.instance.exploreFabricatorButton != null)
        {
            Button exploreButton = ResourceTimerManager.instance.exploreFabricatorButton;
            CanvasGroup exploreCanvasGroup = exploreButton.GetComponent<CanvasGroup>();
            if (exploreCanvasGroup != null)
            {
                StartCoroutine(FadeCanvasGroup(exploreCanvasGroup, 1f, 0f, 1f, () =>
                {
                    exploreButton.gameObject.SetActive(false);
                    exploreButton.interactable = false;
                }));
            }
        }

        if (ResourceTimerManager.instance != null)
        {
            ResourceTimerManager.instance.ShowFabricatorProcessButton();
        }

        if (fabricatorDiscoveryWindow != null)
        {
            fabricatorDiscoveryWindow.SetActive(true);
            if (ResourceTimerManager.instance != null)
            {
                ResourceTimerManager.instance.PauseTimers(true);
            }
        }
    }

    public void ShowRestoreHealthButton()
    {
        if (restoreHealthButton != null && restoreHealthCanvasGroup != null)
        {
            restoreHealthButton.gameObject.SetActive(true);
            StartCoroutine(FadeCanvasGroup(restoreHealthCanvasGroup, 0f, 1f, 1f, () => restoreHealthButton.interactable = true));
            Debug.Log("Restore Health button made visible and interactable.");
        }
    }

    public void ShowAutoScavDropdown()
    {
        if (autoScavDropdown != null && autoScavCanvasGroup != null)
        {
            autoScavDropdown.gameObject.SetActive(true);
            StartCoroutine(FadeCanvasGroup(autoScavCanvasGroup, 0f, 1f, 1f, () => autoScavDropdown.interactable = true));
            Debug.Log("Auto-SCAV dropdown made visible and interactable.");
        }
    }

    public void ShowSettings()
    {
        if (settings != null && settingsCanvasGroup != null)
        {
            settings.SetActive(true);
            StartCoroutine(FadeCanvasGroup(settingsCanvasGroup, 0f, 1f, 1f));
            Debug.Log("Settings panel made visible.");
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration, System.Action onComplete = null)
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
        onComplete?.Invoke();
    }

    public void SetUIInteractable(bool interactable)
    {
        if (scavengeButton != null) scavengeButton.interactable = interactable;
        if (autoScavDropdown != null) autoScavDropdown.interactable = interactable;
        if (restoreHealthButton != null) restoreHealthButton.interactable = interactable;
    }
}
}