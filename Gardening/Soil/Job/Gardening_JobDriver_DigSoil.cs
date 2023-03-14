using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Rimworld_Gardening {
    internal class Gardening_JobDriver_DigSoil : JobDriver {
        private int WorkDone;
        private int WorkSpeed;
        private const int BaseWorkSpeed = 50;
        private const int WorkNeeded = 20000;
        public override bool TryMakePreToilReservations(bool errorOnFailed) {
            return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
        }
        protected override IEnumerable<Toil> MakeNewToils() {
            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
            WorkSpeed = (int)(BaseWorkSpeed * pawn.GetStatValue(StatDefOf.MiningSpeed));
            Toil dig = ToilMaker.MakeToil("MakeNewToils");
            dig.tickAction = delegate {
                WorkDone += WorkSpeed;
                if (WorkDone >= WorkNeeded) {
                    Thing thing = ThingMaker.MakeThing(ThingDef.Named("Gardening_Soil"));
                    thing.stackCount = 25;
                    GenPlace.TryPlaceThing(thing, pawn.Position, base.Map, ThingPlaceMode.Near);
                    base.Map.terrainGrid.SetTerrain(job.targetA.Cell, TerrainDef.Named("Gardening_DugSoil"));
                    base.Map.designationManager.DesignationAt(job.targetA.Cell, Gardening_DesignatorDefOf.Gardening_DigSoil).Delete();
                    ReadyForNextToil();
                }
            };
            dig.defaultCompleteMode = ToilCompleteMode.Never;
            dig.WithEffect(EffecterDefOf.Clean, TargetIndex.A);
            dig.PlaySustainerOrSound(() => SoundDefOf.Interact_CleanFilth);
            dig.WithProgressBar(TargetIndex.A, () => WorkDone / WorkNeeded, interpolateBetweenActorAndTarget: true);
            dig.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            yield return dig;
        }
    }
}
