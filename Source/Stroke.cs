namespace Merthsoft.DesignatorShapes.Freeform;
// Continuous sampled segments from Jacbo's FreeformLine, with bounded stroke storage.
public sealed class Stroke
{
    private readonly HashSet<(int x, int z)> cells = new();
    private (int x, int z) last;
    private bool started;
    public int Count => cells.Count;
    public IEnumerable<(int x, int z)> Cells => cells;
    public void Clear() { cells.Clear(); started = false; }
    public void Add(int x, int z, int width, int height)
    {
        x = Math.Clamp(x, 0, width - 1); z = Math.Clamp(z, 0, height - 1);
        if (!started) { last = (x,z); started = true; }
        int dx = Math.Abs(x-last.x), dz = Math.Abs(z-last.z), steps = Math.Max(dx,dz);
        for (int i=0; i<=steps; i++)
        {
            double fraction = steps == 0 ? 0 : (double)i/steps;
            cells.Add(((int)Math.Round(last.x+(x-last.x)*fraction, MidpointRounding.AwayFromZero),
                       (int)Math.Round(last.z+(z-last.z)*fraction, MidpointRounding.AwayFromZero)));
        }
        last = (x,z);
    }
}
