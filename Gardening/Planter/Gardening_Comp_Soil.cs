using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Rimworld_Gardening {
    public class Gardening_Comp_Soil : ThingComp {
        public Gardening_CompProperties_Soil Props => (Gardening_CompProperties_Soil)props;
        private float configuredSoilCapacity = -1f;
        private float soilFertility;
        private float soil;
        public float Soil => soil;
        public bool HasSoil {
            get {
                return soil > Props.soilCapacity;
            }
        }
        public bool IsFull {
            get {
                if (HasSoil) {
                    return soil == Props.soilCapacity;
                }
                return false;
            }
        }
        public int GetSoilCountToFullyFill() {
            return Mathf.CeilToInt(Props.soilCapacity - soil);
        }
        public override void Initialize(CompProperties props) {
            base.Initialize(props);
            soilFertility = 0f;
            soil = 0f;
            configuredSoilCapacity = Props.soilCapacity;
        }
        public override void CompTick() {
            base.CompTick();
        }
        public void FillSoil(List<Thing> soilThings) {
            int soilToCap = GetSoilCountToFullyFill();
            while (soilToCap > 0 && soilThings.Count > 0) {
                Thing thing = soilThings.Pop();
                int soilToAdd = thing.stackCount;
                FillSoil(Mathf.Min(soilToCap, soilToAdd));
                thing.SplitOff(soilToCap).Destroy();
                soilToCap -= soilToAdd;
            }
        }
        public void FillSoil(float amount) {
            soil += amount;
            if (soil > Props.soilCapacity) {
                soil = Props.soilCapacity;
            }
        }
        public override IEnumerable<Gizmo> CompGetGizmosExtra() {
            foreach (Gizmo item in base.CompGetGizmosExtra()) {
                yield return item;
            }
            Command_Action command_Action = new Command_Action();
            command_Action.defaultLabel = string.Format("{0}: {1}", "Gardening_SoilFertility".Translate(), soilFertility.ToStringPercent());
            command_Action.defaultDesc = string.Format("{0}: {1}", "Gardening_SoilFertility".Translate(), soilFertility.ToStringPercent());
            command_Action.action = delegate {
                Find.WindowStack.Add(new FloatMenu(GetFloatMenuOptions()));
            };

            yield return command_Action;
        }
        private List<FloatMenuOption> GetFloatMenuOptions() {
            List<FloatMenuOption> floatMenuOptions = new List<FloatMenuOption>();
            floatMenuOptions.Add(new FloatMenuOption("70", delegate {
                soilFertility = 0.7f;
            }));
            floatMenuOptions.Add(new FloatMenuOption("100", delegate {
                soilFertility = 1.0f;
            }));
            floatMenuOptions.Add(new FloatMenuOption("140", delegate {
                soilFertility = 1.4f;
            }));
            return floatMenuOptions;
        }
    }
    public class Gardening_CompProperties_Soil : CompProperties {
        public float soilCapacity;

        public Gardening_CompProperties_Soil() {
            this.compClass = typeof(Gardening_Comp_Soil);
        }
        public Gardening_CompProperties_Soil(Type compClass) : base(compClass) {
            this.compClass = compClass;
        }
    }
}
