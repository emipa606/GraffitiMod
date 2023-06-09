using RimWorld;
using Verse;

namespace GraffitiMod;

public class Filth_Graffiti : Building_Art
{
    public Building Parent { get; private set; }

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        Parent = Position.GetEdifice(map);
    }

    public override void Tick()
    {
        if (Parent.DestroyedOrNull())
        {
            Destroy();
        }
    }
}