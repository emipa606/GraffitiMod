using System.Collections.Generic;
using RimWorld;
using Verse;

namespace GraffitiMod;

public static class GraffitiUtility
{
    public static List<Thing> GetListOfViableWalls(Pawn pawn, float maxDist)
    {
        if (!CellFinder.TryFindClosestRegionWith(pawn.GetRegion(), TraverseParms.For(pawn),
                r => r.Room.PsychologicallyOutdoors, 100, out var rootReg))
        {
            return null;
        }

        var result = new List<Thing>();
        RegionTraverser.BreadthFirstTraverse(rootReg, (_, r) => r.District == rootReg.District, delegate
        {
            var list = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Wall);
            var list2 = new List<Thing>();
            foreach (var thing in list)
            {
                if (thing.Position.InHorDistOf(pawn.Position, maxDist) && isGoodPaintWallCell(thing.Position, pawn))
                {
                    list2.Add(thing);
                }
            }

            if (list2.Count <= 0)
            {
                return false;
            }

            result = list2;
            return true;
        }, 30);
        return result;
    }

    public static IntVec3 TryFindPaintWallCell(Pawn pawn, float maxDist)
    {
        var result = IntVec3.Invalid;
        List<Thing> listOfViableWalls;
        if ((listOfViableWalls = GetListOfViableWalls(pawn, maxDist)) != null && listOfViableWalls.Count > 0)
        {
            result = listOfViableWalls.RandomElement().Position;
        }

        return result;
    }

    private static bool isGoodPaintWallCell(IntVec3 c, Pawn pawn)
    {
        if (c.IsForbidden(pawn))
        {
            return false;
        }

        for (var i = 0; i < 9; i++)
        {
            var intVec = c + GenAdj.AdjacentCellsAndInside[i];
            if (!intVec.InBounds(pawn.Map) || pawn.Map.reservationManager.IsReservedAndRespected(intVec, pawn))
            {
                return false;
            }
        }

        return true;
    }
}