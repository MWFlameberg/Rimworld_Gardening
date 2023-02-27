using RimWorld;
using Verse;

namespace Rimworld_Gardening {
    public class Gardening_TrellisPlant : Plant {
        public override void PlantCollected(Pawn by, PlantDestructionMode plantDestructionMode) {
            if (base.Position.GetFirstBuilding(base.Map).Label == "trellis") {
                growthInt = def.GetModExtension<Gardening_TrellisExtension>().trellisHarvestAfterGrowth;
                base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
            }
            else {
                base.PlantCollected(by, plantDestructionMode);
            }
        }
    }
}
