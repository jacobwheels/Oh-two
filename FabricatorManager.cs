using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using O2Game; // Added to reference GameConstants and InventoryManager

namespace O2Game // Added namespace
{
    public class FabricatorManager : MonoBehaviour
    {
        [System.Serializable]
        public struct FabricationRecipe
        {
            public FabricatorItemType outputItem;
            public float baseTimeRequirement;
            public List<ResourceCost> inputCosts;
        }

        [SerializeField] private List<FabricationRecipe> recipes;
        private InventoryManager inventoryManager;

        private void Awake()
        {
            inventoryManager = FindObjectOfType<InventoryManager>();

            // Initialize recipes based on Excel data
            recipes = new List<FabricationRecipe>
            {
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.CopperPlate,
                    baseTimeRequirement = 10f,
                    inputCosts = new List<ResourceCost> 
                    { 
                        new ResourceCost 
                        { 
                            scavengeResourceType = ScavengeResourceType.Copper, 
                            amount = 2, 
                            fabricatorItemType = FabricatorItemType.None, 
                            combatDropType = CombatDropType.None 
                        } 
                    }
                },
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.CopperWire,
                    baseTimeRequirement = 15f,
                    inputCosts = new List<ResourceCost> 
                    { 
                        new ResourceCost 
                        { 
                            scavengeResourceType = ScavengeResourceType.Copper, 
                            amount = 3, 
                            fabricatorItemType = FabricatorItemType.None, 
                            combatDropType = CombatDropType.None 
                        } 
                    }
                },
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.SteelPlate,
                    baseTimeRequirement = 20f,
                    inputCosts = new List<ResourceCost> 
                    { 
                        new ResourceCost 
                        { 
                            scavengeResourceType = ScavengeResourceType.Steel, 
                            amount = 3, 
                            fabricatorItemType = FabricatorItemType.None, 
                            combatDropType = CombatDropType.None 
                        } 
                    }
                },
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.TemperedGlass,
                    baseTimeRequirement = 25f,
                    inputCosts = new List<ResourceCost> 
                    { 
                        new ResourceCost 
                        { 
                            scavengeResourceType = ScavengeResourceType.Glass, 
                            amount = 3, 
                            fabricatorItemType = FabricatorItemType.None, 
                            combatDropType = CombatDropType.None 
                        } 
                    }
                },
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.GrapheneSheet,
                    baseTimeRequirement = 30f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            scavengeResourceType = ScavengeResourceType.Carbon, 
                            amount = 5, 
                            fabricatorItemType = FabricatorItemType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            scavengeResourceType = ScavengeResourceType.Hydrogen, 
                            amount = 2, 
                            fabricatorItemType = FabricatorItemType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.NutrientPaste,
                    baseTimeRequirement = 20f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            scavengeResourceType = ScavengeResourceType.Carbon, 
                            amount = 3, 
                            fabricatorItemType = FabricatorItemType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.CarbonFiber,
                    baseTimeRequirement = 35f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            scavengeResourceType = ScavengeResourceType.Carbon, 
                            amount = 5, 
                            fabricatorItemType = FabricatorItemType.None, 
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
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.LiquidHydrogen,
                    baseTimeRequirement = 40f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            scavengeResourceType = ScavengeResourceType.Hydrogen, 
                            amount = 5, 
                            fabricatorItemType = FabricatorItemType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.TitaniumPlate,
                    baseTimeRequirement = 50f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            scavengeResourceType = ScavengeResourceType.Titanium, 
                            amount = 5, 
                            fabricatorItemType = FabricatorItemType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.U235,
                    baseTimeRequirement = 60f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            scavengeResourceType = ScavengeResourceType.Uranium, 
                            amount = 5, 
                            fabricatorItemType = FabricatorItemType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.Hydrocarbon,
                    baseTimeRequirement = 45f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            scavengeResourceType = ScavengeResourceType.Hydrogen, 
                            amount = 3, 
                            fabricatorItemType = FabricatorItemType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            scavengeResourceType = ScavengeResourceType.Carbon, 
                            amount = 3, 
                            fabricatorItemType = FabricatorItemType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.Nanobots,
                    baseTimeRequirement = 70f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            scavengeResourceType = ScavengeResourceType.Titanium, 
                            amount = 5, 
                            fabricatorItemType = FabricatorItemType.None, 
                            combatDropType = CombatDropType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.CopperWire, 
                            amount = 3, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.PlasmaConduit,
                    baseTimeRequirement = 80f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            combatDropType = CombatDropType.PlasmaShard, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            fabricatorItemType = FabricatorItemType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.CopperWire, 
                            amount = 3, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.AblativeCoating,
                    baseTimeRequirement = 90f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            combatDropType = CombatDropType.BiosteelShard, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            fabricatorItemType = FabricatorItemType.None 
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
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.SyntheticMuscle,
                    baseTimeRequirement = 100f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            combatDropType = CombatDropType.BiosteelShard, 
                            amount = 3, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            fabricatorItemType = FabricatorItemType.None 
                        },
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.CarbonFiber, 
                            amount = 5, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                // Completing the remaining recipes based on the pattern
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.GravitonEmitter,
                    baseTimeRequirement = 110f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            combatDropType = CombatDropType.NeutronShard, 
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            fabricatorItemType = FabricatorItemType.None 
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
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.PowerCoreS,
                    baseTimeRequirement = 60f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.U235, 
                            amount = 1, 
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
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.PowerCoreM,
                    baseTimeRequirement = 80f,
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
                            fabricatorItemType = FabricatorItemType.CopperWire, 
                            amount = 3, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.PowerCoreL,
                    baseTimeRequirement = 100f,
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
                            amount = 2, 
                            scavengeResourceType = ScavengeResourceType.None, 
                            combatDropType = CombatDropType.None 
                        }
                    }
                },
                new FabricationRecipe
                {
                    outputItem = FabricatorItemType.PowerCoreXL,
                    baseTimeRequirement = 120f,
                    inputCosts = new List<ResourceCost>
                    {
                        new ResourceCost 
                        { 
                            fabricatorItemType = FabricatorItemType.U235, 
                            amount = 4, 
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
                }
            };
        }

        public void Fabricate(FabricatorItemType type)
        {
            FabricationRecipe recipe = recipes.Find(r => r.outputItem == type);
            if (!inventoryManager.IsFabricatedItemDiscovered(type))
            {
                Debug.LogWarning($"Cannot fabricate {type}: item not yet discovered.");
                return;
            }

            if (!inventoryManager.HasEnoughResources(recipe.inputCosts))
            {
                Debug.LogWarning($"Not enough resources to fabricate {type}");
                return;
            }

            StartCoroutine(FabricateCoroutine(recipe));
        }

        public void UnlockFabricatedItem(FabricatorItemType type)
        {
            inventoryManager.UnlockFabricatedItem(type);
        }

        private IEnumerator FabricateCoroutine(FabricationRecipe recipe)
        {
            yield return new WaitForSeconds(recipe.baseTimeRequirement);
            inventoryManager.DeductResources(recipe.inputCosts);
            inventoryManager.AddFabricatedItem(recipe.outputItem, 1);
        }
    }
}