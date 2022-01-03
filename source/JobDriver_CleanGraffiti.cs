using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace GraffitiMod
{
  /*
   *  Copy of JobDriver_CleanFilth (modifications noted below)
   */
  
  public class JobDriver_CleanGraffiti : JobDriver
  {
    private float cleaningWorkDone;
    private float totalCleaningWorkDone;
    private float totalCleaningWorkRequired;
    private const TargetIndex FilthInd = TargetIndex.A;
    
    //mod start
    private Filth_Graffiti Graffiti => (Filth_Graffiti) this.job.GetTarget(TargetIndex.A).Thing;
    //mod end
    
    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
      this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job);
      return true;
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
      Toil initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A);
      yield return initExtractTargetFromQueue;
      yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
      yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A);
      yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch)
        .JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue)
        .JumpIfOutsideHomeArea(TargetIndex.A, initExtractTargetFromQueue);
      Toil clean = new Toil()
      {
        initAction = (Action) (() =>
        {
          this.cleaningWorkDone = 0.0f;
          this.totalCleaningWorkDone = 0.0f;
          //mod start
          this.totalCleaningWorkRequired = this.Graffiti.def.filth.cleaningWorkToReduceThickness;
          //mod end
        })
      };
      clean.tickAction = (Action) (() =>
      {
        //mod start
        Filth_Graffiti filth = this.Graffiti;
        //mod end
        ++this.cleaningWorkDone;
        ++this.totalCleaningWorkDone;
        if ((double) this.cleaningWorkDone <= (double) filth.def.filth.cleaningWorkToReduceThickness)
          return;
        this.cleaningWorkDone = 0.0f;
        //mod start
        filth.Destroy(DestroyMode.Vanish);
        // if (!filth.Destroyed)
        //   return;
        // clean.actor.records.Increment(RecordDefOf.MessesCleaned);
        //mod end
        this.ReadyForNextToil();
      });
      clean.defaultCompleteMode = ToilCompleteMode.Never;
      clean.WithEffect(EffecterDefOf.Clean, TargetIndex.A);
      clean.WithProgressBar(TargetIndex.A,
        (Func<float>) (() => this.totalCleaningWorkDone / this.totalCleaningWorkRequired), true);
      clean.PlaySustainerOrSound((Func<SoundDef>) (() =>
      {
        //mod start
        ThingDef def = this.Graffiti.def;
        //mod end
        return !def.filth.cleaningSound.NullOrUndefined() ? def.filth.cleaningSound : SoundDefOf.Interact_CleanFilth;
      }));
      clean.JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue);
      clean.JumpIfOutsideHomeArea(TargetIndex.A, initExtractTargetFromQueue);
      yield return clean;
      yield return Toils_Jump.Jump(initExtractTargetFromQueue);
    }

    public override void ExposeData()
    {
      base.ExposeData();
      Scribe_Values.Look<float>(ref this.cleaningWorkDone, "cleaningWorkDone");
      Scribe_Values.Look<float>(ref this.totalCleaningWorkDone, "totalCleaningWorkDone");
      Scribe_Values.Look<float>(ref this.totalCleaningWorkRequired, "totalCleaningWorkRequired");
    }
    
    
  }
}