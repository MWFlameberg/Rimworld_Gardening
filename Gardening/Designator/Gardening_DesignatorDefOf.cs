using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Rimworld_Gardening {
    [DefOf]
    public static class Gardening_DesignatorDefOf {
        public static Gardening_DesignatorDef Gardening_DigSoil;
        public static Gardening_DesignatorDef Gardening_FillSoil;
    }
    public class Gardening_DesignatorDef : DesignationDef {
        public override void PostLoad() {
            base.PostLoad();
        }
    }
}
