using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;
using static UnityEngine.GraphicsBuffer;
using System.Reflection.Emit;
using System.Security.Principal;
using UnityEngine;

namespace Rimworld_Gardening {
    internal class Gardening_JobDriver_FillSoil : JobDriver {
        private int WorkDone;
        private int WorkSpeed;
        private const int BaseWorkSpeed = 50;
        private const int WorkNeeded = 20000;
        public override bool TryMakePreToilReservations(bool errorOnFailed) {
            if (pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed)) {
                pawn.ReserveAsManyAsPossible(job.targetQueueB, job, 1, -1, null);
                return true;
            }
            return false;
        }
        protected override IEnumerable<Toil> MakeNewToils() {
            Toil reserveSoil = Toils_Reserve.ReserveQueue(TargetIndex.B, 1, 25, null);
            foreach (Toil item in CollectSoilToils(TargetIndex.B, subtractNumTakenFromJobCount: false, failIfStackCountLessThanJobCount: true)) {
                yield return item;
            }
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

        public static IEnumerable<Toil> CollectSoilToils(TargetIndex soilIndex, bool subtractNumTakenFromJobCount = false, bool failIfStackCountLessThanJobCount = false, bool placeInBillGiver = false) {
            Toil extract = Toils_JobTransforms.ExtractNextTargetFromQueue(soilIndex);
            yield return extract;
            Toil gotoHaulTarget = Toils_Goto.GotoThing(soilIndex, PathEndMode.ClosestTouch);
            yield return gotoHaulTarget;
            yield return Toils_Haul.StartCarryThing(TargetIndex.B, putRemainderInQueue: true, subtractNumTakenFromJobCount, failIfStackCountLessThanJobCount, reserve: false);
            yield return JumpToCollectNextIntoHandsForBill(gotoHaulTarget, soilIndex);
            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
        }
        public static Toil JumpToCollectNextIntoHandsForBill(Toil gotoGetTargetToil, TargetIndex ind) {
            Toil toil = ToilMaker.MakeToil("JumpToCollectNextIntoHandsForBill");
            toil.initAction = delegate
            {
                Pawn actor = toil.actor;
                if (actor.carryTracker.CarriedThing == null) {
                    Log.Error(string.Concat("JumpToAlsoCollectTargetInQueue run on ", actor, " who is not carrying something."));
                }
                else if (!actor.carryTracker.Full) {
                    Job curJob = actor.jobs.curJob;
                    List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
                    Log.Message(targetQueue.Count.ToString());

                    if (!targetQueue.NullOrEmpty()) {
                        for (int i = 0; i < targetQueue.Count; i++) {
                            Log.Message(targetQueue[i].ToStringSafe());
                            if (GenAI.CanUseItemForWork(actor, targetQueue[i].Thing) && targetQueue[i].Thing.CanStackWith(actor.carryTracker.CarriedThing) && !((float)(actor.Position - targetQueue[i].Thing.Position).LengthHorizontalSquared > 64f)) {
                                int num = ((actor.carryTracker.CarriedThing != null) ? actor.carryTracker.CarriedThing.stackCount : 0);
                                int a = curJob.countQueue[i];
                                a = Mathf.Min(a, targetQueue[i].Thing.def.stackLimit - num);
                                a = Mathf.Min(a, actor.carryTracker.AvailableStackSpace(targetQueue[i].Thing.def));
                                if (a > 0) {
                                    curJob.count = a;
                                    curJob.SetTarget(ind, targetQueue[i].Thing);
                                    curJob.countQueue[i] -= a;
                                    if (curJob.countQueue[i] <= 0) {
                                        curJob.countQueue.RemoveAt(i);
                                        targetQueue.RemoveAt(i);
                                    }
                                    actor.jobs.curDriver.JumpToToil(gotoGetTargetToil);
                                    break;
                                }
                            }
                        }
                    }
                }
            };
            return toil;
        }
    }
}
