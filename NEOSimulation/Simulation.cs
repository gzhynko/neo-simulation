using NEOSimulation.GlobalManagers;
using Nez;
using Nez.ImGuiTools;

namespace NEOSimulation
{
    public class Simulation : Core
    {
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
            imGuiManager.ShowSeperateGameWindow = false;
            imGuiManager.ShowCoreWindow = false;
            imGuiManager.ShowSceneGraphWindow = false;
            
            RegisterGlobalManager(imGuiManager);
            RegisterGlobalManager(new ImGuiCustomWindows());
        }

    }
}