using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace GraffitiMod;

public class MentalStateWorker_GraffitiPaintingSpree : MentalStateWorker
{
    private static List<Thing> tmpThings = new List<Thing>();

    public override bool StateCanOccur(Pawn pawn)
    {
        if (!base.StateCanOccur(pawn))
        {
            return false;
        }

        tmpThings.Clear();
        tmpThings = GraffitiUtility.GetListOfViableWalls(pawn, 30f);
        var num = tmpThings.Count >= 2 ? 1 : 0;
        tmpThings.Clear();
        return num != 0;
    }
}