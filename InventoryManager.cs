using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;

namespace O2Game
{
    [System.Serializable]
    public struct ResourceCost {
        public ScavengeResourceType scavengeResourceType;
        public CombatDropType combatDropType;
        public FabricatorItemType fabricatorItemType;
        public BarracksItemType barracksItemType; 
        public int amount;
    }
    [System.Serializable]
public struct ResourceSprite
{
    public object resourceType;
    public Sprite sprite;
}

[System.Serializable]
public struct InventoryItemTemplate
{
    public TMP_Text nameText;
    public TMP_Text quantityText;
    public Image icon;
}

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventoryItemTemplate;
    [SerializeField] private Transform inventoryGridGroup;
    [SerializeField] private InventoryItemTemplate inventoryItemTemplateComponents;
    [SerializeField] private ResourceSprite[] resourceSprites;

    private Dictionary<ScavengeResourceType, int> scavengeResources = new Dictionary<ScavengeResourceType, int>();
    private Dictionary<CombatDropType, int> combatDrops = new Dictionary<CombatDropType, int>();
    private Dictionary<FabricatorItemType, int> fabricatedItems = new Dictionary<FabricatorItemType, int>();
    private Dictionary<BarracksItemType, int> barracksItems = new Dictionary<BarracksItemType, int>();

    private HashSet<ScavengeResourceType> discoveredScavengeResources = new HashSet<ScavengeResourceType>();
    private HashSet<CombatDropType> discoveredCombatDrops = new HashSet<CombatDropType>();
    private HashSet<FabricatorItemType> discoveredFabricatedItems = new HashSet<FabricatorItemType>();
    private HashSet<BarracksItemType> discoveredBarracksItems = new HashSet<BarracksItemType>();

    private Dictionary<object, GameObject> inventoryItems = new Dictionary<object, GameObject>();
    private Dictionary<object, InventoryItemTemplate> inventoryItemComponents = new Dictionary<object, InventoryItemTemplate>();

    private void Awake()
    {
        foreach (ScavengeResourceType type in System.Enum.GetValues(typeof(ScavengeResourceType)))
        {
            scavengeResources[type] = 0;
            if (type == ScavengeResourceType.Copper || type == ScavengeResourceType.Steel)
            {
                discoveredScavengeResources.Add(type);
            }
        }
        foreach (CombatDropType type in System.Enum.GetValues(typeof(CombatDropType)))
        {
            combatDrops[type] = 0;
        }
        foreach (FabricatorItemType type in System.Enum.GetValues(typeof(FabricatorItemType)))
        {
            fabricatedItems[type] = 0;
        }
        foreach (BarracksItemType type in System.Enum.GetValues(typeof(BarracksItemType)))
        {
            barracksItems[type] = 0;
        }

        InitializeInventoryUI();
    }

    private void InitializeInventoryUI()
    {
        if (inventoryPanel == null)
        {
            Debug.LogWarning("InventoryPanel not assigned in InventoryManager!");
            return;
        }
        if (inventoryItemTemplate == null)
        {
            Debug.LogWarning("InventoryItemTemplate not assigned in InventoryManager!");
            return;
        }
        if (inventoryGridGroup == null)
        {
            Debug.LogWarning("InventoryGridGroup not assigned in InventoryManager!");
            return;
        }
        if (inventoryItemTemplateComponents.nameText == null || inventoryItemTemplateComponents.quantityText == null || inventoryItemTemplateComponents.icon == null)
        {
            Debug.LogWarning("InventoryItemTemplate components (NameText, QuantityText, or Icon) not assigned in InventoryManager!");
            return;
        }

        inventoryItemTemplate.SetActive(false);

        InitializeItemsForType<ScavengeResourceType>(scavengeResources, discoveredScavengeResources);
        InitializeItemsForType<CombatDropType>(combatDrops, discoveredCombatDrops);
        InitializeItemsForType<FabricatorItemType>(fabricatedItems, discoveredFabricatedItems);
        InitializeItemsForType<BarracksItemType>(barracksItems, discoveredBarracksItems);

        Debug.Log("Inventory UI initialized.");
    }

    private void InitializeItemsForType<T>(Dictionary<T, int> items, HashSet<T> discoveredItems) where T : Enum
    {
        foreach (T type in System.Enum.GetValues(typeof(T)))
        {
            GameObject item = Instantiate(inventoryItemTemplate, inventoryGridGroup);
            inventoryItems[type] = item;

            InventoryItemTemplate components;
            components.nameText = FindMatchingTMPText(item, inventoryItemTemplateComponents.nameText);
            components.quantityText = FindMatchingTMPText(item, inventoryItemTemplateComponents.quantityText);
            components.icon = FindMatchingImage(item, inventoryItemTemplateComponents.icon);

            if (components.nameText == null || components.quantityText == null || components.icon == null)
            {
                Debug.LogWarning($"Failed to find NameText, QuantityText, or Icon on instantiated item for {type}!");
                item.SetActive(false);
                continue;
            }

            inventoryItemComponents[type] = components;

            string resourceName = type.ToString();
            string quantity = items[type] + "x";
            components.nameText.text = resourceName;
            components.quantityText.text = quantity;
            Debug.Log($"Initialized {type}: NameText set to '{resourceName}', QuantityText set to '{quantity}', Parent={inventoryGridGroup.name}, ParentActive={inventoryGridGroup.gameObject.activeInHierarchy}, ItemActive={item.activeSelf}");

            Sprite sprite = GetSpriteForResource(type);
            if (sprite != null)
            {
                components.icon.sprite = sprite;
            }
            else
            {
                Debug.LogWarning($"No sprite assigned for {type} in InventoryManager!");
                components.icon.gameObject.SetActive(false);
            }

            bool shouldBeActive = items[type] > 0 && discoveredItems.Contains(type);
            Debug.Log($"Initial visibility for {type}: Quantity={items[type]}, IsDiscovered={discoveredItems.Contains(type)}, ShouldBeActive={shouldBeActive}");
            item.SetActive(shouldBeActive);
        }
    }

    private TMP_Text FindMatchingTMPText(GameObject item, TMP_Text templateText)
    {
        TMP_Text[] allTexts = item.GetComponentsInChildren<TMP_Text>(true);
        string allNames = string.Join(", ", allTexts.Select(t => t.name));
        Debug.Log($"Searching for TMP_Text '{templateText.name}' in {item.name}. Found: {allNames}");
        foreach (TMP_Text text in allTexts)
        {
            if (text.name == templateText.name)
            {
                return text;
            }
        }
        Debug.LogWarning($"No matching TMP_Text found for template {templateText.name} in {item.name}");
        return null;
    }

    private Image FindMatchingImage(GameObject item, Image templateImage)
    {
        Image[] allImages = item.GetComponentsInChildren<Image>(true);
        string allNames = string.Join(", ", allImages.Select(i => i.name));
        Debug.Log($"Searching for Image '{templateImage.name}' in {item.name}. Found: {allNames}");
        foreach (Image image in allImages)
        {
            if (image.name == templateImage.name)
            {
                return image;
            }
        }
        Debug.LogWarning($"No matching Image found for template {templateImage.name} in {item.name}");
        return null;
    }

    private Sprite GetSpriteForResource(object type)
    {
        foreach (var resourceSprite in resourceSprites)
        {
            if (resourceSprite.resourceType.Equals(type))
            {
                return resourceSprite.sprite;
            }
        }
        return null;
    }

    public void AddScavengeResource(ScavengeResourceType type, int amount)
    {
        scavengeResources[type] += amount;
        discoveredScavengeResources.Add(type);
        Debug.Log($"Added {amount} {type}. Total: {scavengeResources[type]}");
        UpdateInventoryItemUI(type);
    }

    public void AddCombatDrop(CombatDropType type, int amount)
    {
        combatDrops[type] += amount;
        discoveredCombatDrops.Add(type);
        Debug.Log($"Added {amount} {type}. Total: {combatDrops[type]}");
        UpdateInventoryItemUI(type);
    }

    public void AddFabricatedItem(FabricatorItemType type, int amount)
    {
        fabricatedItems[type] += amount;
        discoveredFabricatedItems.Add(type);
        Debug.Log($"Added {amount} {type}. Total: {fabricatedItems[type]}");
        UpdateInventoryItemUI(type);
    }

    public void AddBarracksItem(BarracksItemType type, int amount)
    {
        barracksItems[type] += amount;
        discoveredBarracksItems.Add(type);
        Debug.Log($"Added {amount} {type}. Total: {barracksItems[type]}");
        UpdateInventoryItemUI(type);
    }

    public void UnlockFabricatedItem(FabricatorItemType type)
    {
        discoveredFabricatedItems.Add(type);
        Debug.Log($"Unlocked {type} for fabrication.");
    }

    public bool HasEnoughResources(List<ResourceCost> costs)
    {
        foreach (var cost in costs)
        {
            if (cost.scavengeResourceType != ScavengeResourceType.None && (!scavengeResources.ContainsKey(cost.scavengeResourceType) || scavengeResources[cost.scavengeResourceType] < cost.amount))
            {
                return false;
            }
            if (cost.combatDropType != CombatDropType.None && (!combatDrops.ContainsKey(cost.combatDropType) || combatDrops[cost.combatDropType] < cost.amount))
            {
                return false;
            }
            if (cost.fabricatorItemType != FabricatorItemType.None && (!fabricatedItems.ContainsKey(cost.fabricatorItemType) || fabricatedItems[cost.fabricatorItemType] < cost.amount))
            {
                return false;
            }
            if (cost.barracksItemType != BarracksItemType.None && (!barracksItems.ContainsKey(cost.barracksItemType) || barracksItems[cost.barracksItemType] < cost.amount))
            {
                return false;
            }
        }
        return true;
    }

    public void DeductResources(List<ResourceCost> costs)
    {
        foreach (var cost in costs)
        {
            if (cost.scavengeResourceType != ScavengeResourceType.None)
            {
                scavengeResources[cost.scavengeResourceType] -= cost.amount;
                Debug.Log($"Deducted {cost.amount} {cost.scavengeResourceType}");
                UpdateInventoryItemUI(cost.scavengeResourceType);
            }
            if (cost.combatDropType != CombatDropType.None)
            {
                combatDrops[cost.combatDropType] -= cost.amount;
                Debug.Log($"Deducted {cost.amount} {cost.combatDropType}");
                UpdateInventoryItemUI(cost.combatDropType);
            }
            if (cost.fabricatorItemType != FabricatorItemType.None)
            {
                fabricatedItems[cost.fabricatorItemType] -= cost.amount;
                Debug.Log($"Deducted {cost.amount} {cost.fabricatorItemType}");
                UpdateInventoryItemUI(cost.fabricatorItemType);
            }
            if (cost.barracksItemType != BarracksItemType.None)
            {
                barracksItems[cost.barracksItemType] -= cost.amount;
                Debug.Log($"Deducted {cost.amount} {cost.barracksItemType}");
                UpdateInventoryItemUI(cost.barracksItemType);
            }
        }
    }

    private void UpdateInventoryItemUI(object type)
    {
        if (inventoryItemComponents.ContainsKey(type))
        {
            InventoryItemTemplate components = inventoryItemComponents[type];
            if (components.quantityText != null && components.nameText != null)
            {
                string resourceName = type.ToString();
                int quantity = GetQuantityForType(type);
                string quantityText = quantity + "x";
                components.nameText.text = resourceName;
                components.quantityText.text = quantityText;
                bool isDiscovered = IsDiscovered(type);
                bool shouldBeActive = quantity > 0 && isDiscovered;
                Debug.Log($"Updating UI for {type}: Quantity={quantity}, IsDiscovered={isDiscovered}, ShouldBeActive={shouldBeActive}, PanelActive={inventoryPanel.activeSelf}, ItemActive={inventoryItems[type].activeSelf}");
                inventoryItems[type].SetActive(shouldBeActive);
                Debug.Log($"Updated UI for {type}: NameText='{resourceName}', QuantityText='{quantityText}'");
            }
            else
            {
                Debug.LogWarning($"NameText or QuantityText not found for {type} in inventory item!");
            }
        }
        else
        {
            Debug.LogWarning($"No UI components found for {type} in inventory!");
        }
    }

    private int GetQuantityForType(object type)
    {
        if (type is ScavengeResourceType srt) return scavengeResources.ContainsKey(srt) ? scavengeResources[srt] : 0;
        if (type is CombatDropType cdt) return combatDrops.ContainsKey(cdt) ? combatDrops[cdt] : 0;
        if (type is FabricatorItemType fit) return fabricatedItems.ContainsKey(fit) ? fabricatedItems[fit] : 0;
        if (type is BarracksItemType bit) return barracksItems.ContainsKey(bit) ? barracksItems[bit] : 0;
        return 0;
    }

    private bool IsDiscovered(object type)
    {
        if (type is ScavengeResourceType srt) return discoveredScavengeResources.Contains(srt);
        if (type is CombatDropType cdt) return discoveredCombatDrops.Contains(cdt);
        if (type is FabricatorItemType fit) return discoveredFabricatedItems.Contains(fit);
        if (type is BarracksItemType bit) return discoveredBarracksItems.Contains(bit);
        return false;
    }

    public bool IsScavengeResourceDiscovered(ScavengeResourceType type) => discoveredScavengeResources.Contains(type);
    public bool IsCombatDropDiscovered(CombatDropType type) => discoveredCombatDrops.Contains(type);
    public bool IsFabricatedItemDiscovered(FabricatorItemType type) => discoveredFabricatedItems.Contains(type);
    public bool IsBarracksItemDiscovered(BarracksItemType type) => discoveredBarracksItems.Contains(type);
    public int GetScavengeResourceAmount(ScavengeResourceType type) => scavengeResources[type];
    public int GetCombatDropAmount(CombatDropType type) => combatDrops[type];
    public int GetFabricatedItemAmount(FabricatorItemType type) => fabricatedItems[type];
    public int GetBarracksItemAmount(BarracksItemType type) => barracksItems[type];
}
}