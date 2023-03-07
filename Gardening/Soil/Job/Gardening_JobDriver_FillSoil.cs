using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;

namespace Rimworld_Gardening {
    internal class Gardening_JobDriver_FillSoil : JobDriver {
        private int WorkDone;
        private int WorkSpeed;
        private const int BaseWorkSpeed = 50;
        private const int WorkNeeded = 20000;
        public override bool TryMakePreToilReservations(bool errorOnFailed) {
            if (pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed)) {
                return pawn.Reserve(job.targetB.Thing, job, 1, -1, null, errorOnFailed);
            }
            return false;
        }
        protected override IEnumerable<Toil> MakeNewToils() {
            Toil reserveSoil = Toils_Reserve.Reserve(TargetIndex.B);
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B, putRemainderInQueue: false, subtractNumTakenFromJobCount: true);
            yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveSoil, TargetIndex.B, TargetIndex.None, takeFromValidStorage: true);
            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
            WorkSpeed = (int)(BaseWorkSpeed * pawn.GetStatValue(StatDefOf.MiningSpeed));
            Toil fill = ToilMaker.MakeToil("MakeNewToils");
            fill.tickAction = delegate {
                WorkDone += WorkSpeed;
                if (WorkDone >= WorkNeeded) {
                    job.targetB.Thing.Destroy(DestroyMode.Vanish);
                    base.Map.terrainGrid.SetTerrain(job.targetA.Cell, TerrainDef.Named("Soil"));
                    base.Map.designationManager.DesignationAt(job.targetA.Cell, Gardening_DesignatorDefOf.Gardening_FillSoil).Delete();
                    ReadyForNextToil();
                }
            };
            fill.defaultCompleteMode = ToilCompleteMode.Never;
            fill.WithEffect(EffecterDefOf.Clean, TargetIndex.A);
            fill.PlaySustainerOrSound(() => SoundDefOf.Interact_CleanFilth);
            fill.WithProgressBar(TargetIndex.A, () => WorkDone / WorkNeeded, interpolateBetweenActorAndTarget: true);
            fill.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            yield return fill;
        }
    }
}
