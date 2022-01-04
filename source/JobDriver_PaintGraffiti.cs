using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;

namespace GraffitiMod
{
    public class JobDriver_PaintGraffiti : JobDriver
    {
        private float workLeft = -1000f;
        protected const int BaseWorkAmount = 500;

        public override bool TryMakePreToilReservations(bool errorOnFailed) => this.pawn.Reserve(this.job.targetA, this.job, errorOnFailed: errorOnFailed);

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
            
            Toil doWork = new Toil()
            {
                initAction = (Action) (() => this.workLeft = BaseWorkAmount)
            };
            doWork.tickAction = (Action) (() =>
            {
                this.workLeft -= doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed) * 1.0f;
                if ((double) this.workLeft <= 0.0)
                {
                    if (pawn.MentalState is MentalState_GraffitiPaintingSpree mentalState)
                            mentalState.Notify_PaintedTarget();

                    Thing newThing = ThingMaker.MakeThing(GraffitiDefOf.GraffitiMod_Paint);
                    GenSpawn.Spawn(newThing, this.TargetLocA, this.Map);
                    this.pawn.needs.mood.thoughts.memories.TryGainMemory(DefDatabase<ThoughtDef>.GetNamed("GraffitiMod_HappyArtist"));
                    this.ReadyForNextToil();
                }
                else
                {
                    /*
                     Having a mental break is not a fun recreation activity.
                     Also: Pawns would stop the painting spree if their recreation were full. 
                     */
                    if (!(pawn.MentalState is MentalState_GraffitiPaintingSpree))
                        JoyUtility.JoyTickCheckEnd(this.pawn);
                }
            });
            doWork.defaultCompleteMode = ToilCompleteMode.Never;
            doWork.FailOnCannotTouch<Toil>(TargetIndex.A, PathEndMode.Touch);
            doWork.activeSkill = (Func<SkillDef>) (() => SkillDefOf.Artistic);
            yield return doWork;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<float>(ref this.workLeft, "workLeft");
        }
    }
}