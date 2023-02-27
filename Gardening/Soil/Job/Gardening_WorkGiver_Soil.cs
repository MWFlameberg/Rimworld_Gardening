using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Rimworld_Gardening {
    public class Gardening_WorkGiver_Soil : WorkGiver_Scanner {
        private static string NoPathTrans;

        private const int MiningJobTicks = 20000;

        public override PathEndMode PathEndMode => PathEndMode.Touch;

        public override Danger MaxPathDanger(Pawn pawn) {
            return Danger.Deadly;
        }
        public static void ResetStaticData() {
            NoPathTrans = "NoPath".Translate();
        }
        public override bool ShouldSkip(Pawn pawn, bool forced = false) {
            return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(Gardening_DesignatorDefOf.Gardening_DigSoil);
        }
        public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn) {
            foreach (Designation item in pawn.Map.designationManager.SpawnedDesignationsOfDef(Gardening_DesignatorDefOf.Gardening_DigSoil)) {
                yield return item.target.Cell;
            }
        }
        public override Job JobOnCell(Pawn pawn, IntVec3 cell, bool forced = false) {
            Job job = new Job(JobDefOf.Mine, cell);
            return job;
        }
    }
}
