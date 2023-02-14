using RimWorld;
using Verse;

namespace Rimworld_Gardening {
    internal class Gardening_SuperRice : Plant {
        public override void PlantCollected(Pawn by, PlantDestructionMode plantDestructionMode) {
            if (base.Position.GetFirstBuilding(base.Map).Label == "trellis") {
                growthInt = def.GetModExtension<Gardening_PlantExtension>().trellisHarvestAfterGrowth;
                base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
            }
            else {
                base.PlantCollected(by, plantDestructionMode);
            }
        }
    }
}
