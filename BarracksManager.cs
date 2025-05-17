using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using O2Game; // Added to reference GameConstants, InventoryManager, and ResourceManager

namespace O2Game // Added namespace
{
    public class BarracksManager : MonoBehaviour
    {
        [System.Serializable]
        public struct BarracksRecipe
        {
            public BarracksItemType outputItem;
            public float baseTimeRequirement;
            public List<ResourceCost> inputCosts;
        }

        [SerializeField] private List<BarracksRecipe> recipes;
        private InventoryManager inventoryManager;
        private ResourceManager resourceManager;

        private void Awake()
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
            resourceManager = FindObjectOfType<ResourceManager>();

            // Initialize recipes based on Excel data
            recipes = new List<BarracksRecipe>
            {
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.SteelKnuckles,
                    baseTimeRequirement = 50f,
                    inputCosts = new List<ResourceCost> 
                    { 
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.SteelPlate, 
                            amount = 4, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        } 
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.RocketGloves,
                    baseTimeRequirement = 70f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.Hydrocarbon, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.SteelPlate, 
                            amount = 4, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.LaserSword,
                    baseTimeRequirement = 100f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.PowerCoreM, 
                            amount = 1, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.PlasmaConduit, 
                            amount = 1, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.CopperPlate, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.NeutronDagger,
                    baseTimeRequirement = 120f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.PowerCoreL, 
                            amount = 1, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.LiquidHydrogen, 
                            amount = 4, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.SteelPlate, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.ImplosionGauntlet,
                    baseTimeRequirement = 130f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.PowerCoreXL, 
                            amount = 1, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.U235, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.TitaniumPlate, 
                            amount = 4, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.Sling,
                    baseTimeRequirement = 70f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.CopperWire, 
                            amount = 3, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.TemperedGlass, 
                            amount = 3, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.GaussPistol,
                    baseTimeRequirement = 90f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.CarbonFiber, 
                            amount = 4, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.SteelPlate, 
                            amount = 3, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                // Completing the remaining recipes based on the pattern
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.LaserRifle,
                    baseTimeRequirement = 110f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.PlasmaConduit, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.TitaniumPlate, 
                            amount = 3, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.GravityCannon,
                    baseTimeRequirement = 130f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.GravitonEmitter, 
                            amount = 1, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.TitaniumPlate, 
                            amount = 4, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.AntimatterLauncher,
                    baseTimeRequirement = 150f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.U235, 
                            amount = 3, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.PlasmaConduit, 
                            amount = 3, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            combatDropType = CombatDropType.DarkMatterCatalyst, 
                            amount = 1, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            fabricatorItemType = FabricatorItemType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.Medkit,
                    baseTimeRequirement = 20f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.NutrientPaste, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.SyntheticMuscle, 
                            amount = 1, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.O2Canister,
                    baseTimeRequirement = 25f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.LiquidHydrogen, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.TitaniumPlate, 
                            amount = 1, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.PortableFriction,
                    baseTimeRequirement = 30f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.Hydrocarbon, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.CopperPlate, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.ReadyVac,
                    baseTimeRequirement = 35f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.TitaniumPlate, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.Nanobots, 
                            amount = 1, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.FieldRation,
                    baseTimeRequirement = 20f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.NutrientPaste, 
                            amount = 3, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.IED,
                    baseTimeRequirement = 40f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.Hydrocarbon, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.CopperWire, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.PlasmaGrenade,
                    baseTimeRequirement = 60f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.PlasmaConduit, 
                            amount = 1, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.TitaniumPlate, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.WristRocket,
                    baseTimeRequirement = 80f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.Hydrocarbon, 
                            amount = 3, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.Nanobots, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.LocalizedFission,
                    baseTimeRequirement = 100f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.U235, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.TitaniumPlate, 
                            amount = 3, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.CannedBlackHole,
                    baseTimeRequirement = 120f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.GravitonEmitter, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            combatDropType = CombatDropType.DarkMatterCatalyst, 
                            amount = 1, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            fabricatorItemType = FabricatorItemType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.Crawler,
                    baseTimeRequirement = 90f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.Nanobots, 
                            amount = 3, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.TitaniumPlate, 
                            amount = 4, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.Grunt,
                    baseTimeRequirement = 110f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.SyntheticMuscle, 
                            amount = 3, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.Nanobots, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.SamurAI,
                    baseTimeRequirement = 130f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.Nanobots, 
                            amount = 4, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.PlasmaConduit, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new BarracksRecipe
                {
                    outputItem = BarracksItemType.Mech,
                    baseTimeRequirement = 150f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.SyntheticMuscle, 
                            amount = 5, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.PowerCoreXL, 
                            amount = 1, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            combatDropType = CombatDropType.DarkMatterCatalyst, 
                            amount = 1, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            fabricatorItemType = FabricatorItemType.None 
                        }
                    }
                }
            };
        }

        public void Craft(BarracksItemType type)
        {
            BarracksRecipe recipe = recipes.Find(r => r.outputItem == type);

            if (!inventoryManager.HasEnoughResources(recipe.inputCosts))
            {
                Debug.LogWarning($"Not enough resources to craft {type}");
                return;
            }

            StartCoroutine(CraftCoroutine(recipe));
        }

        private IEnumerator CraftCoroutine(BarracksRecipe recipe)
        {
            yield return new WaitForSeconds(recipe.baseTimeRequirement);
            inventoryManager.DeductResources(recipe.inputCosts);
            inventoryManager.AddBarracksItem(recipe.outputItem, 1);

            // If the item is a consumable, apply its effect
            if (IsConsumable(recipe.outputItem))
            {
                resourceManager.UseConsumable(recipe.outputItem);
            }
        }

        private bool IsConsumable(BarracksItemType type)
        {
            return type == BarracksItemType.Medkit ||
                   type == BarracksItemType.O2Canister ||
                   type == BarracksItemType.PortableFriction ||
                   type == BarracksItemType.ReadyVac ||
                   type == BarracksItemType.FieldRation;
        }
    }
}