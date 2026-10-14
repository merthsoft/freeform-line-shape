# Freeform Lines for RimWorld 1.7

Select a tool in a vanilla Default1D, Default2D or Fill2D palette, select freeform line, then hold left mouse and trace a continuous path. Release to designate; use the vanilla cancel action to discard. The brush is one cell wide. No settings, history, or dependency on Designator Shapes state.

Requires Harmony. Full Designator Shapes is incompatible: its active controls patch UpdateDragCellsIfNeeded and replace vanilla style evaluation. This mod has a unique defName and texture directory, but distinct names cannot resolve that behavioral conflict.

Original code by Merthsoft, freeform tool by Jacbo, icon art by Halno. Extracted from Designator Shapes under its Unlicense. The original mask/path cache was replaced to correct x/z bounds, missing initial endpoints, diagonal offsets, stale same-origin strokes, and unbounded repeated samples. Segment interpolation preserves the continuous sampled-line behavior; thin diagonals now use centered cells and include both endpoints.

Build: `./build-1.7.ps1 -RimWorldDir <installed game root> -HarmonyPath <1.7 0Harmony.dll>` with .NET 10. Only the mod DLL belongs in 1.7/Assemblies; game and Harmony DLLs are referenced with Private=false. Tests: `dotnet run --project Tests -c Release`. Tests use minimal host doubles for deterministic geometry and lifecycle checks; they do not substitute for in-game validation.

Gameplay validation: while paused, trace an L-shaped zone with a non-diagonal origin (x differs from z), release, then repeat from the same origin. Cancel a third stroke, switch style and tool, and draw again. Switch maps, load a save, and start a new game. Confirm old paths never return. Test mining and wall building, fast diagonal motion, stationary cursor and map edges. Full Shapes must be disabled. Parent coordinator owns game smoke tests and packaging.
