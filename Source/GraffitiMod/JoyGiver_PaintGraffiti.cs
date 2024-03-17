using RimWorld;
using Verse;
using Verse.AI;

namespace GraffitiMod;

public class JoyGiver_PaintGraffiti : JoyGiver
{
    public override Job TryGiveJob(Pawn pawn)
    {
        if (pawn.WorkTypeIsDisabled(GraffitiDefOf.Art))
        {
            return null;
        }

        if (pawn.needs.mood.CurLevel > pawn.needs.mood.MaxLevel * 0.6f)
        {
            return null;
        }

        if (pawn.skills.AverageOfRelevantSkillsFor(GraffitiDefOf.Art) < 3f)
        {
            return null;
        }

        var intVec = GraffitiUtility.TryFindPaintWallCell(pawn, 30f);
        return !intVec.IsValid ? null : JobMaker.MakeJob(def.jobDef, intVec);
    }
}