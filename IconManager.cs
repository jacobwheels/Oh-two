using UnityEngine;
using System.Collections.Generic;

namespace O2Game
{
    public class IconManager : MonoBehaviour
    {
        [System.Serializable]
        public struct IconEntry
        {
            public string name; // e.g., "Copper", "BiosteelShard", "Oxygen"
            public Sprite sprite; // Corresponding sprite
        }

        [SerializeField]
        private IconEntry[] icons = new IconEntry[]
        {
            new IconEntry { name = "Oxygen", sprite = null },
            new IconEntry { name = "Heat", sprite = null },
            new IconEntry { name = "Pressure", sprite = null },
            new IconEntry { name = "Energy", sprite = null },
            new IconEntry { name = "Copper", sprite = null },
            new IconEntry { name = "Steel", sprite = null },
            new IconEntry { name = "Glass", sprite = null },
            new IconEntry { name = "Carbon", sprite = null },
            new IconEntry { name = "Hydrogen", sprite = null },
            new IconEntry { name = "Titanium", sprite = null },
            new IconEntry { name = "Uranium", sprite = null },
            new IconEntry { name = "Biosteel shard", sprite = null },
            new IconEntry { name = "Plasma shard", sprite = null },
            new IconEntry { name = "Neutron shard", sprite = null },
            new IconEntry { name = "Dark matter catalyst", sprite = null },
            new IconEntry { name = "Copper plate", sprite = null },
            new IconEntry { name = "Copper wire", sprite = null },
            new IconEntry { name = "Steel plate", sprite = null },
            new IconEntry { name = "Tempered glass", sprite = null },
            new IconEntry { name = "Graphene sheet", sprite = null },
            new IconEntry { name = "Nutrient paste", sprite = null },
            new IconEntry { name = "Carbon fiber", sprite = null },
            new IconEntry { name = "Liquid hydrogen", sprite = null },
            new IconEntry { name = "Titanium plate", sprite = null },
            new IconEntry { name = "U-235", sprite = null },
            new IconEntry { name = "Hydrocarbon", sprite = null },
            new IconEntry { name = "Nanobots", sprite = null },
            new IconEntry { name = "Plasma conduit", sprite = null },
            new IconEntry { name = "Ablative coating", sprite = null },
            new IconEntry { name = "Synthetic muscle", sprite = null },
            new IconEntry { name = "Graviton emitter", sprite = null },
            new IconEntry { name = "Power core (S)", sprite = null },
            new IconEntry { name = "Power core (M)", sprite = null },
            new IconEntry { name = "Power core (L)", sprite = null },
            new IconEntry { name = "Power core (XL)", sprite = null },
            new IconEntry { name = "Steel knuckles", sprite = null },
            new IconEntry { name = "Rocket gloves", sprite = null },
            new IconEntry { name = "Laser sword", sprite = null },
            new IconEntry { name = "Neutron dagger", sprite = null },
            new IconEntry { name = "Implosion gauntlet", sprite = null },
            new IconEntry { name = "Sling", sprite = null },
            new IconEntry { name = "Gauss pistol", sprite = null },
            new IconEntry { name = "Laser rifle", sprite = null },
            new IconEntry { name = "Gravity cannon", sprite = null },
            new IconEntry { name = "Antimatter launcher", sprite = null },
            new IconEntry { name = "Medkit", sprite = null },
            new IconEntry { name = "O2 cannister", sprite = null },
            new IconEntry { name = "Portable friction", sprite = null },
            new IconEntry { name = "Ready-vac", sprite = null },
            new IconEntry { name = "Field ration", sprite = null },
            new IconEntry { name = "IED", sprite = null },
            new IconEntry { name = "Plasma grenade", sprite = null },
            new IconEntry { name = "Wrist rocket", sprite = null },
            new IconEntry { name = "Localized fission", sprite = null },
            new IconEntry { name = "Canned black hole", sprite = null },
            new IconEntry { name = "Crawler", sprite = null },
            new IconEntry { name = "Grunt", sprite = null },
            new IconEntry { name = "Samur-AI", sprite = null },
            new IconEntry { name = "Mech", sprite = null }
        };

        private Dictionary<string, Sprite> iconMap = new Dictionary<string, Sprite>();
        public static IconManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            // Populate icon map with normalized names
            foreach (var entry in icons)
            {
                if (!string.IsNullOrEmpty(entry.name) && entry.sprite != null)
                {
                    string normalizedName = NormalizeName(entry.name);
                    iconMap[normalizedName] = entry.sprite;
                }
                else if (string.IsNullOrEmpty(entry.name))
                {
                    Debug.LogWarning("Icon entry has empty name.");
                }
                else
                {
                    Debug.LogWarning($"Sprite missing for icon: Name='{entry.name}'");
                }
            }
        }

        public Sprite GetIcon(string name)
        {
            string normalizedName = NormalizeName(name);
            if (iconMap.TryGetValue(normalizedName, out Sprite sprite))
            {
                return sprite;
            }
            Debug.LogWarning($"No icon found for '{name}' (normalized: '{normalizedName}') in IconManager.");
            return null;
        }

        private string NormalizeName(string name)
        {
            // Convert user-friendly names to enum-style names (e.g., "Biosteel shard" to "BiosteelShard")
            if (string.IsNullOrEmpty(name)) return name;

            // Handle special cases (e.g., "Power core (S)" to "PowerCoreS")
            name = name.Replace("Power core (S)", "PowerCoreS")
                       .Replace("Power core (M)", "PowerCoreM")
                       .Replace("Power core (L)", "PowerCoreL")
                       .Replace("Power core (XL)", "PowerCoreXL")
                       .Replace("U-235", "U235")
                       .Replace("Samur-AI", "SamurAI");

            // Remove spaces and convert to camelCase
            string[] words = name.Split(' ');
            if (words.Length == 1) return name; // Single word, e.g., "Copper"

            for (int i = 1; i < words.Length; i++)
            {
                if (!string.IsNullOrEmpty(words[i]))
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
            }
            return string.Concat(words);
        }
    }
}