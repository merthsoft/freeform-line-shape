using HarmonyLib;
using Verse;
namespace Merthsoft.DesignatorShapes.Freeform;
public sealed class FreeformMod : Mod
{
    public FreeformMod(ModContentPack content) : base(content) => new Harmony("Merthsoft.DesignatorShapes.FreeformLine").PatchAll();
}
public sealed class FreeformStyle : DrawStyle
{
    internal static FreeformStyle Active;
    private readonly Stroke stroke = new();
    private Map map;
    private Designator tool;
    private IntVec3 start;
    public override bool CanHaveDuplicates => false;
    internal static void Reset() { Active?.stroke.Clear(); Active = null; }
    public override void Update(IntVec3 origin, IntVec3 target, List<IntVec3> buffer)
    {
        buffer.Clear();
        var currentMap = Find.CurrentMap;
        if (currentMap == null) { Reset(); return; }
        var manager = Find.DesignatorManager;
        if (!manager.Dragger.Dragging)
        {
            Reset();
            if (target.InBounds(currentMap)) buffer.Add(target);
            return;
        }
        if (Active != this || map != currentMap || tool != manager.SelectedDesignator || start != origin)
        {
            Reset(); Active = this; map = currentMap; tool = manager.SelectedDesignator; start = origin;
            stroke.Add(origin.x, origin.z, map.Size.x, map.Size.z);
        }
        stroke.Add(target.x, target.z, map.Size.x, map.Size.z);
        foreach (var p in stroke.Cells) buffer.Add(new IntVec3(p.x, origin.y, p.z));
    }
}
[HarmonyPatch(typeof(DesignationDragger), "StartDrag")]
static class StartStroke { static void Prefix() => FreeformStyle.Reset(); }
[HarmonyPatch(typeof(DesignationDragger), "EndDrag")]
static class EndStroke { static void Postfix() => FreeformStyle.Reset(); }
[HarmonyPatch(typeof(DesignatorManager), "set_SelectedStyle")]
static class SwitchStyle { static void Prefix() => FreeformStyle.Reset(); }
[HarmonyPatch(typeof(Game), "InitNewGame")]
static class NewGame { static void Prefix() => FreeformStyle.Reset(); }
[HarmonyPatch(typeof(Game), "LoadGame")]
static class LoadGame { static void Prefix() => FreeformStyle.Reset(); }
// Recompute our worker per frame, independent of the vanilla tick/endpoint cache.
// Other styles still use the vanilla path; designation validation remains vanilla.
[HarmonyPatch(typeof(DesignationDragger), "UpdateCellBuffer")]
static class UpdateStroke
{
    static bool Prefix(DesignationDragger __instance, IntVec3 ___startDragCell)
    {
        if (Find.DesignatorManager.SelectedStyle?.DrawStyleWorker is not FreeformStyle style) return true;
        style.Update(___startDragCell, UI.MouseCell(), __instance.CellBuffer);
        return false;
    }
}
