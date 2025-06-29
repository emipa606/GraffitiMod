using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace GraffitiMod;

internal class WorkGiver_CleanGraffiti : WorkGiver_Scanner
{
    public override PathEndMode PathEndMode => PathEndMode.Touch;

    public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(GraffitiDefOf.GraffitiMod_Paint);

    public override int MaxRegionsToScanBeforeGlobalSearch => 4;

    public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
    {
        return pawn.Map.listerFilthInHomeArea.FilthInHomeArea;
    }

    public override bool ShouldSkip(Pawn pawn, bool forced = false)
    {
        return pawn.Map.listerFilthInHomeArea.FilthInHomeArea.Count == 0;
    }

    public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
    {
        return t is Filth_Graffiti filthGraffiti && filthGraffiti.Map.areaManager.Home[filthGraffiti.Position] &&
               pawn.CanReserve(t, 1, -1, null, forced);
    }

    public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
    {
        var job = JobMaker.MakeJob(GraffitiDefOf.GraffitiMod_CleanJob);
        job.AddQueuedTarget(TargetIndex.A, t);
        const int num = 15;
        var map = t.Map;
        var room = t.GetRoom();
        for (var i = 0; i < 100; i++)
        {
            var c2 = t.Position + GenRadial.RadialPattern[i];
            if (!shouldClean(c2))
            {
                continue;
            }

            var thingList = c2.GetThingList(map);
            foreach (var thing in thingList)
            {
                if (HasJobOnThing(pawn, thing, forced) && thing != t)
                {
                    job.AddQueuedTarget(TargetIndex.A, thing);
                }
            }

            if (job.GetTargetQueue(TargetIndex.A).Count >= num)
            {
                break;
            }
        }

        if (job.targetQueueA is { Count: >= 5 })
        {
            job.targetQueueA.SortBy(targ => targ.Cell.DistanceToSquared(pawn.Position));
        }

        return job;

        bool shouldClean(IntVec3 c)
        {
            if (!c.InBounds(map))
            {
                return false;
            }

            if (room == c.GetRoom(map))
            {
                return true;
            }

            var region = c.GetDoor(map)?.GetRegion(RegionType.Portal);
            if (region == null || region.links.NullOrEmpty())
            {
                return false;
            }

            foreach (var regionLink in region.links)
            {
                for (var l = 0; l < 2; l++)
                {
                    if (regionLink.regions[l] != null && !Equals(regionLink.regions[l], region) &&
                        regionLink.regions[l].valid && regionLink.regions[l].Room == room)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}