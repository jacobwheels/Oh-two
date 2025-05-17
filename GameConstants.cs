using UnityEngine;

namespace O2Game // Added namespace
{
    public enum ResourceType
    {
        None,
        Oxygen,
        Heat,
        Pressure,
        Energy
    }

    public enum ScavengeResourceType
    {
        None,
        Copper,
        Steel,
        Glass,
        Carbon,
        Hydrogen,
        Titanium,
        Uranium
    }

    public enum CombatDropType
    {
        None,
        BiosteelShard,
        PlasmaShard,
        NeutronShard,
        DarkMatterCatalyst
    }

    public enum FabricatorItemType
    {
        None,
        CopperPlate,
        CopperWire,
        SteelPlate,
        TemperedGlass,
        GrapheneSheet,
        NutrientPaste,
        CarbonFiber,
        LiquidHydrogen,
        TitaniumPlate,
        U235,
        Hydrocarbon,
        Nanobots,
        PlasmaConduit,
        AblativeCoating,
        SyntheticMuscle,
        GravitonEmitter,
        PowerCoreS,
        PowerCoreM,
        PowerCoreL,
        PowerCoreXL
    }

    public enum BarracksItemType
    {
        None,
        // Melee
        SteelKnuckles,
        RocketGloves,
        LaserSword,
        NeutronDagger,
        ImplosionGauntlet,
        // Ranged
        Sling,
        GaussPistol,
        LaserRifle,
        GravityCannon,
        AntimatterLauncher,
        // Consumables
        Medkit,
        O2Canister,
        PortableFriction,
        ReadyVac,
        FieldRation,
        // Explosives
        IED,
        PlasmaGrenade,
        WristRocket,
        LocalizedFission,
        CannedBlackHole,
        // Comrades
        Crawler,
        Grunt,
        SamurAI,
        Mech
    }

    public static class GameConstants
    {
        // Resource base values
        public static readonly float OXYGEN_BASE_RESET_TIME = 3f;
        public static readonly float OXYGEN_BASE_CONSUMPTION_RATE = 10f;
        public static readonly float OXYGEN_BASE_MAX = 150f;
        public static readonly float OXYGEN_BASE_SECONDS = 15f;

        public static readonly float HEAT_BASE_RESET_TIME = 5f;
        public static readonly float HEAT_BASE_CONSUMPTION_RATE = 10f;
        public static readonly float HEAT_BASE_MAX = 250f;
        public static readonly float HEAT_BASE_SECONDS = 25f;

        public static readonly float PRESSURE_BASE_RESET_TIME = 6f;
        public static readonly float PRESSURE_BASE_CONSUMPTION_RATE = 12f;
        public static readonly float PRESSURE_BASE_MAX = 350f;
        public static readonly float PRESSURE_BASE_SECONDS = 29.166666666666668f;

        public static readonly float ENERGY_BASE_RESET_TIME = 4f;
        public static readonly float ENERGY_BASE_CONSUMPTION_RATE = 5f;
        public static readonly float ENERGY_BASE_MAX = 100f;
        public static readonly float ENERGY_BASE_SECONDS = 20f;

        // Scavenge base times
        public static readonly float COPPER_SCAVENGE_TIME = 5f;
        public static readonly float STEEL_SCAVENGE_TIME = 7f;
        public static readonly float GLASS_SCAVENGE_TIME = 10f;
        public static readonly float CARBON_SCAVENGE_TIME = 12f;
        public static readonly float HYDROGEN_SCAVENGE_TIME = 20f;
        public static readonly float TITANIUM_SCAVENGE_TIME = 25f;
        public static readonly float URANIUM_SCAVENGE_TIME = 30f;

        // Combat drop rates
        public static readonly float BIOSTEEL_SHARD_DROP_RATE = 0.3f;
        public static readonly float PLASMA_SHARD_DROP_RATE = 0.3f;
        public static readonly float NEUTRON_SHARD_DROP_RATE = 0.3f;
        public static readonly float DARK_MATTER_CATALYST_DROP_RATE = 0.3f;
    }
}