using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Rimworld_Gardening {
    public class Gardening_WorkGiver_DigSoil : WorkGiver_Scanner {
        public override PathEndMode PathEndMode => PathEndMode.Touch;

        public override Danger MaxPathDanger(Pawn pawn) {
            return Danger.Deadly;
        }
        public override bool ShouldSkip(Pawn pawn, bool forced = false) {
            return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(Gardening_DesignatorDefOf.Gardening_DigSoil);
        }
        public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn) {
            foreach (Designation item in pawn.Map.designationManager.SpawnedDesignationsOfDef(Gardening_DesignatorDefOf.Gardening_DigSoil)) {
                yield return item.target.Cell;
            }
        }
        public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false) {
            if(c.IsForbidden(pawn)) {
                return false;
            }
            if(!pawn.CanReserve(c, 1, -1, null, forced)) {
                return false;
            }
            return true;
        }
        public override Job JobOnCell(Pawn pawn, IntVec3 cell, bool forced = false) {
            return JobMaker.MakeJob(Gardening_JobDefOf.Gardening_DigSoil, cell);
        }
    }
    [DefOf]
    public static class Gardening_JobDefOf {
        public static JobDef Gardening_DigSoil;
    }
}
