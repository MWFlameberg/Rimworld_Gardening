using RimWorld;
using UnityEngine;
using Verse;

namespace Rimworld_Gardening {
    public class Gardening_Designator_DigSoil : Designator_Cells {
        public override int DraggableDimensions => 2;
        public override bool DragDrawMeasurements => true;
        protected override DesignationDef Designation => Gardening_DesignatorDefOf.Gardening_DigSoil;
        public Gardening_Designator_DigSoil() {
            defaultLabel = "Dig Soil";
            //icon = ContentFinder<Texture2D>.Get("UI/Designators/Mine");
            defaultDesc = "Dig Soil";
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
            if (!base.Map.terrainGrid.TerrainAt(loc).IsSoil) {
                return AcceptanceReport.WasRejected;
            }
            return AcceptanceReport.WasAccepted;
        }
    }
    [DefOf]
    public static class Gardening_DesignatorDefOf {
        public static Gardening_DesignatorDef Gardening_DigSoil;
    }
    public class Gardening_DesignatorDef : DesignationDef {
        public override void PostLoad() {
            base.PostLoad();
        }
    }
}
