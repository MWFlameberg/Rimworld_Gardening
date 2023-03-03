using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Rimworld_Gardening {
    internal class Gardening_JobDriver_DigSoil : JobDriver {
        private float WorkDone;
        private const float WorkNeeded = 20000f;
        public override bool TryMakePreToilReservations(bool errorOnFailed) {
            pawn.Map.pawnDestinationReservationManager.Reserve(pawn, job, job.targetA.Cell);
            return true;
        }
        protected override IEnumerable<Toil> MakeNewToils() {
            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
            Toil dig = ToilMaker.MakeToil("MakeNewToils");
            dig.tickAction = delegate {
                WorkDone += 10;
                if (WorkDone >= WorkNeeded) {
                    Thing thing = ThingMaker.MakeThing(ThingDef.Named("Gardening_Soil"));
                    thing.stackCount = 50;
                    GenPlace.TryPlaceThing(thing, pawn.Position, base.Map, ThingPlaceMode.Near);
                    base.Map.terrainGrid.SetTerrain(job.targetA.Cell, TerrainDef.Named("Gardening_DugSoil"));
                    base.Map.designationManager.DesignationAt(job.targetA.Cell, Gardening_DesignatorDefOf.Gardening_DigSoil).Delete();
                    ReadyForNextToil();
                }
            };
            dig.defaultCompleteMode = ToilCompleteMode.Never;
            dig.WithEffect(EffecterDefOf.Mine, TargetIndex.A);
            dig.PlaySustainerOrSound(() => SoundDefOf.Interact_CleanFilth);
            dig.WithProgressBar(TargetIndex.A, () => WorkDone / WorkNeeded, interpolateBetweenActorAndTarget: true);
            dig.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            yield return dig;
        }
    }
}
