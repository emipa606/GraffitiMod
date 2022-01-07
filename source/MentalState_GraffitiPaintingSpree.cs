using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace GraffitiMod
{
    public class MentalState_GraffitiPaintingSpree : MentalState
    {
        public Thing target;
        
        public override RandomSocialMode SocialModeMax() => RandomSocialMode.Off;
        
        public override void MentalStateTick()
        {
            if (target == null)
                this.ChooseNextTarget();
            else if (!target.Spawned)
                this.ChooseNextTarget();

            base.MentalStateTick();
        }
        
        private void ChooseNextTarget()
        {
            List<Thing> possibleTargets = GraffitiUtility.GetListOfViableWalls(pawn, 30f);
            if (possibleTargets.Count > 0)
                target = possibleTargets.RandomElement();
            else
                target = null;
        }
        
        public void Notify_InvalidTarget ()
        {
            ChooseNextTarget();
        }
        
        public void Notify_PaintedTarget ()
        {
            ChooseNextTarget();
        }
        
    }
}