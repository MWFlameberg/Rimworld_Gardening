using RimWorld;
using UnityEngine;
using Verse;

namespace Rimworld_Gardening {
    public class Gardening_Designator_FillSoil : Designator_Cells {
        public override int DraggableDimensions => 2;
        public override bool DragDrawMeasurements => true;
        protected override DesignationDef Designation => Gardening_DesignatorDefOf.Gardening_FillSoil;
        public Gardening_Designator_FillSoil() {
            defaultLabel = "Fill Soil";
            //icon = ContentFinder<Texture2D>.Get("UI/Designators/Mine");
            defaultDesc = "Fill Soil";
            useMouseIcon = true;
            soundDragSustain = SoundDefOf.Designate_DragStandard;
            soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
            soundSucceeded = SoundDefOf.Designate_Mine;
            //hotKey = KeyBindingDefOf.Misc10;
            //tutorTag = "Mine";
        }
        public override void DesignateSingleCell(IntVec3 loc) {
            base.Map.designationManager.AddDesignation(new Designation(loc, Designation));
        }
        public override AcceptanceReport CanDesignateCell(IntVec3 loc) {
            if (!loc.InBounds(base.Map)) {
                return false;
            }
            if (base.Map.designationManager.DesignationAt(loc, Designation) != null) {
                return AcceptanceReport.WasRejected;
            }
            if (base.Map.terrainGrid.TerrainAt(loc).defName != "Gardening_DugSoil") {
                return AcceptanceReport.WasRejected;
            }
            return AcceptanceReport.WasAccepted;
        }
    }
}
