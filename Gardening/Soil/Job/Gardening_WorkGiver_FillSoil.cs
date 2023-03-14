using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;

namespace Rimworld_Gardening {
    public class Gardening_WorkGiver_FillSoil : WorkGiver_Scanner {
        public override PathEndMode PathEndMode => PathEndMode.Touch;

        public override Danger MaxPathDanger(Pawn pawn) {
            return Danger.Deadly;
        }
        public override bool ShouldSkip(Pawn pawn, bool forced = false) {
            return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(Gardening_DesignatorDefOf.Gardening_FillSoil);
        }
        public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn) {
            foreach (Designation item in pawn.Map.designationManager.SpawnedDesignationsOfDef(Gardening_DesignatorDefOf.Gardening_FillSoil)) {
                yield return item.target.Cell;
            }
        }
        public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false) {
            if (c.IsForbidden(pawn)) {
                return false;
            }
            if (!pawn.CanReserve(c, 1, -1, null, forced)) {
                return false;
            }
            return true;
        }
        public override Job JobOnCell(Pawn pawn, IntVec3 cell, bool forced = false) {
            List<Thing> soil = FindAllSoil(pawn, cell);
            Job job = JobMaker.MakeJob(Gardening_JobDefOf.Gardening_FillSoil, cell);
            job.targetQueueB = soil.Select((Thing f) => new LocalTargetInfo(f)).ToList();
            job.count = 25;
            return job;
        }
        public List<Thing> FindAllSoil(Pawn pawn, IntVec3 cell) {
            return RefuelWorkGiverUtility.FindEnoughReservableThings(pawn, cell, new IntRange(50, 50), (Thing t) => t.def == ThingDef.Named("Gardening_Soil"));
        }
    }
}
