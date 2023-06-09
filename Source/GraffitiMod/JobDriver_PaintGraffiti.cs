using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace GraffitiMod;

public class JobDriver_PaintGraffiti : JobDriver
{
    protected const int BaseWorkAmount = 500;
    private float workLeft = -1000f;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
        var doWork = new Toil
        {
            initAction = delegate { workLeft = 500f; }
        };
        doWork.tickAction = delegate
        {
            workLeft -= doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed) * 1f;
            if (workLeft <= 0.0)
            {
                if (pawn.MentalState is MentalState_GraffitiPaintingSpree mentalState_GraffitiPaintingSpree)
                {
                    mentalState_GraffitiPaintingSpree.Notify_PaintedTarget();
                }

                var newThing = ThingMaker.MakeThing(GraffitiDefOf.GraffitiMod_Paint);
                GenSpawn.Spawn(newThing, TargetLocA, Map);
                pawn.needs.mood.thoughts.memories.TryGainMemory(
                    DefDatabase<ThoughtDef>.GetNamed("GraffitiMod_HappyArtist"));
                ReadyForNextToil();
            }
            else if (!(pawn.MentalState is MentalState_GraffitiPaintingSpree))
            {
                JoyUtility.JoyTickCheckEnd(pawn);
            }
        };
        doWork.defaultCompleteMode = ToilCompleteMode.Never;
        doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
        doWork.activeSkill = () => SkillDefOf.Artistic;
        yield return doWork;
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref workLeft, "workLeft");
    }
}