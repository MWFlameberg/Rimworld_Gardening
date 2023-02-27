using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Rimworld_Gardening {
    public class Gardening_Planter_SoilComp : ThingComp {
        public Gardening_Planter_SoilCompProperties Props => (Gardening_Planter_SoilCompProperties)this.Props;
        public float soilFertility;
        private static TargetingParameters TargetingParams => new TargetingParameters {
            canTargetPawns = false,
            canTargetLocations = true
        };
        public override void Initialize(CompProperties props) {
            base.Initialize(props);
        }
        public override void CompTick() {
            base.CompTick();
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
    public class Gardening_Planter_SoilCompProperties : CompProperties {
        public float mySoilFertility;
        public Gardening_Planter_SoilCompProperties() {
            this.compClass = typeof(Gardening_Planter_SoilComp);
        }
        public Gardening_Planter_SoilCompProperties(Type compClass) : base(compClass) {
            this.compClass = compClass;
        }
    }
}
