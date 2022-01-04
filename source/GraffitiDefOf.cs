using RimWorld;
using Verse;

namespace GraffitiMod
{
    [DefOf]
    public static class GraffitiDefOf
    {
        public static ThingDef GraffitiMod_Paint;
        
        /* painting */
        public static JobDef GraffitiMod_PaintGraffitiJob;
        public static JoyGiverDef GraffitiMod_PaintGraffitiJoy;
        
        /* cleaning */
        public static JobDef GraffitiMod_CleanJob;
        public static WorkGiverDef GraffitiMod_CleanWork;

        /* mental break */
        public static ThinkTreeDef GraffitiMod_PaintingSpreeThinkTree;
        public static MentalBreakDef GraffitiMod_GraffitiPaintingSpreeBreak;
        public static MentalStateDef GraffitiMod_GraffitiPaintingSpreeState;
        
    }
}

