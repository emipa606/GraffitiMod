using RimWorld;
using Verse;

namespace GraffitiMod;

public class Filth_Graffiti : Building_Art
{
    private Building Parent { get; set; }

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        Parent = Position.GetEdifice(map);
    }

    protected override void Tick()
    {
        if (Parent.DestroyedOrNull())
        {
            Destroy();
        }
    }
}