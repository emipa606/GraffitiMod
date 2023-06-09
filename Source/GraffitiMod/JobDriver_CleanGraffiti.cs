using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace GraffitiMod;

public class JobDriver_CleanGraffiti : JobDriver
{
    private const TargetIndex FilthInd = TargetIndex.A;
    private float cleaningWorkDone;

    private float totalCleaningWorkDone;

    private float totalCleaningWorkRequired;

    private Filth_Graffiti Graffiti => (Filth_Graffiti)job.GetTarget(TargetIndex.A).Thing;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        pawn.ReserveAsManyAsPossible(job.GetTargetQueue(TargetIndex.A), job);
        return true;
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        var initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A);
        yield return initExtractTargetFromQueue;
        yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
        yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A);
        yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch)
            .JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue)
            .JumpIfOutsideHomeArea(TargetIndex.A, initExtractTargetFromQueue);
        var clean = new Toil
        {
            initAction = delegate
            {
                cleaningWorkDone = 0f;
                totalCleaningWorkDone = 0f;
                totalCleaningWorkRequired = Graffiti.def.filth.cleaningWorkToReduceThickness;
            },
            tickAction = delegate
            {
                var graffiti = Graffiti;
                cleaningWorkDone += 1f;
                totalCleaningWorkDone += 1f;
                if (cleaningWorkDone <= (double)graffiti.def.filth.cleaningWorkToReduceThickness)
                {
                    return;
                }

                cleaningWorkDone = 0f;
                graffiti.Destroy();
                ReadyForNextToil();
            },
            defaultCompleteMode = ToilCompleteMode.Never
        };
        clean.WithEffect(EffecterDefOf.Clean, TargetIndex.A);
        clean.WithProgressBar(TargetIndex.A, () => totalCleaningWorkDone / totalCleaningWorkRequired, true);
        clean.PlaySustainerOrSound(delegate
        {
            var def = Graffiti.def;
            return !def.filth.cleaningSound.NullOrUndefined()
                ? def.filth.cleaningSound
                : SoundDefOf.Interact_CleanFilth;
        });
        clean.JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue);
        clean.JumpIfOutsideHomeArea(TargetIndex.A, initExtractTargetFromQueue);
        yield return clean;
        yield return Toils_Jump.Jump(initExtractTargetFromQueue);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref cleaningWorkDone, "cleaningWorkDone");
        Scribe_Values.Look(ref totalCleaningWorkDone, "totalCleaningWorkDone");
        Scribe_Values.Look(ref totalCleaningWorkRequired, "totalCleaningWorkRequired");
    }
}