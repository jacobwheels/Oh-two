using UnityEngine;
using System.Collections;
using O2Game; // Added to reference GameConstants, ResourceManager, and InventoryManager

namespace O2Game // Added namespace
{
    public class ScavengeManager : MonoBehaviour
    {
        private InventoryManager inventoryManager;
        private ResourceManager resourceManager;

        private bool allTimersActive = false;

        private void Awake()
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
            resourceManager = FindObjectOfType<ResourceManager>();
        }

        public void Scavenge(ScavengeResourceType type)
        {
            if (type == ScavengeResourceType.None) return;

            if (!allTimersActive)
            {
                resourceManager.ActivateResourceTimer(ResourceType.Oxygen);
                resourceManager.ActivateResourceTimer(ResourceType.Heat);
                resourceManager.ActivateResourceTimer(ResourceType.Pressure);
                resourceManager.ActivateResourceTimer(ResourceType.Energy);
            }

            StartCoroutine(ScavengeCoroutine(type));
        }

        public void SetAllTimersActive(bool active)
        {
            allTimersActive = active;
        }

        private IEnumerator ScavengeCoroutine(ScavengeResourceType type)
        {
            float scavengeTime = GetScavengeTime(type);
            yield return new WaitForSeconds(scavengeTime);
            inventoryManager.AddScavengeResource(type, 1);
        }

        public float GetScavengeTime(ScavengeResourceType type)
        {
            float baseTime = 0f;
            switch (type)
            {
                case ScavengeResourceType.Copper:
                    baseTime = GameConstants.COPPER_SCAVENGE_TIME;
                    break;
                case ScavengeResourceType.Steel:
                    baseTime = GameConstants.STEEL_SCAVENGE_TIME;
                    break;
                case ScavengeResourceType.Glass:
                    baseTime = GameConstants.GLASS_SCAVENGE_TIME;
                    break;
                case ScavengeResourceType.Carbon:
                    baseTime = GameConstants.CARBON_SCAVENGE_TIME;
                    break;
                case ScavengeResourceType.Hydrogen:
                    baseTime = GameConstants.HYDROGEN_SCAVENGE_TIME;
                    break;
                case ScavengeResourceType.Titanium:
                    baseTime = GameConstants.TITANIUM_SCAVENGE_TIME;
                    break;
                case ScavengeResourceType.Uranium:
                    baseTime = GameConstants.URANIUM_SCAVENGE_TIME;
                    break;
            }
            return baseTime * resourceManager.GetExploreTimeMultiplier();
        }
    }
}