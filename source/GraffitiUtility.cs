using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace GraffitiMod
{
  public static class GraffitiUtility
  {

    public static List<Thing> GetListOfViableWalls(Pawn pawn, float maxDist)
    {
      if (!CellFinder.TryFindClosestRegionWith(pawn.GetRegion(), TraverseParms.For(pawn), (Predicate<Region>) (r => r.Room.PsychologicallyOutdoors), 100, out var rootReg))
        return null;
      List<Thing> result = new List<Thing>();
      RegionTraverser.BreadthFirstTraverse(rootReg, (RegionEntryPredicate) ((from, r) => r.District == rootReg.District), (RegionProcessor) (r =>
      {
        List<Thing> wallList = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Wall);
        List<Thing> viableWallsList = new List<Thing>();
        for (int index = 0; index < wallList.Count; ++index)
        {
          if (wallList[index].Position.InHorDistOf(pawn.Position, maxDist))
          {
            if (IsGoodPaintWallCell(wallList[index].Position, pawn))
              viableWallsList.Add(wallList[index]);
          }
        }
        if (viableWallsList.Count > 0)
        {
          result = viableWallsList;
          return true;
        }
        
        return false;
      }), 30);
      
      return result;
    }
    
    public static IntVec3 TryFindPaintWallCell(Pawn pawn, float maxDist)
    {
      List<Thing> viableWallsList;
      IntVec3 result = IntVec3.Invalid;
      
      if ((viableWallsList = GetListOfViableWalls(pawn, maxDist)) != null)
      {
        result = viableWallsList.RandomElement().Position;
      }
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
