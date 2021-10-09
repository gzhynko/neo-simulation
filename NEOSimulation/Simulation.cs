using NEOSimulation.GlobalManagers;
using Nez;
using Nez.ImGuiTools;

namespace NEOSimulation
{
    public class Simulation : Core
    {
        //private CelestialBody _celestialBody;
        protected override void Initialize()
        {
            base.Initialize();
            
            Window.AllowUserResizing = true;
            IsFixedTimeStep = true;
            ExitOnEscapeKeypress = false;
            
            Scene = new MainScene();

            Batcher.UseFnaHalfPixelMatrix = true;

            var imGuiManager = new ImGuiManager();
            imGuiManager.ShowMenuBar = false;
            
            RegisterGlobalManager(imGuiManager);
            RegisterGlobalManager(new ImGuiCustomWindows());
        }

    }
}