using UnityEngine;
using System.Collections.Generic;
using O2Game; // Added to reference GameConstants and ScavengeManager

namespace O2Game // Added namespace
{
    public class ResourceManager : MonoBehaviour
    {
        [System.Serializable]
        public struct ResourceData
        {
            public ResourceType type;
            public float currentValue;
            public float maxValue;
            public float resetTime;
            public float consumptionRate;
            public float timer;
            public bool isActive;
        }

        [SerializeField] private List<ResourceData> resources = new List<ResourceData>();

        private void Awake()
        {
            resources.Add(new ResourceData
            {
                type = ResourceType.Oxygen,
                currentValue = GameConstants.OXYGEN_BASE_MAX,
                maxValue = GameConstants.OXYGEN_BASE_MAX,
                resetTime = GameConstants.OXYGEN_BASE_RESET_TIME,
                consumptionRate = GameConstants.OXYGEN_BASE_CONSUMPTION_RATE,
                timer = 0f,
                isActive = false
            });

            resources.Add(new ResourceData
            {
                type = ResourceType.Heat,
                currentValue = GameConstants.HEAT_BASE_MAX,
                maxValue = GameConstants.HEAT_BASE_MAX,
                resetTime = GameConstants.HEAT_BASE_RESET_TIME,
                consumptionRate = GameConstants.HEAT_BASE_CONSUMPTION_RATE,
                timer = 0f,
                isActive = false
            });

            resources.Add(new ResourceData
            {
                type = ResourceType.Pressure,
                currentValue = GameConstants.PRESSURE_BASE_MAX,
                maxValue = GameConstants.PRESSURE_BASE_MAX,
                resetTime = GameConstants.PRESSURE_BASE_RESET_TIME,
                consumptionRate = GameConstants.PRESSURE_BASE_CONSUMPTION_RATE,
                timer = 0f,
                isActive = false
            });

            resources.Add(new ResourceData
            {
                type = ResourceType.Energy,
                currentValue = GameConstants.ENERGY_BASE_MAX,
                maxValue = GameConstants.ENERGY_BASE_MAX,
                resetTime = GameConstants.ENERGY_BASE_RESET_TIME,
                consumptionRate = GameConstants.ENERGY_BASE_CONSUMPTION_RATE,
                timer = 0f,
                isActive = false
            });
        }

        private void Update()
        {
            bool allActive = true;
            foreach (var resource in resources)
            {
                if (!resource.isActive)
                {
                    allActive = false;
                    continue;
                }

                ResourceData updatedResource = resource;
                updatedResource.timer += Time.deltaTime;

                if (updatedResource.timer >= updatedResource.resetTime)
                {
                    updatedResource.currentValue = Mathf.Min(
                        updatedResource.currentValue + (updatedResource.maxValue / (updatedResource.maxValue / updatedResource.consumptionRate)),
                        updatedResource.maxValue
                    );
                    updatedResource.timer = 0f;
                }

                updatedResource.currentValue = Mathf.Max(updatedResource.currentValue - (updatedResource.consumptionRate * Time.deltaTime), 0f);
                resources[resources.IndexOf(resource)] = updatedResource;

                if (updatedResource.currentValue <= 0f)
                {
                    Debug.LogError($"Game Over! {updatedResource.type} depleted!");
                    Time.timeScale = 0f; // Pause the game for simplicity
                }
            }

            if (allActive)
            {
                FindObjectOfType<ScavengeManager>()?.SetAllTimersActive(true);
            }
        }

        public void ActivateResourceTimer(ResourceType type)
        {
            int index = resources.FindIndex(r => r.type == type);
            if (index >= 0)
            {
                ResourceData updatedResource = resources[index];
                updatedResource.isActive = true;
                resources[index] = updatedResource;
            }
        }

        public void UseConsumable(BarracksItemType item)
        {
            switch (item)
            {
                case BarracksItemType.Medkit:
                    AdjustResourceValue(ResourceType.Oxygen, 20f);
                    break;
                case BarracksItemType.O2Canister:
                    AdjustResourceValue(ResourceType.Oxygen, 50f);
                    break;
                case BarracksItemType.PortableFriction:
                    AdjustResourceValue(ResourceType.Heat, 30f);
                    break;
                case BarracksItemType.ReadyVac:
                    AdjustResourceValue(ResourceType.Pressure, -30f);
                    break;
                case BarracksItemType.FieldRation:
                    AdjustResourceValue(ResourceType.Energy, 40f);
                    break;
            }
        }

        private void AdjustResourceValue(ResourceType type, float amount)
        {
            int index = resources.FindIndex(r => r.type == type);
            if (index >= 0)
            {
                ResourceData updatedResource = resources[index];
                updatedResource.currentValue = Mathf.Clamp(
                    updatedResource.currentValue + amount,
                    0f,
                    updatedResource.maxValue
                );
                resources[index] = updatedResource;
                Debug.Log($"{type} adjusted by {amount}. New value: {updatedResource.currentValue}");
            }
        }

        public float GetExploreTimeMultiplier()
        {
            float oxygenMultiplier = resources.Find(r => r.type == ResourceType.Oxygen).currentValue / GameConstants.OXYGEN_BASE_MAX;
            float heatMultiplier = resources.Find(r => r.type == ResourceType.Heat).currentValue / GameConstants.HEAT_BASE_MAX;
            float pressureMultiplier = resources.Find(r => r.type == ResourceType.Pressure).currentValue / GameConstants.PRESSURE_BASE_MAX;
            float energyMultiplier = resources.Find(r => r.type == ResourceType.Energy).currentValue / GameConstants.ENERGY_BASE_MAX;

            return (oxygenMultiplier + heatMultiplier + pressureMultiplier + energyMultiplier) / 4f;
        }
    }
}