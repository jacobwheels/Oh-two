// ResourceTimerManager.cs
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace O2Game
{
    public class ResourceTimerManager : MonoBehaviour
    {
        public static ResourceTimerManager instance;

                [System.Serializable]
        public struct ResourceTimer
        {
            public string name;
            public float maxUnits;
            public float currentUnits;
            public float consumptionRate;
            public TMP_Text displayText;
            public GameObject timerObject;
            [HideInInspector] public bool wasActive;
            [HideInInspector] public CanvasGroup timerCanvasGroup;
        }

        [System.Serializable]
        public struct ResearchProject
        {
            public string projectId; // Unique identifier for the project
            public ResourceCost[] costs; // Array of resource costs (same as ResearchBayManager)
            public float duration; // Duration of the research in seconds
            public float oxygenMaxIncrease; // Amount to increase Oxygen maxUnits by
        }

        public ResourceTimer[] timers = new ResourceTimer[4];

        public Button oxygenResetButton;
        public Button heatResetButton;
        public Button pressureResetButton;
        public Button energyResetButton;

        public Image oxygenCooldownBar;
        public Image heatCooldownBar;
        public Image pressureCooldownBar;
        public Image energyCooldownBar;

        public TMP_Text oxygenCooldownText;
        public TMP_Text heatCooldownText;
        public TMP_Text pressureCooldownText;
        public TMP_Text energyCooldownText;

        private CanvasGroup oxygenButtonCanvasGroup;
        private CanvasGroup heatButtonCanvasGroup;
        private CanvasGroup pressureButtonCanvasGroup;
        private CanvasGroup energyButtonCanvasGroup;

        public Button exploreFabricatorButton;
        public Button exploreResearchBayButton;
        public Button exploreBarracksButton;
        public Button fabricatorProcessButton;
        public Button researchBayResearchButton;
        public Button barracksOrderButton;
        public Button sector1Button;
        public Button sector2Button;
        public Button sector3Button;
        public Button sector4Button;
        public Button sector5Button;
        public Button sector6Button;
        public Button sector7Button;
        public Button sector8Button;
        public Button sector9Button;
        public Button sector10Button;
        public Button sector99Button;
        public Button sectorUnknownButton;

        public Button scavengeButton;
        public Image scavengeCooldownBar;
        private CanvasGroup scavengeButtonCanvasGroup;

        public ScavengeDropdownManager scavengeDropdownManager;
        public InventoryManager inventoryManager;

        public Image exploreFabricatorCooldownBar;
        public Image exploreResearchBayCooldownBar;
        public Image exploreBarracksCooldownBar;
        public Image fabricatorProcessCooldownBar;
        public Image researchBayResearchCooldownBar;
        public Image barracksOrderCooldownBar;
        public Image sector1CooldownBar;
        public Image sector2CooldownBar;
        public Image sector3CooldownBar;
        public Image sector4CooldownBar;
        public Image sector5CooldownBar;
        public Image sector6CooldownBar;
        public Image sector7CooldownBar;
        public Image sector8CooldownBar;
        public Image sector9CooldownBar;
        public Image sector10CooldownBar;
        public Image sector99CooldownBar;
        public Image sectorUnknownCooldownBar;

        public TMP_Text exploreFabricatorCooldownText;
        [SerializeField] private float exploreFabricatorCooldownDuration = 10f;
        private float lastExploreFabricatorCooldownDuration;

        public TMP_Text exploreResearchBayCooldownText;
        [SerializeField] private float exploreResearchBayCooldownDuration = 10f;
        private float lastExploreResearchBayCooldownDuration;

        private CanvasGroup exploreFabricatorButtonCanvasGroup;
        private CanvasGroup exploreResearchBayButtonCanvasGroup;
        private CanvasGroup exploreBarracksButtonCanvasGroup;
        private CanvasGroup fabricatorProcessButtonCanvasGroup;
        private CanvasGroup researchBayResearchButtonCanvasGroup;
        private CanvasGroup barracksOrderButtonCanvasGroup;
        private CanvasGroup sector1ButtonCanvasGroup;
        private CanvasGroup sector2ButtonCanvasGroup;
        private CanvasGroup sector3ButtonCanvasGroup;
        private CanvasGroup sector4ButtonCanvasGroup;
        private CanvasGroup sector5ButtonCanvasGroup;
        private CanvasGroup sector6ButtonCanvasGroup;
        private CanvasGroup sector7ButtonCanvasGroup;
        private CanvasGroup sector8ButtonCanvasGroup;
        private CanvasGroup sector9ButtonCanvasGroup;
        private CanvasGroup sector10ButtonCanvasGroup;
        private CanvasGroup sector99ButtonCanvasGroup;
        private CanvasGroup sectorUnknownButtonCanvasGroup;

        public GameObject gameOverOverlay;
        public TMP_Text gameOverMessageText;
        private bool isGameOver = false;

        private int oxygenRestartCount = 0;
        private float globalCooldownTimer = 0f;
        private bool isOnGlobalCooldown = false;

        [SerializeField] private float oxygenCooldownDuration = 3f;
        [SerializeField] private float heatCooldownDuration = 2f;
        [SerializeField] private float pressureCooldownDuration = 2f;
        [SerializeField] private float energyCooldownDuration = 2f;

        private float lastOxygenCooldownDuration;
        private float lastHeatCooldownDuration;
        private float lastPressureCooldownDuration;
        private float lastEnergyCooldownDuration;

        private float currentCooldownDuration = 0f;
        private string lastScavengedResource = "";
        private string lastAction = "";

        // Pausing state
        private bool isPaused = false;

        // Progress tracking for Explore Research Bay
        private float lastReportedProgress = -1f; // To avoid spamming messages

        // Delegate for research completion callback
        public delegate void ResearchCompletedCallback();
        private ResearchCompletedCallback researchCallback;

        public StoryManager storyManager;

        // New Research Timer fields
        public GameObject researchTimerObject; // Parent GameObject for the Research Timer UI
        public TMP_Text researchTimerText; // Text component to display the Research Timer
        private CanvasGroup researchTimerCanvasGroup; // For fading the Research Timer in
        private bool isResearchTimerVisible = false; // Track if the timer has been shown
        private bool isResearchActive = false; // Track if research is currently active
        private float researchTimer = 0f; // Current research timer value
        private float researchDuration = 0f; // Total duration of the current research

        public ResearchProject currentResearchProject; // The research project to start

        private void Awake()
        {
            instance = this;
            InitializeTimers();
            AssignButtonEvents();

            lastOxygenCooldownDuration = oxygenCooldownDuration;
            lastHeatCooldownDuration = heatCooldownDuration;
            lastPressureCooldownDuration = pressureCooldownDuration;
            lastEnergyCooldownDuration = energyCooldownDuration;
            lastExploreFabricatorCooldownDuration = exploreFabricatorCooldownDuration;
            lastExploreResearchBayCooldownDuration = exploreResearchBayCooldownDuration;
        }

        private void InitializeTimers()
        {
            for (int i = 0; i < timers.Length; i++)
            {
                if (timers[i].displayText == null)
                {
                    Debug.LogError($"DisplayText for timer {timers[i].name} is not assigned!");
                    continue;
                }

                if (timers[i].timerObject == null)
                {
                    Debug.LogError($"TimerObject for timer {timers[i].name} is not assigned!");
                    continue;
                }

                ResourceTimer timer = timers[i];
                timer.timerCanvasGroup = timer.timerObject.GetComponent<CanvasGroup>();
                if (timer.timerCanvasGroup == null)
                {
                    timer.timerCanvasGroup = timer.timerObject.AddComponent<CanvasGroup>();
                }

                timer.currentUnits = timer.maxUnits;

                if (i == 0)
                {
                    timer.wasActive = true;
                    timer.timerObject.SetActive(true);
                    timer.timerCanvasGroup.alpha = 1f;
                }
                else
                {
                    timer.wasActive = false;
                    timer.timerCanvasGroup.alpha = 0f;
                    timer.timerObject.SetActive(false);
                }

                timers[i] = timer;
                UpdateTimerDisplay(i);
            }

            if (gameOverOverlay != null)
            {
                gameOverOverlay.SetActive(false);
            }
            else
            {
                Debug.LogWarning("GameOverOverlay is not assigned in ResourceTimerManager!");
            }

            if (gameOverMessageText == null)
            {
                Debug.LogWarning("GameOverMessageText is not assigned in ResourceTimerManager! Attempting to find it on GameOverOverlay.");
                if (gameOverOverlay != null)
                {
                    gameOverMessageText = gameOverOverlay.GetComponentInChildren<TMP_Text>();
                }
            }

            if (oxygenCooldownBar == null) Debug.LogWarning("OxygenCooldownBar is not assigned in ResourceTimerManager!");
            if (heatCooldownBar == null) Debug.LogWarning("HeatCooldownBar is not assigned in ResourceTimerManager!");
            if (pressureCooldownBar == null) Debug.LogWarning("PressureCooldownBar is not assigned in ResourceTimerManager!");
            if (energyCooldownBar == null) Debug.LogWarning("EnergyCooldownBar is not assigned in ResourceTimerManager!");

            if (exploreFabricatorCooldownBar == null) Debug.LogWarning("ExploreFabricatorCooldownBar is not assigned in ResourceTimerManager!");
            if (exploreResearchBayCooldownBar == null) Debug.LogWarning("ExploreResearchBayCooldownBar is not assigned in ResourceTimerManager!");
            if (exploreBarracksCooldownBar == null) Debug.LogWarning("ExploreBarracksCooldownBar is not assigned in ResourceTimerManager!");
            if (fabricatorProcessCooldownBar == null) Debug.LogWarning("FabricatorProcessCooldownBar is not assigned in ResourceTimerManager!");
            if (researchBayResearchCooldownBar == null) Debug.LogWarning("ResearchBayResearchCooldownBar is not assigned in ResourceTimerManager!");
            if (barracksOrderCooldownBar == null) Debug.LogWarning("BarracksOrderCooldownBar is not assigned in ResourceTimerManager!");
            if (sector1CooldownBar == null) Debug.LogWarning("Sector1CooldownBar is not assigned in ResourceTimerManager!");
            if (sector2CooldownBar == null) Debug.LogWarning("Sector2CooldownBar is not assigned in ResourceTimerManager!");
            if (sector3CooldownBar == null) Debug.LogWarning("Sector3CooldownBar is not assigned in ResourceTimerManager!");
            if (sector4CooldownBar == null) Debug.LogWarning("Sector4CooldownBar is not assigned in ResourceTimerManager!");
            if (sector5CooldownBar == null) Debug.LogWarning("Sector5CooldownBar is not assigned in ResourceTimerManager!");
            if (sector6CooldownBar == null) Debug.LogWarning("Sector6CooldownBar is not assigned in ResourceTimerManager!");
            if (sector7CooldownBar == null) Debug.LogWarning("Sector7CooldownBar is not assigned in ResourceTimerManager!");
            if (sector8CooldownBar == null) Debug.LogWarning("Sector8CooldownBar is not assigned in ResourceTimerManager!");
            if (sector9CooldownBar == null) Debug.LogWarning("Sector9CooldownBar is not assigned in ResourceTimerManager!");
            if (sector10CooldownBar == null) Debug.LogWarning("Sector10CooldownBar is not assigned in ResourceTimerManager!");
            if (sector99CooldownBar == null) Debug.LogWarning("Sector99CooldownBar is not assigned in ResourceTimerManager!");
            if (sectorUnknownCooldownBar == null) Debug.LogWarning("SectorUnknownCooldownBar is not assigned in ResourceTimerManager!");
            if (scavengeCooldownBar == null) Debug.LogWarning("ScavengeCooldownBar is not assigned in ResourceTimerManager!");

            if (oxygenCooldownText == null) Debug.LogWarning("OxygenCooldownText is not assigned in ResourceTimerManager!");
            if (heatCooldownText == null) Debug.LogWarning("HeatCooldownText is not assigned in ResourceTimerManager!");
            if (pressureCooldownText == null) Debug.LogWarning("PressureCooldownText is not assigned in ResourceTimerManager!");
            if (energyCooldownText == null) Debug.LogWarning("EnergyCooldownText is not assigned in ResourceTimerManager!");
            if (exploreFabricatorCooldownText == null) Debug.LogWarning("ExploreFabricatorCooldownText is not assigned in ResourceTimerManager!");
            if (exploreResearchBayCooldownText == null) Debug.LogWarning("ExploreResearchBayCooldownText is not assigned in ResourceTimerManager!");

            if (scavengeButton == null) Debug.LogWarning("ScavengeButton is not assigned in ResourceTimerManager!");
            if (scavengeDropdownManager == null) Debug.LogWarning("ScavengeDropdownManager is not assigned in ResourceTimerManager!");
            if (inventoryManager == null) Debug.LogWarning("InventoryManager is not assigned in ResourceTimerManager!");
            if (storyManager == null) Debug.LogWarning("StoryManager is not assigned in ResourceTimerManager!");

            // Initialize Research Timer
            if (researchTimerObject == null)
            {
                Debug.LogWarning("ResearchTimerObject is not assigned in ResourceTimerManager!");
            }
            else
            {
                researchTimerCanvasGroup = researchTimerObject.GetComponent<CanvasGroup>();
                if (researchTimerCanvasGroup == null)
                {
                    researchTimerCanvasGroup = researchTimerObject.AddComponent<CanvasGroup>();
                }
                researchTimerCanvasGroup.alpha = 0f;
                researchTimerObject.SetActive(false);
            }

            if (researchTimerText == null)
            {
                Debug.LogWarning("ResearchTimerText is not assigned in ResourceTimerManager!");
            }
            else
            {
                researchTimerText.text = "-"; // Initial state
            }

            SetCooldownBarActive(oxygenCooldownBar, false);
            SetCooldownBarActive(heatCooldownBar, false);
            SetCooldownBarActive(pressureCooldownBar, false);
            SetCooldownBarActive(energyCooldownBar, false);

            SetCooldownBarActive(exploreFabricatorCooldownBar, false);
            SetCooldownBarActive(exploreResearchBayCooldownBar, false);
            SetCooldownBarActive(exploreBarracksCooldownBar, false);
            SetCooldownBarActive(fabricatorProcessCooldownBar, false);
            SetCooldownBarActive(researchBayResearchCooldownBar, false);
            SetCooldownBarActive(barracksOrderCooldownBar, false);
            SetCooldownBarActive(sector1CooldownBar, false);
            SetCooldownBarActive(sector2CooldownBar, false);
            SetCooldownBarActive(sector3CooldownBar, false);
            SetCooldownBarActive(sector4CooldownBar, false);
            SetCooldownBarActive(sector5CooldownBar, false);
            SetCooldownBarActive(sector6CooldownBar, false);
            SetCooldownBarActive(sector7CooldownBar, false);
            SetCooldownBarActive(sector8CooldownBar, false);
            SetCooldownBarActive(sector9CooldownBar, false);
            SetCooldownBarActive(sector10CooldownBar, false);
            SetCooldownBarActive(sector99CooldownBar, false);
            SetCooldownBarActive(sectorUnknownCooldownBar, false);
            SetCooldownBarActive(scavengeCooldownBar, false);

            UpdateCooldownText(oxygenCooldownText, oxygenCooldownDuration);
            UpdateCooldownText(heatCooldownText, heatCooldownDuration);
            UpdateCooldownText(pressureCooldownText, pressureCooldownDuration);
            UpdateCooldownText(energyCooldownText, energyCooldownDuration);
            UpdateCooldownText(exploreFabricatorCooldownText, exploreFabricatorCooldownDuration);
            UpdateCooldownText(exploreResearchBayCooldownText, exploreResearchBayCooldownDuration);

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
            }
        }

        private void AssignButtonEvents()
        {
            if (oxygenResetButton != null)
            {
                oxygenResetButton.onClick.RemoveAllListeners();
                oxygenResetButton.onClick.AddListener(() => ResetTimer(0));
                oxygenButtonCanvasGroup = oxygenResetButton.GetComponent<CanvasGroup>();
                if (oxygenButtonCanvasGroup == null)
                {
                    oxygenButtonCanvasGroup = oxygenResetButton.gameObject.AddComponent<CanvasGroup>();
                }
                oxygenButtonCanvasGroup.alpha = timers[0].timerObject.activeInHierarchy ? 1f : 0f;
                oxygenResetButton.gameObject.SetActive(timers[0].timerObject.activeInHierarchy);
                oxygenResetButton.interactable = true;
                Debug.Log("Assigned OnClick event to Oxygen reset button.");
            }
            else
            {
                Debug.LogWarning("Oxygen reset button is not assigned in ResourceTimerManager!");
            }

            if (heatResetButton != null)
            {
                heatResetButton.onClick.RemoveAllListeners();
                heatResetButton.onClick.AddListener(() => ResetTimer(1));
                heatButtonCanvasGroup = heatResetButton.GetComponent<CanvasGroup>();
                if (heatButtonCanvasGroup == null)
                {
                    heatButtonCanvasGroup = heatResetButton.gameObject.AddComponent<CanvasGroup>();
                }
                heatButtonCanvasGroup.alpha = 0f;
                heatResetButton.gameObject.SetActive(false);
                heatResetButton.interactable = false;
                Debug.Log("Initialized Heat reset button (hidden).");
            }
            else
            {
                Debug.LogWarning("Heat reset button is not assigned in ResourceTimerManager!");
            }

            if (pressureResetButton != null)
            {
                pressureResetButton.onClick.RemoveAllListeners();
                pressureResetButton.onClick.AddListener(() => ResetTimer(2));
                pressureButtonCanvasGroup = pressureResetButton.GetComponent<CanvasGroup>();
                if (pressureButtonCanvasGroup == null)
                {
                    pressureButtonCanvasGroup = pressureResetButton.gameObject.AddComponent<CanvasGroup>();
                }
                pressureButtonCanvasGroup.alpha = 0f;
                pressureResetButton.gameObject.SetActive(false);
                pressureResetButton.interactable = false;
                Debug.Log("Initialized Pressure reset button (hidden).");
            }
            else
            {
                Debug.LogWarning("Pressure reset button is not assigned in ResourceTimerManager!");
            }

            if (energyResetButton != null)
            {
                energyResetButton.onClick.RemoveAllListeners();
                energyResetButton.onClick.AddListener(() => ResetTimer(3));
                energyButtonCanvasGroup = energyResetButton.GetComponent<CanvasGroup>();
                if (energyButtonCanvasGroup == null)
                {
                    energyButtonCanvasGroup = energyResetButton.gameObject.AddComponent<CanvasGroup>();
                }
                energyButtonCanvasGroup.alpha = 0f;
                energyResetButton.gameObject.SetActive(false);
                energyResetButton.interactable = false;
                Debug.Log("Initialized Energy reset button (hidden).");
            }
            else
            {
                Debug.LogWarning("Energy reset button is not assigned in ResourceTimerManager!");
            }

            if (scavengeButton != null)
            {
                UnityEngine.Events.UnityAction scavengeAction = () =>
                {
                    if (isOnGlobalCooldown)
                    {
                        Debug.Log("Cannot scavenge: Global cooldown is active!");
                        return;
                    }

                    if (scavengeDropdownManager != null)
                    {
                        string selectedResource = scavengeDropdownManager.GetSelectedOption();
                        float cooldownDuration = scavengeDropdownManager.GetSelectedOptionCooldown();
                        Debug.Log($"Scavenging for {selectedResource} with a cooldown of {cooldownDuration}s...");

                        lastScavengedResource = selectedResource;
                        lastAction = "Scavenge";

                        if (storyManager != null)
                        {
                            storyManager.OnScavengeClicked();
                        }

                        StartGlobalCooldown(cooldownDuration);
                    }
                };

                scavengeButton.onClick.RemoveAllListeners();
                scavengeButton.onClick.AddListener(scavengeAction);
                scavengeButtonCanvasGroup = scavengeButton.GetComponent<CanvasGroup>();
                if (scavengeButtonCanvasGroup == null)
                {
                    scavengeButtonCanvasGroup = scavengeButton.gameObject.AddComponent<CanvasGroup>();
                }
                scavengeButtonCanvasGroup.alpha = 0f;
                scavengeButton.gameObject.SetActive(false);
                scavengeButton.interactable = false;
                Debug.Log("Initialized Scavenge button (hidden).");
            }

            if (exploreFabricatorButton != null)
            {
                exploreFabricatorButton.onClick.RemoveAllListeners();
                exploreFabricatorButton.onClick.AddListener(() =>
                {
                    if (isOnGlobalCooldown)
                    {
                        Debug.Log("Cannot explore Fabricator: Global cooldown is active!");
                        return;
                    }
                    lastAction = "ExploreFabricator";
                    StartGlobalCooldown(exploreFabricatorCooldownDuration);
                    Debug.Log("Exploring Fabricator...");
                });
                exploreFabricatorButtonCanvasGroup = exploreFabricatorButton.GetComponent<CanvasGroup>();
                if (exploreFabricatorButtonCanvasGroup == null)
                {
                    exploreFabricatorButtonCanvasGroup = exploreFabricatorButton.gameObject.AddComponent<CanvasGroup>();
                }
                exploreFabricatorButtonCanvasGroup.alpha = 0f;
                exploreFabricatorButton.gameObject.SetActive(false);
                exploreFabricatorButton.interactable = false;
                Debug.Log("Initialized Explore Fabricator button (hidden).");
            }
            else
            {
                Debug.LogWarning("Explore Fabricator button is not assigned in ResourceTimerManager!");
            }

            if (exploreResearchBayButton != null)
            {
                exploreResearchBayButton.onClick.RemoveAllListeners();
                exploreResearchBayButton.onClick.AddListener(() =>
                {
                    if (isOnGlobalCooldown)
                    {
                        Debug.Log("Cannot explore Research Bay: Global cooldown is active!");
                        return;
                    }
                    lastAction = "ExploreResearchBay";
                    StartGlobalCooldown(exploreResearchBayCooldownDuration);
                    Debug.Log("Exploring Research Bay...");
                    if (storyManager != null)
                    {
                        storyManager.OnExploreResearchBayProgress(0f);
                    }
                });
                exploreResearchBayButtonCanvasGroup = exploreResearchBayButton.GetComponent<CanvasGroup>();
                if (exploreResearchBayButtonCanvasGroup == null)
                {
                    exploreResearchBayButtonCanvasGroup = exploreResearchBayButton.gameObject.AddComponent<CanvasGroup>();
                }
                exploreResearchBayButtonCanvasGroup.alpha = 0f;
                exploreResearchBayButton.gameObject.SetActive(false);
                exploreResearchBayButton.interactable = false;
                Debug.Log("Initialized Explore Research Bay button (hidden).");
            }
            else
            {
                Debug.LogWarning("Explore Research Bay button is not assigned in ResourceTimerManager!");
            }

            if (exploreBarracksButton != null)
            {
                exploreBarracksButton.onClick.RemoveAllListeners();
                exploreBarracksButtonCanvasGroup = exploreBarracksButton.GetComponent<CanvasGroup>();
                if (exploreBarracksButtonCanvasGroup == null)
                {
                    exploreBarracksButtonCanvasGroup = exploreBarracksButton.gameObject.AddComponent<CanvasGroup>();
                }
                exploreBarracksButtonCanvasGroup.alpha = 0f;
                exploreBarracksButton.gameObject.SetActive(false);
                exploreBarracksButton.interactable = false;
                Debug.Log("Initialized Explore Barracks button (hidden).");
            }
            else
            {
                Debug.LogWarning("Explore Barracks button is not assigned in ResourceTimerManager!");
            }

            if (fabricatorProcessButton != null)
            {
                fabricatorProcessButton.onClick.RemoveAllListeners();
                fabricatorProcessButtonCanvasGroup = fabricatorProcessButton.GetComponent<CanvasGroup>();
                if (fabricatorProcessButtonCanvasGroup == null)
                {
                    fabricatorProcessButtonCanvasGroup = fabricatorProcessButton.gameObject.AddComponent<CanvasGroup>();
                }
                fabricatorProcessButtonCanvasGroup.alpha = 0f;
                fabricatorProcessButton.gameObject.SetActive(false);
                fabricatorProcessButton.interactable = false;
                Debug.Log("Initialized Fabricator Process button (hidden).");
            }
            else
            {
                Debug.LogWarning("Fabricator Process button is not assigned in ResourceTimerManager!");
            }

            if (researchBayResearchButton != null)
            {
                researchBayResearchButton.onClick.RemoveAllListeners();
                researchBayResearchButton.onClick.AddListener(() =>
                {
                    if (isResearchActive)
                    {
                        Debug.Log("Cannot start new research: Research Bay is currently active!");
                        return;
                    }

                    // Check if the player has enough resources for the project
                    bool hasEnoughResources = true;
                    if (inventoryManager != null)
                    {
                        hasEnoughResources = inventoryManager.HasEnoughResources(new List<ResourceCost>(currentResearchProject.costs));
                    }
                    else
                    {
                        Debug.LogWarning("InventoryManager is not assigned in ResourceTimerManager!");
                        hasEnoughResources = false;
                    }

                    if (!hasEnoughResources)
                    {
                        Debug.LogWarning($"Not enough resources to start research project: {currentResearchProject.projectId}");
                        return;
                    }

                    // Deduct the required resources
                    if (inventoryManager != null)
                    {
                        inventoryManager.DeductResources(new List<ResourceCost>(currentResearchProject.costs));
                        Debug.Log($"Resources deducted for Research Project: {currentResearchProject.projectId}");
                    }

                    // Start the research with the project's duration and oxygen max increase
                    StartResearchBayResearch(currentResearchProject.duration, currentResearchProject.oxygenMaxIncrease, () =>
                    {
                        Debug.Log($"Research project {currentResearchProject.projectId} completed!");
                    });
                });
                researchBayResearchButtonCanvasGroup = researchBayResearchButton.GetComponent<CanvasGroup>();
                if (researchBayResearchButtonCanvasGroup == null)
                {
                    researchBayResearchButtonCanvasGroup = researchBayResearchButton.gameObject.AddComponent<CanvasGroup>();
                }
                researchBayResearchButtonCanvasGroup.alpha = 0f;
                researchBayResearchButton.gameObject.SetActive(false);
                researchBayResearchButton.interactable = false;
                Debug.Log("Initialized Research Bay Research button (hidden).");
            }
            else
            {
                Debug.LogWarning("Research Bay Research button is not assigned in ResourceTimerManager!");
            }

            if (barracksOrderButton != null)
            {
                barracksOrderButton.onClick.RemoveAllListeners();
                barracksOrderButtonCanvasGroup = barracksOrderButton.GetComponent<CanvasGroup>();
                if (barracksOrderButtonCanvasGroup == null)
                {
                    barracksOrderButtonCanvasGroup = barracksOrderButton.gameObject.AddComponent<CanvasGroup>();
                }
                barracksOrderButtonCanvasGroup.alpha = 0f;
                barracksOrderButton.gameObject.SetActive(false);
                barracksOrderButton.interactable = false;
                Debug.Log("Initialized Barracks Order button (hidden).");
            }
            else
            {
                Debug.LogWarning("Barracks Order button is not assigned in ResourceTimerManager!");
            }

            if (sector1Button != null)
            {
                sector1Button.onClick.RemoveAllListeners();
                sector1ButtonCanvasGroup = sector1Button.GetComponent<CanvasGroup>();
                if (sector1ButtonCanvasGroup == null)
                {
                    sector1ButtonCanvasGroup = sector1Button.gameObject.AddComponent<CanvasGroup>();
                }
                sector1ButtonCanvasGroup.alpha = 0f;
                sector1Button.gameObject.SetActive(false);
                sector1Button.interactable = false;
                Debug.Log("Initialized Sector 1 button (hidden).");
            }
            else
            {
                Debug.LogWarning("Sector 1 button is not assigned in ResourceTimerManager!");
            }

            if (sector2Button != null)
            {
                sector2Button.onClick.RemoveAllListeners();
                sector2ButtonCanvasGroup = sector2Button.GetComponent<CanvasGroup>();
                if (sector2ButtonCanvasGroup == null)
                {
                    sector2ButtonCanvasGroup = sector2Button.gameObject.AddComponent<CanvasGroup>();
                }
                sector2ButtonCanvasGroup.alpha = 0f;
                sector2Button.gameObject.SetActive(false);
                sector2Button.interactable = false;
                Debug.Log("Initialized Sector 2 button (hidden).");
            }
            else
            {
                Debug.LogWarning("Sector 2 button is not assigned in ResourceTimerManager!");
            }

            if (sector3Button != null)
            {
                sector3Button.onClick.RemoveAllListeners();
                sector3ButtonCanvasGroup = sector3Button.GetComponent<CanvasGroup>();
                if (sector3ButtonCanvasGroup == null)
                {
                    sector3ButtonCanvasGroup = sector3Button.gameObject.AddComponent<CanvasGroup>();
                }
                sector3ButtonCanvasGroup.alpha = 0f;
                sector3Button.gameObject.SetActive(false);
                sector3Button.interactable = false;
                Debug.Log("Initialized Sector 3 button (hidden).");
            }
            else
            {
                Debug.LogWarning("Sector 3 button is not assigned in ResourceTimerManager!");
            }

            if (sector4Button != null)
            {
                sector4Button.onClick.RemoveAllListeners();
                sector4ButtonCanvasGroup = sector4Button.GetComponent<CanvasGroup>();
                if (sector4ButtonCanvasGroup == null)
                {
                    sector4ButtonCanvasGroup = sector4Button.gameObject.AddComponent<CanvasGroup>();
                }
                sector4ButtonCanvasGroup.alpha = 0f;
                sector4Button.gameObject.SetActive(false);
                sector4Button.interactable = false;
                Debug.Log("Initialized Sector 4 button (hidden).");
            }
            else
            {
                Debug.LogWarning("Sector 4 button is not assigned in ResourceTimerManager!");
            }

            if (sector5Button != null)
            {
                sector5Button.onClick.RemoveAllListeners();
                sector5ButtonCanvasGroup = sector5Button.GetComponent<CanvasGroup>();
                if (sector5ButtonCanvasGroup == null)
                {
                    sector5ButtonCanvasGroup = sector5Button.gameObject.AddComponent<CanvasGroup>();
                }
                sector5ButtonCanvasGroup.alpha = 0f;
                sector5Button.gameObject.SetActive(false);
                sector5Button.interactable = false;
                Debug.Log("Initialized Sector 5 button (hidden).");
            }
            else
            {
                Debug.LogWarning("Sector 5 button is not assigned in ResourceTimerManager!");
            }

            if (sector6Button != null)
            {
                sector6Button.onClick.RemoveAllListeners();
                sector6ButtonCanvasGroup = sector6Button.GetComponent<CanvasGroup>();
                if (sector6ButtonCanvasGroup == null)
                {
                    sector6ButtonCanvasGroup = sector6Button.gameObject.AddComponent<CanvasGroup>();
                }
                sector6ButtonCanvasGroup.alpha = 0f;
                sector6Button.gameObject.SetActive(false);
                sector6Button.interactable = false;
                Debug.Log("Initialized Sector 6 button (hidden).");
            }
            else
            {
                Debug.LogWarning("Sector 6 button is not assigned in ResourceTimerManager!");
            }

            if (sector7Button != null)
            {
                sector7Button.onClick.RemoveAllListeners();
                sector7ButtonCanvasGroup = sector7Button.GetComponent<CanvasGroup>();
                if (sector7ButtonCanvasGroup == null)
                {
                    sector7ButtonCanvasGroup = sector7Button.gameObject.AddComponent<CanvasGroup>();
                }
                sector7ButtonCanvasGroup.alpha = 0f;
                sector7Button.gameObject.SetActive(false);
                sector7Button.interactable = false;
                Debug.Log("Initialized Sector 7 button (hidden).");
            }
            else
            {
                Debug.LogWarning("Sector 7 button is not assigned in ResourceTimerManager!");
            }

            if (sector8Button != null)
            {
                sector8Button.onClick.RemoveAllListeners();
                sector8ButtonCanvasGroup = sector8Button.GetComponent<CanvasGroup>();
                if (sector8ButtonCanvasGroup == null)
                {
                    sector8ButtonCanvasGroup = sector8Button.gameObject.AddComponent<CanvasGroup>();
                }
                sector8ButtonCanvasGroup.alpha = 0f;
                sector8Button.gameObject.SetActive(false);
                sector8Button.interactable = false;
                Debug.Log("Initialized Sector 8 button (hidden).");
            }
            else
            {
                Debug.LogWarning("Sector 8 button is not assigned in ResourceTimerManager!");
            }

            if (sector9Button != null)
            {
                sector9Button.onClick.RemoveAllListeners();
                sector9ButtonCanvasGroup = sector9Button.GetComponent<CanvasGroup>();
                if (sector9ButtonCanvasGroup == null)
                {
                    sector9ButtonCanvasGroup = sector9Button.gameObject.AddComponent<CanvasGroup>();
                }
                sector9ButtonCanvasGroup.alpha = 0f;
                sector9Button.gameObject.SetActive(false);
                sector9Button.interactable = false;
                Debug.Log("Initialized Sector 9 button (hidden).");
            }
            else
            {
                Debug.LogWarning("Sector 9 button is not assigned in ResourceTimerManager!");
            }

            if (sector10Button != null)
            {
                sector10Button.onClick.RemoveAllListeners();
                sector10ButtonCanvasGroup = sector10Button.GetComponent<CanvasGroup>();
                if (sector10ButtonCanvasGroup == null)
                {
                    sector10ButtonCanvasGroup = sector10Button.gameObject.AddComponent<CanvasGroup>();
                }
                sector10ButtonCanvasGroup.alpha = 0f;
                sector10Button.gameObject.SetActive(false);
                sector10Button.interactable = false;
                Debug.Log("Initialized Sector 10 button (hidden).");
            }
            else
            {
                Debug.LogWarning("Sector 10 button is not assigned in ResourceTimerManager!");
            }

            if (sector99Button != null)
            {
                sector99Button.onClick.RemoveAllListeners();
                sector99ButtonCanvasGroup = sector99Button.GetComponent<CanvasGroup>();
                if (sector99ButtonCanvasGroup == null)
                {
                    sector99ButtonCanvasGroup = sector99Button.gameObject.AddComponent<CanvasGroup>();
                }
                sector99ButtonCanvasGroup.alpha = 0f;
                sector99Button.gameObject.SetActive(false);
                sector99Button.interactable = false;
                Debug.Log("Initialized Sector 99 button (hidden).");
            }
            else
            {
                Debug.LogWarning("Sector 99 button is not assigned in ResourceTimerManager!");
            }

            if (sectorUnknownButton != null)
            {
                sectorUnknownButton.onClick.RemoveAllListeners();
                sectorUnknownButtonCanvasGroup = sectorUnknownButton.GetComponent<CanvasGroup>();
                if (sectorUnknownButtonCanvasGroup == null)
                {
                    sectorUnknownButtonCanvasGroup = sectorUnknownButton.gameObject.AddComponent<CanvasGroup>();
                }
                sectorUnknownButtonCanvasGroup.alpha = 0f;
                sectorUnknownButton.gameObject.SetActive(false);
                sectorUnknownButton.interactable = false;
                Debug.Log("Initialized Sector ???? button (hidden).");
            }
            else
            {
                Debug.LogWarning("Sector ???? button is not assigned in ResourceTimerManager!");
            }
        }

        private void Update()
        {
            if (isGameOver || isPaused) return;

            // Update cooldown durations if they change
            if (oxygenCooldownDuration != lastOxygenCooldownDuration)
            {
                UpdateCooldownText(oxygenCooldownText, oxygenCooldownDuration);
                lastOxygenCooldownDuration = oxygenCooldownDuration;
            }
            if (heatCooldownDuration != lastHeatCooldownDuration)
            {
                UpdateCooldownText(heatCooldownText, heatCooldownDuration);
                lastHeatCooldownDuration = heatCooldownDuration;
            }
            if (pressureCooldownDuration != lastPressureCooldownDuration)
            {
                UpdateCooldownText(pressureCooldownText, pressureCooldownDuration);
                lastPressureCooldownDuration = pressureCooldownDuration;
            }
            if (energyCooldownDuration != lastEnergyCooldownDuration)
            {
                UpdateCooldownText(energyCooldownText, energyCooldownDuration);
                lastEnergyCooldownDuration = energyCooldownDuration;
            }
            if (exploreFabricatorCooldownDuration != lastExploreFabricatorCooldownDuration)
            {
                UpdateCooldownText(exploreFabricatorCooldownText, exploreFabricatorCooldownDuration);
                lastExploreFabricatorCooldownDuration = exploreFabricatorCooldownDuration;
            }
            if (exploreResearchBayCooldownDuration != lastExploreResearchBayCooldownDuration)
            {
                UpdateCooldownText(exploreResearchBayCooldownText, exploreResearchBayCooldownDuration);
                lastExploreResearchBayCooldownDuration = exploreResearchBayCooldownDuration;
            }

            // Update Research Timer
            if (isResearchActive)
            {
                researchTimer -= Time.deltaTime;
                if (researchTimer <= 0f)
                {
                    researchTimer = 0f;
                    isResearchActive = false;
                    if (researchTimerText != null)
                    {
                        researchTimerText.text = "-";
                    }
                    if (researchBayResearchButton != null)
                    {
                        researchBayResearchButton.interactable = true;
                    }
                    SetCooldownBarActive(researchBayResearchCooldownBar, false);
                    if (researchCallback != null)
                    {
                        researchCallback.Invoke();
                        researchCallback = null;
                    }
                    Debug.Log("Research completed.");
                }
                else
                {
                    int secondsRemaining = Mathf.CeilToInt(researchTimer);
                    if (researchTimerText != null)
                    {
                        researchTimerText.text = $"{secondsRemaining}s";
                    }
                    float fillAmount = researchTimer / researchDuration;
                    UpdateCooldownBar(researchBayResearchCooldownBar, fillAmount);
                }
            }

            // Update Global Cooldown
            if (isOnGlobalCooldown)
            {
                globalCooldownTimer -= Time.deltaTime;
                float fillAmount = currentCooldownDuration > 0 ? globalCooldownTimer / currentCooldownDuration : 0f;

                // Update all cooldown bars for visible buttons
                if (oxygenResetButton != null && oxygenResetButton.gameObject.activeInHierarchy)
                    UpdateCooldownBar(oxygenCooldownBar, fillAmount);
                if (heatResetButton != null && heatResetButton.gameObject.activeInHierarchy)
                    UpdateCooldownBar(heatCooldownBar, fillAmount);
                if (pressureResetButton != null && pressureResetButton.gameObject.activeInHierarchy)
                    UpdateCooldownBar(pressureCooldownBar, fillAmount);
                if (energyResetButton != null && energyResetButton.gameObject.activeInHierarchy)
                    UpdateCooldownBar(energyCooldownBar, fillAmount);
                if (scavengeButton != null && scavengeButton.gameObject.activeInHierarchy)
                    UpdateCooldownBar(scavengeCooldownBar, fillAmount);
                if (exploreFabricatorButton != null && exploreFabricatorButton.gameObject.activeInHierarchy)
                    UpdateCooldownBar(exploreFabricatorCooldownBar, fillAmount);
                if (exploreResearchBayButton != null && exploreResearchBayButton.gameObject.activeInHierarchy)
                    UpdateCooldownBar(exploreResearchBayCooldownBar, fillAmount);
                if (exploreBarracksButton != null && exploreBarracksButton.gameObject.activeInHierarchy)
                    UpdateCooldownBar(exploreBarracksCooldownBar, fillAmount);
                if (fabricatorProcessButton != null && fabricatorProcessButton.gameObject.activeInHierarchy)
                    UpdateCooldownBar(fabricatorProcessCooldownBar, fillAmount);
                if (barracksOrderButton != null && barracksOrderButton.gameObject.activeInHierarchy)
                    UpdateCooldownBar(barracksOrderCooldownBar, fillAmount);
                if (sector1Button != null && sector1Button.gameObject.activeInHierarchy)
                    UpdateCooldownBar(sector1CooldownBar, fillAmount);
                if (sector2Button != null && sector2Button.gameObject.activeInHierarchy)
                    UpdateCooldownBar(sector2CooldownBar, fillAmount);
                if (sector3Button != null && sector3Button.gameObject.activeInHierarchy)
                    UpdateCooldownBar(sector3CooldownBar, fillAmount);
                if (sector4Button != null && sector4Button.gameObject.activeInHierarchy)
                    UpdateCooldownBar(sector4CooldownBar, fillAmount);
                if (sector5Button != null && sector5Button.gameObject.activeInHierarchy)
                    UpdateCooldownBar(sector5CooldownBar, fillAmount);
                if (sector6Button != null && sector6Button.gameObject.activeInHierarchy)
                    UpdateCooldownBar(sector6CooldownBar, fillAmount);
                if (sector7Button != null && sector7Button.gameObject.activeInHierarchy)
                    UpdateCooldownBar(sector7CooldownBar, fillAmount);
                if (sector8Button != null && sector8Button.gameObject.activeInHierarchy)
                    UpdateCooldownBar(sector8CooldownBar, fillAmount);
                if (sector9Button != null && sector9Button.gameObject.activeInHierarchy)
                    UpdateCooldownBar(sector9CooldownBar, fillAmount);
                if (sector10Button != null && sector10Button.gameObject.activeInHierarchy)
                    UpdateCooldownBar(sector10CooldownBar, fillAmount);
                if (sector99Button != null && sector99Button.gameObject.activeInHierarchy)
                    UpdateCooldownBar(sector99CooldownBar, fillAmount);
                if (sectorUnknownButton != null && sectorUnknownButton.gameObject.activeInHierarchy)
                    UpdateCooldownBar(sectorUnknownCooldownBar, fillAmount);

                if (globalCooldownTimer <= 0)
                {
                    isOnGlobalCooldown = false;
                    globalCooldownTimer = 0f;
                    currentCooldownDuration = 0f;
                    SetButtonsInteractable(true);

                    SetCooldownBarActive(oxygenCooldownBar, false);
                    SetCooldownBarActive(heatCooldownBar, false);
                    SetCooldownBarActive(pressureCooldownBar, false);
                    SetCooldownBarActive(energyCooldownBar, false);
                    SetCooldownBarActive(exploreFabricatorCooldownBar, false);
                    SetCooldownBarActive(exploreResearchBayCooldownBar, false);
                    SetCooldownBarActive(exploreBarracksCooldownBar, false);
                    SetCooldownBarActive(fabricatorProcessCooldownBar, false);
                    SetCooldownBarActive(barracksOrderCooldownBar, false);
                    SetCooldownBarActive(sector1CooldownBar, false);
                    SetCooldownBarActive(sector2CooldownBar, false);
                    SetCooldownBarActive(sector3CooldownBar, false);
                    SetCooldownBarActive(sector4CooldownBar, false);
                    SetCooldownBarActive(sector5CooldownBar, false);
                    SetCooldownBarActive(sector6CooldownBar, false);
                    SetCooldownBarActive(sector7CooldownBar, false);
                    SetCooldownBarActive(sector8CooldownBar, false);
                    SetCooldownBarActive(sector9CooldownBar, false);
                    SetCooldownBarActive(sector10CooldownBar, false);
                    SetCooldownBarActive(sector99CooldownBar, false);
                    SetCooldownBarActive(sectorUnknownCooldownBar, false);
                    SetCooldownBarActive(scavengeCooldownBar, false);

                    if (lastAction == "Scavenge" && !string.IsNullOrEmpty(lastScavengedResource) && inventoryManager != null)
                    {
                        if (System.Enum.TryParse(lastScavengedResource, out ScavengeResourceType resourceType))
                        {
                            inventoryManager.AddScavengeResource(resourceType, 1);
                            Debug.Log($"Added 1x {resourceType} to inventory.");
                        }
                        else
                        {
                            Debug.LogWarning($"Unknown resource type: {lastScavengedResource}");
                        }
                        if (storyManager != null)
                        {
                            storyManager.OnScavengeCompleted(lastScavengedResource);
                        }
                        lastScavengedResource = "";
                    }
                    else if (lastAction == "ExploreFabricator")
                    {
                        if (storyManager != null)
                        {
                            storyManager.OnExploreFabricatorCompleted();
                        }
                    }
                    else if (lastAction == "ExploreResearchBay" && storyManager != null)
                    {
                        storyManager.OnExploreResearchBayCompleted();
                    }

                    lastAction = "";
                    lastReportedProgress = -1f;
                }
                else
                {
                    if (lastAction == "ExploreResearchBay" && storyManager != null)
                    {
                        float progress = 1f - fillAmount;
                        if (progress >= 0f && lastReportedProgress < 0f)
                        {
                            storyManager.OnExploreResearchBayProgress(progress);
                            lastReportedProgress = 0f;
                        }
                        else if (progress >= 0.25f && lastReportedProgress < 0.25f)
                        {
                            storyManager.OnExploreResearchBayProgress(progress);
                            lastReportedProgress = 0.25f;
                        }
                        else if (progress >= 0.50f && lastReportedProgress < 0.50f)
                        {
                            storyManager.OnExploreResearchBayProgress(progress);
                            lastReportedProgress = 0.50f;
                        }
                        else if (progress >= 0.70f && lastReportedProgress < 0.70f)
                        {
                            storyManager.OnExploreResearchBayProgress(progress);
                            lastReportedProgress = 0.70f;
                        }
                        else if (progress >= 0.90f && lastReportedProgress < 0.90f)
                        {
                            storyManager.OnExploreResearchBayProgress(progress);
                            lastReportedProgress = 0.90f;
                        }
                    }
                }
            }

            for (int i = 0; i < timers.Length; i++)
            {
                UpdateTimer(i);
                UpdateButtonAndTimerObjectState(i);
            }
        }

        private void UpdateTimer(int timerIndex)
        {
            ResourceTimer timer = timers[timerIndex];

            if (timer.timerObject != null && timer.timerObject.activeInHierarchy)
            {
                timer.currentUnits -= timer.consumptionRate * Time.deltaTime;

                if (timer.currentUnits <= 0)
                {
                    timer.currentUnits = 0;
                    timers[timerIndex] = timer;
                    UpdateTimerDisplay(timerIndex);
                    TriggerGameOver(timer.name);
                    return;
                }
            }

            timers[timerIndex] = timer;
            UpdateTimerDisplay(timerIndex);
        }

        private void UpdateButtonAndTimerObjectState(int timerIndex)
        {
            ResourceTimer timer = timers[timerIndex];
            bool isActive = timer.timerObject != null && timer.timerObject.activeInHierarchy;

            if (isActive != timer.wasActive)
            {
                if (timer.timerCanvasGroup != null)
                {
                    if (isActive)
                    {
                        timer.timerObject.SetActive(true);
                        StartCoroutine(FadeCanvasGroup(timer.timerCanvasGroup, 0f, 1f, 1f));
                    }
                    else
                    {
                        StartCoroutine(FadeCanvasGroup(timer.timerCanvasGroup, 1f, 0f, 1f, () => timer.timerObject.SetActive(false)));
                    }
                }

                Button targetButton = null;
                CanvasGroup targetCanvasGroup = null;
                TMP_Text cooldownText = null;

                switch (timerIndex)
                {
                    case 0:
                        targetButton = oxygenResetButton;
                        targetCanvasGroup = oxygenButtonCanvasGroup;
                        cooldownText = oxygenCooldownText;
                        break;
                    case 1:
                        targetButton = heatResetButton;
                        targetCanvasGroup = heatButtonCanvasGroup;
                        cooldownText = heatCooldownText;
                        break;
                    case 2:
                        targetButton = pressureResetButton;
                        targetCanvasGroup = pressureButtonCanvasGroup;
                        cooldownText = pressureCooldownText;
                        break;
                    case 3:
                        targetButton = energyResetButton;
                        targetCanvasGroup = energyButtonCanvasGroup;
                        cooldownText = energyCooldownText;
                        break;
                }

                if (targetButton != null && targetCanvasGroup != null)
                {
                    if (isActive)
                    {
                        targetButton.gameObject.SetActive(true);
                        StartCoroutine(FadeCanvasGroup(targetCanvasGroup, 0f, 1f, 0.5f));
                        if (cooldownText != null) cooldownText.gameObject.SetActive(true);
                    }
                    else
                    {
                        StartCoroutine(FadeCanvasGroup(targetCanvasGroup, 1f, 0f, 0.5f, () => targetButton.gameObject.SetActive(false)));
                        if (cooldownText != null) cooldownText.gameObject.SetActive(false);
                    }
                }

                timer.wasActive = isActive;
                timers[timerIndex] = timer;
            }
        }

        private System.Collections.IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration, System.Action onComplete = null)
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

        private void UpdateTimerDisplay(int timerIndex)
        {
            ResourceTimer timer = timers[timerIndex];
            float remainingTime;
            if (timer.consumptionRate == 0)
            {
                remainingTime = 999f;
            }
            else
            {
                remainingTime = timer.currentUnits / timer.consumptionRate;
            }
            int secondsRemaining = Mathf.CeilToInt(remainingTime);
            timer.displayText.text = $"{secondsRemaining}s";
        }

        private void TriggerGameOver(string resourceName)
        {
            if (isGameOver) return;

            isGameOver = true;
            Debug.Log($"Game Over due to {resourceName} reaching zero!");

            string message = "";
            switch (resourceName.ToLower())
            {
                case "oxygen":
                    message = "Oxygen depleted. The world fades to black.";
                    break;
                case "heat":
                    message = "Heat depleted. All turns to ice.";
                    break;
                case "pressure":
                    message = "Pressure depleted. The vacuum prevails.";
                    break;
                case "energy":
                    message = "Energy depleted. The world fades to black.";
                    break;
                default:
                    message = "Game Over! A critical resource was depleted.";
                    break;
            }

            if (gameOverOverlay != null)
            {
                gameOverOverlay.SetActive(true);

                if (gameOverMessageText != null)
                {
                    gameOverMessageText.text = message;
                }
                else
                {
                    Debug.LogWarning("GameOverMessageText is not assigned! Cannot display game over message.");
                }

                CanvasGroup canvasGroup = gameOverOverlay.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f, 1f));
                }
            }
            else
            {
                Debug.LogWarning("GameOverOverlay is not assigned in ResourceTimerManager!");
            }
        }

        public void ResetTimer(int timerIndex)
        {
            if (isGameOver)
            {
                Debug.LogWarning("Cannot reset timer: Game is over!");
                return;
            }

            if (timerIndex < 0 || timerIndex >= timers.Length)
            {
                Debug.LogError($"Invalid timer index: {timerIndex}");
                return;
            }

            if (isOnGlobalCooldown)
            {
                Debug.LogWarning("Cannot reset timer: Global cooldown is active!");
                return;
            }

            float cooldownDuration = 0f;
            switch (timerIndex)
            {
                case 0:
                    cooldownDuration = oxygenCooldownDuration;
                    lastAction = "Oxygen";
                    break;
                case 1:
                    cooldownDuration = heatCooldownDuration;
                    lastAction = "Heat";
                    break;
                case 2:
                    cooldownDuration = pressureCooldownDuration;
                    lastAction = "Pressure";
                    break;
                case 3:
                    cooldownDuration = energyCooldownDuration;
                    lastAction = "Energy";
                    break;
            }
            StartGlobalCooldown(cooldownDuration);

            if (timerIndex == 0)
            {
                bool shouldResetTimer = true;
                if (oxygenRestartCount == 10 || oxygenRestartCount == 11)
                {
                    shouldResetTimer = false;
                }

                oxygenRestartCount++;
                if (storyManager != null)
                {
                    storyManager.OnOxygenRestart(oxygenRestartCount);
                }

                if (shouldResetTimer)
                {
                    ResourceTimer timer = timers[timerIndex];
                    timer.currentUnits = timer.maxUnits;
                    timers[timerIndex] = timer;
                    UpdateTimerDisplay(timerIndex);
                    Debug.Log($"Reset {timer.name} to {timer.currentUnits} units.");
                }
                else
                {
                    Debug.Log("Oxygen timer not reset due to 'Nothing happens' message.");
                }
            }
            else
            {
                ResourceTimer timer = timers[timerIndex];
                timer.currentUnits = timer.maxUnits;
                timers[timerIndex] = timer;
                UpdateTimerDisplay(timerIndex);
                Debug.Log($"Reset {timer.name} to {timer.currentUnits} units.");
            }
        }

        private void StartGlobalCooldown(float duration)
        {
            isOnGlobalCooldown = true;
            globalCooldownTimer = duration;
            currentCooldownDuration = duration;

            SetButtonsInteractable(false);

            if (oxygenResetButton != null && oxygenResetButton.gameObject.activeInHierarchy)
                SetCooldownBarActive(oxygenCooldownBar, true);
            if (heatResetButton != null && heatResetButton.gameObject.activeInHierarchy)
                SetCooldownBarActive(heatCooldownBar, true);
            if (pressureResetButton != null && pressureResetButton.gameObject.activeInHierarchy)
                SetCooldownBarActive(pressureCooldownBar, true);
            if (energyResetButton != null && energyResetButton.gameObject.activeInHierarchy)
                SetCooldownBarActive(energyCooldownBar, true);
            if (scavengeButton != null && scavengeButton.gameObject.activeInHierarchy)
                SetCooldownBarActive(scavengeCooldownBar, true);
            if (exploreFabricatorButton != null && exploreFabricatorButton.gameObject.activeInHierarchy)
                SetCooldownBarActive(exploreFabricatorCooldownBar, true);
            if (exploreResearchBayButton != null && exploreResearchBayButton.gameObject.activeInHierarchy)
                SetCooldownBarActive(exploreResearchBayCooldownBar, true);
            if (exploreBarracksButton != null && exploreBarracksButton.gameObject.activeInHierarchy)
                SetCooldownBarActive(exploreBarracksCooldownBar, true);
            if (fabricatorProcessButton != null && fabricatorProcessButton.gameObject.activeInHierarchy)
                SetCooldownBarActive(fabricatorProcessCooldownBar, true);
            if (barracksOrderButton != null && barracksOrderButton.gameObject.activeInHierarchy)
                SetCooldownBarActive(barracksOrderCooldownBar, true);
            if (sector1Button != null && sector1Button.gameObject.activeInHierarchy)
                SetCooldownBarActive(sector1CooldownBar, true);
            if (sector2Button != null && sector2Button.gameObject.activeInHierarchy)
                SetCooldownBarActive(sector2CooldownBar, true);
            if (sector3Button != null && sector3Button.gameObject.activeInHierarchy)
                SetCooldownBarActive(sector3CooldownBar, true);
            if (sector4Button != null && sector4Button.gameObject.activeInHierarchy)
                SetCooldownBarActive(sector4CooldownBar, true);
            if (sector5Button != null && sector5Button.gameObject.activeInHierarchy)
                SetCooldownBarActive(sector5CooldownBar, true);
            if (sector6Button != null && sector6Button.gameObject.activeInHierarchy)
                SetCooldownBarActive(sector6CooldownBar, true);
            if (sector7Button != null && sector7Button.gameObject.activeInHierarchy)
                SetCooldownBarActive(sector7CooldownBar, true);
            if (sector8Button != null && sector8Button.gameObject.activeInHierarchy)
                SetCooldownBarActive(sector8CooldownBar, true);
            if (sector9Button != null && sector9Button.gameObject.activeInHierarchy)
                SetCooldownBarActive(sector9CooldownBar, true);
            if (sector10Button != null && sector10Button.gameObject.activeInHierarchy)
                SetCooldownBarActive(sector10CooldownBar, true);
            if (sector99Button != null && sector99Button.gameObject.activeInHierarchy)
                SetCooldownBarActive(sector99CooldownBar, true);
            if (sectorUnknownButton != null && sectorUnknownButton.gameObject.activeInHierarchy)
                SetCooldownBarActive(sectorUnknownCooldownBar, true);
        }

        public void StartResearchBayResearch(float duration, float oxygenMaxIncrease, ResearchCompletedCallback callback)
        {
            if (isResearchActive)
            {
                Debug.LogWarning("Cannot start research: Research Bay is already active!");
                return;
            }

            // Show the Research Timer if it's the first time
            if (!isResearchTimerVisible && researchTimerObject != null)
            {
                researchTimerObject.SetActive(true);
                StartCoroutine(FadeCanvasGroup(researchTimerCanvasGroup, 0f, 1f, 1f));
                isResearchTimerVisible = true;
            }

            // Start the research
            isResearchActive = true;
            researchTimer = duration;
            researchDuration = duration;
            researchCallback = () =>
            {
                // Apply the Oxygen max increase
                ResourceTimer oxygenTimer = timers[0]; // Oxygen is the first timer (index 0)
                oxygenTimer.maxUnits += oxygenMaxIncrease;
                oxygenTimer.currentUnits = Mathf.Min(oxygenTimer.currentUnits, oxygenTimer.maxUnits); // Ensure current doesn't exceed new max
                timers[0] = oxygenTimer;
                UpdateTimerDisplay(0);
                Debug.Log($"Oxygen max units increased by {oxygenMaxIncrease}. New max: {oxygenTimer.maxUnits}");

                // Invoke the original callback
                callback?.Invoke();
            };

            // Disable the Research button
            if (researchBayResearchButton != null)
            {
                researchBayResearchButton.interactable = false;
            }

            // Activate the cooldown bar for the Research button
            SetCooldownBarActive(researchBayResearchCooldownBar, true);
            UpdateCooldownBar(researchBayResearchCooldownBar, 1f);

            Debug.Log($"Started research with duration {duration}s and oxygen max increase of {oxygenMaxIncrease}.");
        }

        private void SetButtonsInteractable(bool interactable)
        {
            if (oxygenResetButton != null) oxygenResetButton.interactable = interactable;
            if (heatResetButton != null) heatResetButton.interactable = interactable;
            if (pressureResetButton != null) pressureResetButton.interactable = interactable;
            if (energyResetButton != null) energyResetButton.interactable = interactable;

            if (exploreFabricatorButton != null) exploreFabricatorButton.interactable = interactable;
            if (exploreResearchBayButton != null) exploreResearchBayButton.interactable = interactable;
            if (exploreBarracksButton != null) exploreBarracksButton.interactable = interactable;
            if (fabricatorProcessButton != null) fabricatorProcessButton.interactable = interactable;
            // Note: researchBayResearchButton is managed separately by isResearchActive
            if (barracksOrderButton != null) barracksOrderButton.interactable = interactable;
            if (sector1Button != null) sector1Button.interactable = interactable;
            if (sector2Button != null) sector2Button.interactable = interactable;
            if (sector3Button != null) sector3Button.interactable = interactable;
            if (sector4Button != null) sector4Button.interactable = interactable;
            if (sector5Button != null) sector5Button.interactable = interactable;
            if (sector6Button != null) sector6Button.interactable = interactable;
            if (sector7Button != null) sector7Button.interactable = interactable;
            if (sector8Button != null) sector8Button.interactable = interactable;
            if (sector9Button != null) sector9Button.interactable = interactable;
            if (sector10Button != null) sector10Button.interactable = interactable;
            if (sector99Button != null) sector99Button.interactable = interactable;
            if (sectorUnknownButton != null) sectorUnknownButton.interactable = interactable;
            if (scavengeButton != null) scavengeButton.interactable = interactable;

            if (storyManager != null)
            {
                storyManager.SetUIInteractable(interactable);
            }
        }

        private void SetCooldownBarActive(Image cooldownBar, bool active)
        {
            if (cooldownBar != null)
            {
                cooldownBar.gameObject.SetActive(active);
            }
        }

        private void UpdateCooldownBar(Image cooldownBar, float fillAmount)
        {
            if (cooldownBar != null)
            {
                cooldownBar.fillAmount = Mathf.Clamp01(fillAmount);
            }
        }

        private void UpdateCooldownText(TMP_Text cooldownText, float duration)
        {
            if (cooldownText != null)
            {
                cooldownText.text = duration > 0f ? $"{duration:F1}s" : "";
            }
        }

        public void PauseTimers(bool pause)
        {
            isPaused = pause;
            Debug.Log($"Timers and cooldowns {(pause ? "paused" : "resumed")}. isGameOver: {isGameOver}, isOnGlobalCooldown: {isOnGlobalCooldown}");

            if (!pause)
            {
                if (!isOnGlobalCooldown)
                {
                    SetButtonsInteractable(true);
                    Debug.Log("No active cooldown detected on resume. Buttons set to interactable.");
                }

                if (isOnGlobalCooldown)
                {
                    Debug.Log($"Resuming with active cooldown. Remaining time: {globalCooldownTimer}s");
                }

                if (isGameOver)
                {
                    Debug.LogWarning("Game is in Game Over state. Timers will not resume until this is resolved.");
                }
            }
        }

        public void SetMaxUnits(int timerIndex, float newMaxUnits)
        {
            if (timerIndex < 0 || timerIndex >= timers.Length)
            {
                Debug.LogError($"Invalid timer index: {timerIndex}");
                return;
            }

            ResourceTimer timer = timers[timerIndex];
            timer.maxUnits = newMaxUnits;
            timer.currentUnits = Mathf.Min(timer.currentUnits, newMaxUnits);
            timers[timerIndex] = timer;
            UpdateTimerDisplay(timerIndex);
        }

        public void SetCurrentUnits(int timerIndex, float newCurrentUnits)
        {
            if (timerIndex < 0 || timerIndex >= timers.Length)
            {
                Debug.LogError($"Invalid timer index: {timerIndex}");
                return;
            }

            ResourceTimer timer = timers[timerIndex];
            timer.currentUnits = Mathf.Clamp(newCurrentUnits, 0, timer.maxUnits);
            timers[timerIndex] = timer;
            UpdateTimerDisplay(timerIndex);
        }

        public void SetConsumptionRate(int timerIndex, float newRate)
        {
            if (timerIndex < 0 || timerIndex >= timers.Length)
            {
                Debug.LogError($"Invalid timer index: {timerIndex}");
                return;
            }

            ResourceTimer timer = timers[timerIndex];
            timer.consumptionRate = newRate;
            timers[timerIndex] = timer;
            UpdateTimerDisplay(timerIndex);
        }

        public void SetOxygenCooldownDuration(float newDuration)
        {
            if (newDuration < 0)
            {
                Debug.LogWarning("Cooldown duration cannot be negative. Setting to 0.");
                newDuration = 0;
            }
            oxygenCooldownDuration = newDuration;
            lastOxygenCooldownDuration = newDuration;
            UpdateCooldownText(oxygenCooldownText, oxygenCooldownDuration);
            Debug.Log($"Oxygen cooldown duration set to {newDuration} seconds.");
        }

        public void SetHeatCooldownDuration(float newDuration)
        {
            if (newDuration < 0)
            {
                Debug.LogWarning("Cooldown duration cannot be negative. Setting to 0.");
                newDuration = 0;
            }
            heatCooldownDuration = newDuration;
            lastHeatCooldownDuration = newDuration;
            UpdateCooldownText(heatCooldownText, heatCooldownDuration);
            Debug.Log($"Heat cooldown duration set to {newDuration} seconds.");
        }

        public void SetPressureCooldownDuration(float newDuration)
        {
            if (newDuration < 0)
            {
                Debug.LogWarning("Cooldown duration cannot be negative. Setting to 0.");
                newDuration = 0;
            }
            pressureCooldownDuration = newDuration;
            lastPressureCooldownDuration = newDuration;
            UpdateCooldownText(pressureCooldownText, pressureCooldownDuration);
            Debug.Log($"Pressure cooldown duration set to {newDuration} seconds.");
        }

        public void SetEnergyCooldownDuration(float newDuration)
        {
            if (newDuration < 0)
            {
                Debug.LogWarning("Cooldown duration cannot be negative. Setting to 0.");
                newDuration = 0;
            }
            energyCooldownDuration = newDuration;
            lastEnergyCooldownDuration = newDuration;
            UpdateCooldownText(energyCooldownText, energyCooldownDuration);
            Debug.Log($"Energy cooldown duration set to {newDuration} seconds.");
        }

        public void SetExploreFabricatorCooldownDuration(float newDuration)
        {
            if (newDuration < 0)
            {
                Debug.LogWarning("Cooldown duration cannot be negative. Setting to 0.");
                newDuration = 0;
            }
            exploreFabricatorCooldownDuration = newDuration;
            lastExploreFabricatorCooldownDuration = newDuration;
            UpdateCooldownText(exploreFabricatorCooldownText, exploreFabricatorCooldownDuration);
            Debug.Log($"Explore Fabricator cooldown duration set to {newDuration} seconds.");
        }

        public void SetExploreResearchBayCooldownDuration(float newDuration)
        {
            if (newDuration < 0)
            {
                Debug.LogWarning("Cooldown duration cannot be negative. Setting to 0.");
                newDuration = 0;
            }
            exploreResearchBayCooldownDuration = newDuration;
            lastExploreResearchBayCooldownDuration = newDuration;
            UpdateCooldownText(exploreResearchBayCooldownText, exploreResearchBayCooldownDuration);
            Debug.Log($"Explore Research Bay cooldown duration set to {newDuration} seconds.");
        }

        public void ShowTimer(int timerIndex)
        {
            ResourceTimer timer = timers[timerIndex];
            timer.wasActive = true;
            timer.timerObject.SetActive(true);
            timer.timerCanvasGroup.alpha = 1f;
            timers[timerIndex] = timer;

            if (timerIndex == 0 && oxygenResetButton != null)
            {
                oxygenButtonCanvasGroup.alpha = 1f;
                oxygenResetButton.gameObject.SetActive(true);
                oxygenResetButton.interactable = true;
            }
            else if (timerIndex == 1 && heatResetButton != null)
            {
                heatButtonCanvasGroup.alpha = 1f;
                heatResetButton.gameObject.SetActive(true);
                heatResetButton.interactable = true;
            }
            else if (timerIndex == 2 && pressureResetButton != null)
            {
                pressureButtonCanvasGroup.alpha = 1f;
                pressureResetButton.gameObject.SetActive(true);
                pressureResetButton.interactable = true;
            }
            else if (timerIndex == 3 && energyResetButton != null)
            {
                energyButtonCanvasGroup.alpha = 1f;
                energyResetButton.gameObject.SetActive(true);
                energyResetButton.interactable = true;
            }
        }

        public void ShowScavengeButton()
        {
            if (scavengeButton != null)
            {
                scavengeButtonCanvasGroup.alpha = 1f;
                scavengeButton.gameObject.SetActive(true);
                scavengeButton.interactable = true;
            }
        }

        public void ShowExploreFabricatorButton()
        {
            if (exploreFabricatorButton != null)
            {
                exploreFabricatorButtonCanvasGroup.alpha = 1f;
                exploreFabricatorButton.gameObject.SetActive(true);
                exploreFabricatorButton.interactable = true;
            }
        }

        public void ShowExploreResearchBayButton()
        {
            if (exploreResearchBayButton != null)
            {
                exploreResearchBayButtonCanvasGroup.alpha = 1f;
                exploreResearchBayButton.gameObject.SetActive(true);
                exploreResearchBayButton.interactable = true;
            }
        }

        public void ShowExploreBarracksButton()
        {
            if (exploreBarracksButton != null)
            {
                exploreBarracksButtonCanvasGroup.alpha = 1f;
                exploreBarracksButton.gameObject.SetActive(true);
                exploreBarracksButton.interactable = true;
            }
        }

        public void ShowFabricatorProcessButton()
        {
            if (fabricatorProcessButton != null)
            {
                fabricatorProcessButtonCanvasGroup.alpha = 1f;
                fabricatorProcessButton.gameObject.SetActive(true);
                fabricatorProcessButton.interactable = true;
            }
        }

        public void ShowResearchBayResearchButton()
        {
            if (researchBayResearchButton != null)
            {
                researchBayResearchButtonCanvasGroup.alpha = 1f;
                researchBayResearchButton.gameObject.SetActive(true);
                researchBayResearchButton.interactable = true;
            }
        }

        public void ShowBarracksOrderButton()
        {
            if (barracksOrderButton != null)
            {
                barracksOrderButtonCanvasGroup.alpha = 1f;
                barracksOrderButton.gameObject.SetActive(true);
                barracksOrderButton.interactable = true;
            }
        }

        public void ShowSectorButton(string sectorName)
        {
            Button button = null;
            CanvasGroup canvasGroup = null;

            switch (sectorName)
            {
                case "Sector 1":
                    button = sector1Button;
                    canvasGroup = sector1ButtonCanvasGroup;
                    break;
                case "Sector 2":
                    button = sector2Button;
                    canvasGroup = sector2ButtonCanvasGroup;
                    break;
                case "Sector 3":
                    button = sector3Button;
                    canvasGroup = sector3ButtonCanvasGroup;
                    break;
                case "Sector 4":
                    button = sector4Button;
                    canvasGroup = sector4ButtonCanvasGroup;
                    break;
                case "Sector 5":
                    button = sector5Button;
                    canvasGroup = sector5ButtonCanvasGroup;
                    break;
                case "Sector 6":
                    button = sector6Button;
                    canvasGroup = sector6ButtonCanvasGroup;
                    break;
                case "Sector 7":
                    button = sector7Button;
                    canvasGroup = sector7ButtonCanvasGroup;
                    break;
                case "Sector 8":
                    button = sector8Button;
                    canvasGroup = sector8ButtonCanvasGroup;
                    break;
                case "Sector 9":
                    button = sector9Button;
                    canvasGroup = sector9ButtonCanvasGroup;
                    break;
                case "Sector 10":
                    button = sector10Button;
                    canvasGroup = sector10ButtonCanvasGroup;
                    break;
                case "Sector 99":
                    button = sector99Button;
                    canvasGroup = sector99ButtonCanvasGroup;
                    break;
                case "Sector Unknown":
                    button = sectorUnknownButton;
                    canvasGroup = sectorUnknownButtonCanvasGroup;
                    break;
            }

            if (button != null && canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                button.gameObject.SetActive(true);
                button.interactable = true;
                Debug.Log($"Showing {sectorName} button.");
            }
        }
    }
}