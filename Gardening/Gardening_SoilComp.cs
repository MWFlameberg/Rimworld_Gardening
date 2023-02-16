using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Rimworld_Gardening {
    public class Gardening_SoilComp : ThingComp {
        public Gardening_SoilCompProperties Props => (Gardening_SoilCompProperties)this.Props;
        public float soilFertility => Props.mySoilFertility;
        private static TargetingParameters TargetingParams => new TargetingParameters {
            canTargetPawns = false,
            canTargetLocations = true
        };
        public override void CompTick() {
            base.CompTick();
        }
        public override IEnumerable<Gizmo> CompGetGizmosExtra() {
            foreach (Gizmo item in base.CompGetGizmosExtra()) {
                yield return item;
            }
            Command_Action command_Action = new Command_Action();
            command_Action.defaultLabel = "70%";
            command_Action.defaultDesc = "70&";
            command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower");
            command_Action.action = delegate {
                Find.WindowStack.Add(new FloatMenu(GetFloatMenuOptions()));
            };

            yield return command_Action;
        }
        private List<FloatMenuOption> GetFloatMenuOptions() {
            List<FloatMenuOption> floatMenuOptions = new List<FloatMenuOption>();
            Action action70 = delegate {
                Props.mySoilFertility = 0.7f;
            };
            Action action100 = delegate {
                Props.mySoilFertility = 1.0f;
            };
            Action action140 = delegate {
                Props.mySoilFertility = 1.4f;
            };
            floatMenuOptions.Add(new FloatMenuOption("70", null));
            floatMenuOptions.Add(new FloatMenuOption("100", null));
            floatMenuOptions.Add(new FloatMenuOption("140", null));
            return floatMenuOptions;
        }
    }
    public class Gardening_SoilCompProperties : CompProperties {
        public float mySoilFertility;
        public Gardening_SoilCompProperties() {
            this.compClass = typeof(Gardening_SoilComp);
        }
        public Gardening_SoilCompProperties(Type compClass) : base(compClass) {
            this.compClass = compClass;
        }
    }

}
