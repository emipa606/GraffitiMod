using RimWorld;
using Verse;


namespace GraffitiMod
{
    /*
     * Inheritance from Building instead of Filth is necessary so the graffiti filth is not despawned when loading a game (wall is placed on top of filth and replaces it).
     * To Keep the cleaning behaviour JobDriver_CleanGraffiti is implemented to target Filth_Graffiti
     */
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
            if (!Parent.DestroyedOrNull())
                return;
            
            Destroy(DestroyMode.Vanish);
        }
        
        //idea for next version: Event based instead of using Tick()
        //if (this.def.CanAffectLinker)
        //{
        //    map.linkGrid.Notify_LinkerCreatedOrDestroyed(this);
        //    map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlag.Things, true, false);
        //}
        

    }
}