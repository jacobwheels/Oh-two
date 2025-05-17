using UnityEngine;
using System.Collections;
using O2Game; // Added to reference GameConstants and InventoryManager

namespace O2Game // Added namespace
{
    public class CombatManager : MonoBehaviour
    {
        private InventoryManager inventoryManager;

        private void Awake()
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
        }

        public void StartCombat()
        {
            StartCoroutine(CombatCoroutine());
        }

        private IEnumerator CombatCoroutine()
        {
            yield return new WaitForSeconds(10f); // Simulate combat duration
            foreach (CombatDropType type in System.Enum.GetValues(typeof(CombatDropType)))
            {
                if (type == CombatDropType.None) continue;

                float dropRate = 0f;
                switch (type)
                {
                    case CombatDropType.BiosteelShard:
                        dropRate = GameConstants.BIOSTEEL_SHARD_DROP_RATE;
                        break;
                    case CombatDropType.PlasmaShard:
                        dropRate = GameConstants.PLASMA_SHARD_DROP_RATE;
                        break;
                    case CombatDropType.NeutronShard:
                        dropRate = GameConstants.NEUTRON_SHARD_DROP_RATE;
                        break;
                    case CombatDropType.DarkMatterCatalyst:
                        dropRate = GameConstants.DARK_MATTER_CATALYST_DROP_RATE;
                        break;
                }

                if (Random.value <= dropRate)
                {
                    inventoryManager.AddCombatDrop(type, 1);
                }
            }
        }
    }
}