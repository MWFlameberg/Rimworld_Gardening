using RimWorld;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace Rimworld_Gardening {
    public class Gardening_Planter : Building_PlantGrower {
        private ThingDef plantDefToGrow;
        public override string GetInspectString() {
            StringBuilder stringBuilder = new StringBuilder();

            ContentsStatistics(out var totalPlants, out var averagePlantAgeTicks, out var oldestPlantAgeTicks, out var averagePlantGrowth, out var maxPlantGrowth);
            if (totalPlants > 0) {
                string arg = (averagePlantAgeTicks / 3600000f).ToStringApproxAge();
                string arg2 = ((float)oldestPlantAgeTicks / 3600000f).ToStringApproxAge();
                stringBuilder.AppendLine(string.Format("{0}: {1} {2}", "Contains".Translate().CapitalizeFirst(), totalPlants, Find.ActiveLanguageWorker.Pluralize(plantDefToGrow.label, totalPlants)));
                stringBuilder.AppendLine(string.Format("{0}: {1} ({2})", "AveragePlantAge".Translate().CapitalizeFirst(), arg, "PercentGrowth".Translate(averagePlantGrowth.ToStringPercent())));
                stringBuilder.AppendLine(string.Format("{0}: {1} ({2})", "OldestPlantAge".Translate().CapitalizeFirst(), arg2, "PercentGrowth".Translate(maxPlantGrowth.ToStringPercent())));
            }

            if (base.InteractionCell.UsesOutdoorTemperature(base.Map)) {
                stringBuilder.AppendLine("OutdoorGrowingPeriod".Translate() + ": " + GrowingQuadrumsDescription(base.Map.Tile));
            }
            if (PlantUtility.GrowthSeasonNow(base.InteractionCell, base.Map, forSowing: true)) {
                stringBuilder.AppendLine("GrowSeasonHereNow".Translate());
            }
            else {
                stringBuilder.AppendLine("CannotGrowBadSeasonTemperature".Translate());
            }
            stringBuilder.Append(string.Format("{0}: {1}", "Fertility_Label".Translate(), base.Map.fertilityGrid.FertilityAt(base.Position).ToStringPercent()));
            return stringBuilder.ToString();
        }
        public void ContentsStatistics(out int totalPlants, out float averagePlantAgeTicks, out int oldestPlantAgeTicks, out float averagePlantGrowth, out float maxPlantGrowth) {
            averagePlantAgeTicks = 0f;
            totalPlants = 0;
            oldestPlantAgeTicks = 0;
            averagePlantGrowth = 0f;
            maxPlantGrowth = 0f;
            foreach (Thing thing in base.InteractionCell.GetThingList(base.Map)) {
                if (thing.def == plantDefToGrow && thing is Plant plant) {
                    totalPlants++;
                    averagePlantAgeTicks += plant.Age;
                    oldestPlantAgeTicks = Mathf.Max(oldestPlantAgeTicks, plant.Age);
                    averagePlantGrowth += plant.Growth;
                    maxPlantGrowth = Mathf.Max(maxPlantGrowth, plant.Growth);
                }
            }
            averagePlantGrowth /= totalPlants;
            averagePlantAgeTicks /= totalPlants;
        }
        public static string GrowingQuadrumsDescription(int tile) {
            List<Twelfth> list = GenTemperature.TwelfthsInAverageTemperatureRange(tile, 6f, 42f);
            if (list.NullOrEmpty()) {
                return "NoGrowingPeriod".Translate();
            }
            if (list.Count == 12) {
                return "GrowYearRound".Translate();
            }
            return "PeriodDays".Translate(list.Count * 5 + "/" + 60) + " (" + QuadrumUtility.QuadrumsRangeLabel(list) + ")";
        }
    }
}
