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

    private Filth_Graffiti Graffiti => (Filth_Graffiti)job.GetTarget(FilthInd).Thing;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        pawn.ReserveAsManyAsPossible(job.GetTargetQueue(FilthInd), job);
        return true;
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        var initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(FilthInd);
        yield return initExtractTargetFromQueue;
        yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(FilthInd);
        yield return Toils_JobTransforms.ExtractNextTargetFromQueue(FilthInd);
        yield return Toils_Goto.GotoThing(FilthInd, PathEndMode.Touch)
            .JumpIfDespawnedOrNullOrForbidden(FilthInd, initExtractTargetFromQueue)
            .JumpIfOutsideHomeArea(FilthInd, initExtractTargetFromQueue);
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
        clean.WithEffect(EffecterDefOf.Clean, FilthInd);
        clean.WithProgressBar(FilthInd, () => totalCleaningWorkDone / totalCleaningWorkRequired, true);
        clean.PlaySustainerOrSound(delegate
        {
            var def = Graffiti.def;
            return !def.filth.cleaningSound.NullOrUndefined()
                ? def.filth.cleaningSound
                : SoundDefOf.Interact_CleanFilth;
        });
        clean.JumpIfDespawnedOrNullOrForbidden(FilthInd, initExtractTargetFromQueue);
        clean.JumpIfOutsideHomeArea(FilthInd, initExtractTargetFromQueue);
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