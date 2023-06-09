using RimWorld;
using Verse;
using Verse.AI;

namespace GraffitiMod;

public class MentalState_GraffitiPaintingSpree : MentalState
{
    public Thing target;

    public override RandomSocialMode SocialModeMax()
    {
        return RandomSocialMode.Off;
    }

    public override void MentalStateTick()
    {
        if (target == null)
        {
            ChooseNextTarget();
        }
        else if (!target.Spawned)
        {
            ChooseNextTarget();
        }

        base.MentalStateTick();
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