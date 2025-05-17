using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using O2Game;

namespace O2Game
{
    public class ResearchBayManager : MonoBehaviour
    {
        [System.Serializable]
        public struct ResearchItem
        {
            public string name;                    // e.g., "Increase O2 Max"
            public float researchTime;             // Time to complete research in seconds
            public ResourceCost[] costs;           // Array of resource costs (same as InventoryManager)
            public Sprite upgradeIcon;             // Icon for the research upgrade
            public string description;             // Description of the research item
            public float oxygenMaxIncrease;        // Amount to increase Oxygen maxUnits by
        }

            [System.Serializable]
    public struct ResourceIcon
    {
        public string resourceName; // e.g., "Copper", "Steel"
        public Sprite iconSprite;   // The sprite for the resource icon
    }

    public ResearchItem[] researchItems;
    public ResourceIcon[] resourceIcons;
    public ToggleGroup researchToggleGroup;
    public GameObject researchItemToggleTemplate;
    public GameObject researchResourceCostTemplate;
    public GameObject researchResourceCostContainer;
    public TMP_Text activeResearchTimeText;
    public TMP_Text activeUpgradeDescriptionText;
    public Button researchButton;
    public ResourceTimerManager resourceTimerManager;
    public InventoryManager inventoryManager;

    public Sprite toggleOnSprite;
    public Sprite toggleOffSprite;

    private Dictionary<string, Sprite> resourceIconMap = new Dictionary<string, Sprite>();
    private List<GameObject> instantiatedToggles = new List<GameObject>();
    private List<GameObject> instantiatedResourceCosts = new List<GameObject>();
    private ResearchItem selectedItem;
    private bool isResearching = false;

    private void Awake()
    {
        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
            if (inventoryManager == null) Debug.LogWarning("InventoryManager not found in the scene!");
        }

        if (resourceTimerManager == null)
        {
            resourceTimerManager = FindObjectOfType<ResourceTimerManager>();
            if (resourceTimerManager == null) Debug.LogWarning("ResourceTimerManager not found in the scene!");
        }

        if (researchToggleGroup == null) Debug.LogWarning("ResearchToggleGroup is not assigned!");
        if (researchItemToggleTemplate == null) Debug.LogWarning("ResearchItemToggleTemplate is not assigned!");
        if (researchResourceCostTemplate == null) Debug.LogWarning("ResearchResourceCostTemplate is not assigned!");
        if (researchResourceCostContainer == null) Debug.LogWarning("ResearchResourceCostContainer is not assigned!");
        if (activeResearchTimeText == null) Debug.LogWarning("ActiveResearchTimeText is not assigned!");
        if (activeUpgradeDescriptionText == null) Debug.LogWarning("ActiveUpgradeDescriptionText is not assigned!");
        if (researchButton == null) Debug.LogWarning("ResearchButton is not assigned!");
        if (resourceIcons == null || resourceIcons.Length == 0) Debug.LogWarning("ResourceIcons array is empty!");
        if (toggleOnSprite == null) Debug.LogWarning("ToggleOnSprite is not assigned!");
        if (toggleOffSprite == null) Debug.LogWarning("ToggleOffSprite is not assigned!");

        if (researchToggleGroup != null) researchToggleGroup.allowSwitchOff = true;

        if (resourceIcons != null)
        {
            foreach (var resourceIcon in resourceIcons)
            {
                if (!string.IsNullOrEmpty(resourceIcon.resourceName) && resourceIcon.iconSprite != null)
                {
                    resourceIconMap[resourceIcon.resourceName] = resourceIcon.iconSprite;
                }
            }
        }

        if (researchItemToggleTemplate != null)
        {
            researchItemToggleTemplate.SetActive(false);
            Toggle templateToggle = researchItemToggleTemplate.GetComponent<Toggle>();
            if (templateToggle != null) templateToggle.transition = Selectable.Transition.None;
        }
        if (researchResourceCostTemplate != null) researchResourceCostTemplate.SetActive(false);

        if (researchButton != null)
        {
            researchButton.interactable = false;
            researchButton.onClick.AddListener(OnResearchButtonClicked);
        }

        if (activeResearchTimeText != null) activeResearchTimeText.text = "";
        if (activeUpgradeDescriptionText != null) activeUpgradeDescriptionText.text = "";

        PopulateResearchToggles();
    }

    public void OnTabOpened()
    {
        selectedItem = new ResearchItem();

        if (researchToggleGroup != null)
        {
            researchToggleGroup.SetAllTogglesOff();
            foreach (var toggleObj in instantiatedToggles)
            {
                Toggle toggle = toggleObj.GetComponent<Toggle>();
                if (toggle != null) UpdateToggleSprite(toggle);
            }
        }

        ClearResourceCosts();

        if (activeResearchTimeText != null) activeResearchTimeText.text = "";
        if (activeUpgradeDescriptionText != null) activeUpgradeDescriptionText.text = "";
        if (researchButton != null) researchButton.interactable = false;

        Debug.Log("Research Bay tab opened: No project selected, UI reset.");
    }

    private void PopulateResearchToggles()
    {
        if (researchToggleGroup == null || researchItemToggleTemplate == null)
        {
            Debug.LogWarning("Cannot populate research toggles: Required UI elements are not assigned.");
            return;
        }

        foreach (var toggle in instantiatedToggles)
        {
            if (toggle != null) Destroy(toggle);
        }
        instantiatedToggles.Clear();

        foreach (var item in researchItems)
        {
            GameObject newToggleObj = Instantiate(researchItemToggleTemplate, researchToggleGroup.transform);
            newToggleObj.SetActive(true);

            Toggle toggle = newToggleObj.GetComponent<Toggle>();
            if (toggle == null)
            {
                Debug.LogWarning($"Toggle component not found for {item.name}!");
                continue;
            }
            toggle.group = researchToggleGroup;

            Transform nameTextTransform = newToggleObj.transform.Find("NameText");
            Transform upgradeIconTransform = newToggleObj.transform.Find("Research upgrade icon");

            TMP_Text nameText = nameTextTransform?.GetComponent<TMP_Text>();
            Image upgradeIcon = upgradeIconTransform?.GetComponent<Image>();

            if (nameText == null)
            {
                Debug.LogWarning($"NameText not found for {item.name}!");
                continue;
            }
            if (upgradeIcon == null)
            {
                Debug.LogWarning($"Research upgrade icon not found for {item.name}!");
                continue;
            }

            nameText.text = item.name;
            upgradeIcon.sprite = item.upgradeIcon;

            UpdateToggleSprite(toggle);

            ResearchItem currentItem = item;
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    SelectResearchItem(currentItem);
                }
                else if (selectedItem.name == currentItem.name)
                {
                    selectedItem = new ResearchItem();
                    ClearResourceCosts();
                    if (activeResearchTimeText != null) activeResearchTimeText.text = "";
                    if (activeUpgradeDescriptionText != null) activeUpgradeDescriptionText.text = "";
                    if (researchButton != null) researchButton.interactable = false;
                }
                UpdateToggleSprite(toggle);
            });

            instantiatedToggles.Add(newToggleObj);
        }
    }

    private void UpdateToggleSprite(Toggle toggle)
    {
        if (toggle == null) return;

        Image toggleImage = toggle.GetComponent<Image>();
        if (toggleImage == null)
        {
            Debug.LogWarning("No Image component found on toggle for sprite swapping!");
            return;
        }

        toggleImage.sprite = toggle.isOn ? toggleOnSprite : toggleOffSprite;
    }

    private void SelectResearchItem(ResearchItem item)
    {
        selectedItem = item;

        if (activeResearchTimeText != null) activeResearchTimeText.text = $"{item.researchTime:F1}s";
        if (activeUpgradeDescriptionText != null) activeUpgradeDescriptionText.text = item.description;

        PopulateResourceCosts(item);
        UpdateResearchButton();

        Debug.Log($"Selected research item: {item.name}");
    }

    private string GetResourceName(ResourceCost cost)
    {
        if (cost.scavengeResourceType != ScavengeResourceType.None)
        {
            return cost.scavengeResourceType.ToString();
        }
        else if (cost.combatDropType != CombatDropType.None)
        {
            return cost.combatDropType.ToString();
        }
        else if (cost.fabricatorItemType != FabricatorItemType.None)
        {
            return cost.fabricatorItemType.ToString();
        }
        else
        {
            Debug.LogWarning("ResourceCost has no valid resource type defined.");
            return "Unknown";
        }
    }

    private void PopulateResourceCosts(ResearchItem item)
    {
        if (researchResourceCostContainer == null || researchResourceCostTemplate == null)
        {
            Debug.LogWarning("Cannot populate resource costs: Required UI elements are not assigned.");
            return;
        }

        ClearResourceCosts();

        foreach (var cost in item.costs)
        {
            string resourceName = GetResourceName(cost);

            GameObject costEntry = Instantiate(researchResourceCostTemplate, researchResourceCostContainer.transform);
            costEntry.SetActive(true);

            Transform quantityTextTransform = costEntry.transform.Find("QuantityText");
            Transform iconTransform = costEntry.transform.Find("Icon");
            Transform resourceNameTextTransform = costEntry.transform.Find("ResourceNameText");

            TMP_Text quantityText = quantityTextTransform?.GetComponent<TMP_Text>();
            Image iconImage = iconTransform?.GetComponent<Image>();
            TMP_Text resourceNameText = resourceNameTextTransform?.GetComponent<TMP_Text>();

            if (quantityText == null)
            {
                Debug.LogWarning($"QuantityText not found for resource {resourceName}!");
                continue;
            }
            if (iconImage == null)
            {
                Debug.LogWarning($"Icon Image not found for resource {resourceName}!");
                continue;
            }
            if (resourceNameText == null)
            {
                Debug.LogWarning($"ResourceNameText not found for resource {resourceName}!");
                continue;
            }

            quantityText.text = $"{cost.amount}x";
            resourceNameText.text = resourceName;

            if (resourceIconMap.ContainsKey(resourceName))
            {
                iconImage.sprite = resourceIconMap[resourceName];
            }
            else
            {
                Debug.LogWarning($"No icon found for resource {resourceName}!");
            }

            instantiatedResourceCosts.Add(costEntry);
        }

        if (researchResourceCostContainer != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(researchResourceCostContainer.GetComponent<RectTransform>());
            Debug.Log("Forced layout rebuild on researchResourceCostContainer.");
        }
    }

    private void ClearResourceCosts()
    {
        foreach (var costEntry in instantiatedResourceCosts)
        {
            if (costEntry != null) Destroy(costEntry);
        }
        instantiatedResourceCosts.Clear();
    }

    private void UpdateResearchButton()
    {
        if (researchButton == null) return;

        if (string.IsNullOrEmpty(selectedItem.name) || isResearching || !CanAffordResearch(selectedItem))
        {
            researchButton.interactable = false;
        }
        else
        {
            researchButton.interactable = true;
        }
    }

    private bool CanAffordResearch(ResearchItem item)
    {
        if (inventoryManager == null) return false;
        return inventoryManager.HasEnoughResources(new List<ResourceCost>(item.costs));
    }

    private void OnResearchButtonClicked()
    {
        if (CanAffordResearch(selectedItem))
        {
            inventoryManager.DeductResources(new List<ResourceCost>(selectedItem.costs));
            StartResearch(selectedItem);
        }
        else
        {
            Debug.Log("Not enough resources to start research.");
        }
    }

    private void StartResearch(ResearchItem item)
    {
        if (resourceTimerManager != null)
        {
            isResearching = true;
            resourceTimerManager.StartResearchBayResearch(item.researchTime, item.oxygenMaxIncrease, () =>
            {
                OnResearchCompleted();
            });
        }
        else
        {
            Debug.LogWarning("ResourceTimerManager is not assigned, cannot start research!");
        }
    }

    private void OnResearchCompleted()
    {
        isResearching = false;

        if (resourceTimerManager != null) resourceTimerManager.PauseTimers(false);

        UpdateResearchButton();

        Debug.Log($"Research completed: {selectedItem.name}");
    }
}
}