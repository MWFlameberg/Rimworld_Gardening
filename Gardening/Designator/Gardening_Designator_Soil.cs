using RimWorld;
using UnityEngine;
using Verse;

namespace Rimworld_Gardening {
    public class Gardening_Designator_Soil : Designator_Cells {
        protected override DesignationDef Designation => Gardening_DesignatorDefOf.Gardening_DigSoil;
        public Gardening_Designator_Soil() {
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
}
