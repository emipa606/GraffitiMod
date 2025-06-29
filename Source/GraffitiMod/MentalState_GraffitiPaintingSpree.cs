using RimWorld;
using Verse;
using Verse.AI;

namespace GraffitiMod;

public class MentalState_GraffitiPaintingSpree : MentalState
{
    private Thing target;

    public override RandomSocialMode SocialModeMax()
    {
        return RandomSocialMode.Off;
    }

    public override void MentalStateTick(int delta)
    {
        if (target is not { Spawned: true })
        {
            ChooseNextTarget();
        }

        base.MentalStateTick(delta);
    }

    private void ChooseNextTarget()
    {
        var listOfViableWalls = GraffitiUtility.GetListOfViableWalls(pawn, 30f);
        target = listOfViableWalls.Count > 0 ? listOfViableWalls.RandomElement() : null;
    }

    public void Notify_InvalidTarget()
    {
        ChooseNextTarget();
    }

    public void Notify_PaintedTarget()
    {
        ChooseNextTarget();
    }
}