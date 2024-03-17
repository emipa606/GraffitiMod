using Verse;
using Verse.AI;

namespace GraffitiMod;

public class JobGiver_GraffitiPaintingSpree : ThinkNode_JobGiver
{
    protected override Job TryGiveJob(Pawn pawn)
    {
        if (pawn.MentalState is not MentalState_GraffitiPaintingSpree mentalState_GraffitiPaintingSpree)
        {
            Log.Error(
                "Mental state MentalState_GraffitiPaintingSpree expected but not found. Please report this to the mod creator via Steam.");
            return null;
        }

        var intVec = GraffitiUtility.TryFindPaintWallCell(pawn, 30f);
        if (intVec != IntVec3.Invalid)
        {
            return !intVec.IsValid ? null : JobMaker.MakeJob(GraffitiDefOf.GraffitiMod_PaintGraffitiJob, intVec);
        }

        mentalState_GraffitiPaintingSpree.Notify_InvalidTarget();
        return null;
    }
}