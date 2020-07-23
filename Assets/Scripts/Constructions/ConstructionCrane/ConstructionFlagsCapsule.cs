namespace Assets.Scripts.Constructions.ConstructionCrane
{
    public struct ConstructionFlagsCapsule
    {
        public bool BuildingInProgress;
        public bool RepairInProgress;
        public bool MenuInProgress;
        public bool DismantleInProgress;

        public ConstructionFlagsCapsule(
            bool buildingInProgress = false,
            bool repairInProgress = false,
            bool menuInProgress = false,
            bool dismantleInProgress = false
            )
        {
            BuildingInProgress = buildingInProgress;
            RepairInProgress = repairInProgress;
            MenuInProgress = menuInProgress;
            DismantleInProgress = dismantleInProgress;
        }
    }
}