using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace GraffitiMod
{
  /*
   * This class is a copy of the internal class WorkGiver_CleanFilth (modifications noted below)
   */
  internal class WorkGiver_CleanGraffiti : WorkGiver_Scanner
  {
    private int MinTicksSinceThickened = 600;

    public override PathEndMode PathEndMode => PathEndMode.Touch;

    //mod start
    public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(GraffitiDefOf.GraffitiMod_Paint);
    //mod end
    public override int MaxRegionsToScanBeforeGlobalSearch => 4;

    public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn) => (IEnumerable<Thing>) pawn.Map.listerFilthInHomeArea.FilthInHomeArea;

    public override bool ShouldSkip(Pawn pawn, bool forced = false) => pawn.Map.listerFilthInHomeArea.FilthInHomeArea.Count == 0;

    //mod start
    public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false) => t is Filth_Graffiti filth && filth.Map.areaManager.Home[filth.Position] && pawn.CanReserve((LocalTargetInfo) t, ignoreOtherReservations: forced);
    //mod end
    
    public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
    {
      //mod start
      Job job = JobMaker.MakeJob(GraffitiDefOf.GraffitiMod_CleanJob);
      //mod end
      
      job.AddQueuedTarget(TargetIndex.A, (LocalTargetInfo) t);
      int num = 15;
      Map map = t.Map;
      Room room = t.GetRoom();
      for (int index1 = 0; index1 < 100; ++index1)
      {
        IntVec3 c = t.Position + GenRadial.RadialPattern[index1];
        if (ShouldClean(c))
        {
          List<Thing> thingList = c.GetThingList(map);
          for (int index2 = 0; index2 < thingList.Count; ++index2)
          {
            Thing thing = thingList[index2];
            if (this.HasJobOnThing(pawn, thing, forced) && thing != t)
              job.AddQueuedTarget(TargetIndex.A, (LocalTargetInfo) thing);
          }
          if (job.GetTargetQueue(TargetIndex.A).Count >= num)
            break;
        }
      }
      if (job.targetQueueA != null && job.targetQueueA.Count >= 5)
        job.targetQueueA.SortBy<LocalTargetInfo, int>((Func<LocalTargetInfo, int>) (targ => targ.Cell.DistanceToSquared(pawn.Position)));
      return job;

      bool ShouldClean(IntVec3 c)
      {
        if (!c.InBounds(map))
          return false;
        if (room == c.GetRoom(map))
          return true;
        Building_Door door = c.GetDoor(map);
        Region region = door != null ? door.GetRegion(RegionType.Portal) : (Region) null;
        if (region != null && !region.links.NullOrEmpty<RegionLink>())
        {
          for (int index1 = 0; index1 < region.links.Count; ++index1)
          {
            RegionLink link = region.links[index1];
            for (int index2 = 0; index2 < 2; ++index2)
            {
              if (link.regions[index2] != null && link.regions[index2] != region && link.regions[index2].valid && link.regions[index2].Room == room)
                return true;
            }
          }
        }
        return false;
      }
    }
  }
}
