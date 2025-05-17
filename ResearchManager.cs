using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using O2Game; // Added to reference GameConstants, InventoryManager, and FabricatorManager

namespace O2Game // Added namespace
{
    [CreateAssetMenu(fileName = "NewResearchProject", menuName = "Research/Project")]
    public class ResearchProject : ScriptableObject
    {
        public string projectName;
        public float baseTimeRequirement;
        public List<ResourceCost> inputCosts;
        public string[] prerequisites;
        public FabricatorItemType unlocksFabricatedItem;
    }

    public class ResearchManager : MonoBehaviour
    {
        [SerializeField] private List<ResearchProject> availableProjects;
        private InventoryManager inventoryManager;
        private FabricatorManager fabricatorManager;
        private List<string> completedProjects = new List<string>();

        private void Awake()
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
            fabricatorManager = FindObjectOfType<FabricatorManager>();

            // Initialize available projects based on Excel data
            availableProjects = new List<ResearchProject>();

            // Project 1: Copper Plate
            ResearchProject copperPlateProject = ScriptableObject.CreateInstance<ResearchProject>();
            copperPlateProject.projectName = "Copper Plate Fabrication";
            copperPlateProject.baseTimeRequirement = 30f;
            copperPlateProject.inputCosts = new List<ResourceCost>
            {
                new ResourceCost { scavengeResourceType = ScavengeResourceType.Copper, amount = 3, fabricatorItemType = FabricatorItemType.None, combatDropType = CombatDropType.None }
            };
            copperPlateProject.prerequisites = new string[] { };
            copperPlateProject.unlocksFabricatedItem = FabricatorItemType.CopperPlate;
            availableProjects.Add(copperPlateProject);

            // Project 2: Copper Wire
            ResearchProject copperWireProject = ScriptableObject.CreateInstance<ResearchProject>();
            copperWireProject.projectName = "Copper Wire Fabrication";
            copperWireProject.baseTimeRequirement = 40f;
            copperWireProject.inputCosts = new List<ResourceCost>
            {
                new ResourceCost { scavengeResourceType = ScavengeResourceType.Copper, amount = 5, fabricatorItemType = FabricatorItemType.None, combatDropType = CombatDropType.None }
            };
            copperWireProject.prerequisites = new string[] { "Copper Plate Fabrication" };
            copperWireProject.unlocksFabricatedItem = FabricatorItemType.CopperWire;
            availableProjects.Add(copperWireProject);

            // Project 3: Steel Plate
            ResearchProject steelPlateProject = ScriptableObject.CreateInstance<ResearchProject>();
            steelPlateProject.projectName = "Steel Plate Fabrication";
            steelPlateProject.baseTimeRequirement = 50f;
            steelPlateProject.inputCosts = new List<ResourceCost>
            {
                new ResourceCost { scavengeResourceType = ScavengeResourceType.Steel, amount = 5, fabricatorItemType = FabricatorItemType.None, combatDropType = CombatDropType.None }
            };
            steelPlateProject.prerequisites = new string[] { }; // Fixed: Was incorrectly using copperPlateProject
            steelPlateProject.unlocksFabricatedItem = FabricatorItemType.SteelPlate; // Fixed: Was incorrectly using copperPlateProject
            availableProjects.Add(steelPlateProject);

            // Project 4: Tempered Glass
            ResearchProject temperedGlassProject = ScriptableObject.CreateInstance<ResearchProject>();
            temperedGlassProject.projectName = "Tempered Glass Fabrication";
            temperedGlassProject.baseTimeRequirement = 60f;
            temperedGlassProject.inputCosts = new List<ResourceCost>
            {
                new ResourceCost { scavengeResourceType = ScavengeResourceType.Glass, amount = 5, fabricatorItemType = FabricatorItemType.None, combatDropType = CombatDropType.None }
            };
            temperedGlassProject.prerequisites = new string[] { "Copper Plate Fabrication", "Steel Plate Fabrication" }; // Fixed: Was incorrectly using copperPlateProject
            temperedGlassProject.unlocksFabricatedItem = FabricatorItemType.TemperedGlass; // Fixed: Was incorrectly using copperPlateProject
            availableProjects.Add(temperedGlassProject);

            // Add more projects as needed...
        }

        public bool CanResearch(string projectName)
        {
            ResearchProject project = availableProjects.Find(p => p.projectName == projectName);
            if (project == null) return false;

            // Check if all prerequisites are completed
            if (project.prerequisites.Length > 0) // Added check to avoid exception with empty arrays
            {
                string lastPrereq = project.prerequisites.Last();
                if (!completedProjects.Contains(lastPrereq))
                {
                    Debug.LogWarning($"Cannot research {projectName}: prerequisite {lastPrereq} not completed.");
                    return false;
                }
            }

            // Check if the project unlocks a fabricated item that is already discovered
            if (project.unlocksFabricatedItem != FabricatorItemType.None && 
                inventoryManager.IsFabricatedItemDiscovered(project.unlocksFabricatedItem))
            {
                Debug.LogWarning($"Cannot research {projectName}: item {project.unlocksFabricatedItem} already discovered.");
                return false;
            }

            // Check if the project is already completed
            if (completedProjects.Contains(projectName))
            {
                Debug.LogWarning($"Cannot research {projectName}: project already completed.");
                return false;
            }

            // Check if resources are available
            if (!inventoryManager.HasEnoughResources(project.inputCosts))
            {
                Debug.LogWarning($"Cannot research {projectName}: not enough resources.");
                return false;
            }

            return true;
        }

        public void StartResearch(string projectName)
        {
            ResearchProject project = availableProjects.Find(p => p.projectName == projectName);
            if (project == null)
            {
                Debug.LogError($"Project {projectName} not found!");
                return;
            }

            if (!CanResearch(projectName))
            {
                return;
            }

            StartCoroutine(ResearchCoroutine(project));
        }

        private System.Collections.IEnumerator ResearchCoroutine(ResearchProject project)
        {
            yield return new WaitForSeconds(project.baseTimeRequirement);
            inventoryManager.DeductResources(project.inputCosts);
            completedProjects.Add(project.projectName);
            if (project.unlocksFabricatedItem != FabricatorItemType.None)
            {
                fabricatorManager.UnlockFabricatedItem(project.unlocksFabricatedItem);
            }
            Debug.Log($"Completed research: {project.projectName}");
        }

        public bool IsProjectCompleted(string projectName) => completedProjects.Contains(projectName);
    }
}