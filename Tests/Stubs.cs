namespace HarmonyLib { public class Harmony { public Harmony(string id){} public void PatchAll(){} } [AttributeUsage(AttributeTargets.Class)] public class HarmonyPatch : Attribute { public HarmonyPatch(Type type,string method){} } }
namespace Verse {
 public class ModContentPack {} public class Mod { public Mod(ModContentPack c){} }
 public class Game { public void InitNewGame(){} public void LoadGame(){} }
 public class Designator {}
 public class Map { public IntVec3 Size=new(40,1,20); }
 public record struct IntVec3(int x,int y,int z) { public bool InBounds(Map m)=>x>=0&&z>=0&&x<m.Size.x&&z<m.Size.z; }
 public class DrawStyle { public virtual bool CanHaveDuplicates=>true; public virtual void Update(IntVec3 s,IntVec3 t,List<IntVec3> b){} }
 public class DrawStyleDef { public DrawStyle DrawStyleWorker; }
 public class DesignationDragger { public bool Dragging; public List<IntVec3> CellBuffer=new(); }
 public class DesignatorManager { public DesignationDragger Dragger=new(); public Designator SelectedDesignator=new(); public DrawStyleDef SelectedStyle; }
 public static class Find { public static Map CurrentMap=new(); public static DesignatorManager DesignatorManager=new(); }
 public static class UI { public static IntVec3 MouseCell()=>new(); }
}
