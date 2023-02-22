using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Rimworld_Gardening.Gardening {
    internal class Gardening_SoilJobDriver : JobDriver {
        public override bool TryMakePreToilReservations(bool errorOnFailed) {
            throw new System.NotImplementedException();
        }
        protected override IEnumerable<Toil> MakeNewToils() {
            yield return null;
        }
    }
}
