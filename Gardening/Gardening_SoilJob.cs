using RimWorld;
using Verse;
using Verse.AI;

namespace Rimworld_Gardening {
    internal class Gardening_SoilJob : WorkGiver_HaulGeneral {
        public override bool ShouldSkip(Pawn pawn, bool forced = false) {
            return base.ShouldSkip(pawn, forced);
        }
        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false) {
            Building planter = FindPlanterToFill(pawn);
            return new Job(Gardening_JobDefOf.Gardening_SoilJob, planter);
        }
        public static Building FindPlanterToFill(Pawn pawn) {
            foreach (Gardening_Planter planter in pawn.Map.listerBuildings.AllBuildingsColonistOfClass<Gardening_Planter>()) {
                if (planter.def.fertility != planter.GetComp<Gardening_SoilComp>().Props.mySoilFertility && pawn.CanReserveAndReach(planter, PathEndMode.Touch, Danger.Deadly)) {
                    return planter;
                }
            }
            return null;
        }
    }
    [DefOf]
    class Gardening_JobDefOf {
        public static JobDef Gardening_SoilJob;
    }
}
