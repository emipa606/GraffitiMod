using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace GraffitiMod
{
    public class JoyGiver_PaintGraffiti : JoyGiver
    {
      public override Job TryGiveJob(Pawn pawn)
      {
        if (pawn.WorkTypeIsDisabled(WorkTypeDefOf.Art))
          return (Job) null;
        //Only unhappy pawns create ugly graffiti, because beautiful wall art is not part of this mod (at this time).
        if (pawn.needs.mood.CurLevel > pawn.needs.mood.MaxLevel * 0.6f)
          return (Job) null;
        if (pawn.skills.AverageOfRelevantSkillsFor(WorkTypeDefOf.Art) < 3)
          return (Job) null;
        IntVec3 paintWallCell = TryFindPaintWallCell(pawn);
        return !paintWallCell.IsValid ? (Job) null : JobMaker.MakeJob(this.def.jobDef, (LocalTargetInfo) paintWallCell);
      }

      private static IntVec3 TryFindPaintWallCell(Pawn pawn)
      {
        Region rootReg;
        if (!CellFinder.TryFindClosestRegionWith(pawn.GetRegion(), TraverseParms.For(pawn), (Predicate<Region>) (r => r.Room.PsychologicallyOutdoors), 100, out rootReg))
          return IntVec3.Invalid;
        IntVec3 result = IntVec3.Invalid;
        RegionTraverser.BreadthFirstTraverse(rootReg, (RegionEntryPredicate) ((from, r) => r.District == rootReg.District), (RegionProcessor) (r =>
        {
          List<Thing> wallList = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Wall);
          List<Thing> wallsInProximityList = new List<Thing>();
          for (int index = 0; index < wallList.Count; ++index)
          {
            if (wallList[index].Position.InHorDistOf(pawn.Position, 30f))
              wallsInProximityList.Add(wallList[index]);
          }
          if (wallsInProximityList.Count == 0)
          {
            return false;
          }

          Thing targetWall = wallsInProximityList.RandomElement();
          if (IsGoodPaintWallCell(targetWall.Position, pawn))
          {
            result = targetWall.Position;
            return true;
          }

          return false;
        }), 30);
        return result;
      }

      private static bool IsGoodPaintWallCell(IntVec3 c, Pawn pawn)
      {
        if (c.IsForbidden(pawn))
          return false;
        
        for (int index = 0; index < 9; ++index)
        {
          IntVec3 intVec3 = c + GenAdj.AdjacentCellsAndInside[index];
          if (!intVec3.InBounds(pawn.Map) || pawn.Map.reservationManager.IsReservedAndRespected((LocalTargetInfo) intVec3, pawn))
            return false;
        }

        return true;
      }
  }
}