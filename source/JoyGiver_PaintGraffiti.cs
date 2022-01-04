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
        // if (pawn.WorkTypeIsDisabled(WorkTypeDefOf.Art))
        //   return (Job) null;
        //Only unhappy pawns create ugly graffiti, because beautiful wall art is not part of this mod (at this time).
        if (pawn.needs.mood.CurLevel > pawn.needs.mood.MaxLevel * 0.6f)
          return (Job) null;
        if (pawn.skills.AverageOfRelevantSkillsFor(WorkTypeDefOf.Art) < 3)
          return (Job) null;
        IntVec3 paintWallCell = GraffitiUtility.TryFindPaintWallCell(pawn, 30f);
        return !paintWallCell.IsValid ? (Job) null : JobMaker.MakeJob(this.def.jobDef, (LocalTargetInfo) paintWallCell);
      }
  }
}