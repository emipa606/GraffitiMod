using RimWorld;
using Verse;
using Verse.AI;

namespace GraffitiMod
{
    public class JobGiver_GraffitiPaintingSpree : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            MentalState_GraffitiPaintingSpree mentalState = pawn.MentalState as MentalState_GraffitiPaintingSpree;

            if (mentalState is null)
            {
                Verse.Log.Error("Mental state MentalState_GraffitiPaintingSpree expected but not found. Please report this to the mod creator via Steam.");
                return null;
            }
            
            IntVec3 paintWallCell = GraffitiUtility.TryFindPaintWallCell(pawn, 30f);
            if (paintWallCell == IntVec3.Invalid)
            {
                mentalState.Notify_InvalidTarget();
                return null;
            }

            return !paintWallCell.IsValid ? (Job) null : JobMaker.MakeJob(GraffitiDefOf.GraffitiMod_PaintGraffitiJob, (LocalTargetInfo) paintWallCell);
        }
        
    }
}