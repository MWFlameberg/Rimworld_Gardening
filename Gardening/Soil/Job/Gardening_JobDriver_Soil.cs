using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Rimworld_Gardening {
    internal class Gardening_JobDriver_Soil : JobDriver {
        private int DiggingDuration = 2000;
        public override bool TryMakePreToilReservations(bool errorOnFailed) {
            return this.pawn.Reserve(this.job.GetDestination(this.pawn), this.job, 1, -1, null);
        }
        protected override IEnumerable<Toil> MakeNewToils() {
            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
            Toil dig = ToilMaker.MakeToil("MakeNewToils");
            dig.tickAction = delegate {
                if (DiggingDuration <= -100) {
                    ResetTicksToDig();
                }
                DiggingDuration--;
                if (DiggingDuration <= 0) {
                    Gardening_Soil soil = new Gardening_Soil();
                    Thing thing = ThingMaker.MakeThing(soil.def);
                    ReadyForNextToil();
                }
                
            };
            dig.WithProgressBar(TargetIndex.A, () => 1f - (float)DiggingDuration / 2000f);
            dig.FailOnCannotTouch(TargetIndex.A, PathEndMode.OnCell);
            yield return dig;
        }
        private void ResetTicksToDig() {
            float num = pawn.GetStatValue(StatDefOf.MiningSpeed);
            if (num < 0.6f && pawn.Faction != Faction.OfPlayer) {
                num = 0.6f;
            }
            DiggingDuration = (int)Math.Round(2000f / num);
        }
    }
}
