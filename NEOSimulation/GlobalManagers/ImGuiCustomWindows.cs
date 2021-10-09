using System;
using System.Numerics;
using Nez;
using Nez.ImGuiTools;

namespace NEOSimulation.GlobalManagers
{
    using ImGui = ImGuiNET.ImGui;

    public class ImGuiCustomWindows : GlobalManager
    {
        /// <summary>
        /// collect all custom windows and bind their draw calls
        /// </summary>
        public override void OnEnabled()
        {
            var customWindows = ReflectionUtils.GetAllSubclasses(typeof(CustomWindow));
            customWindows.ForEach(customWindowType =>
            {
                var customWindow = Activator.CreateInstance(customWindowType) as CustomWindow;
                if (customWindow != null) 
                    Core.GetGlobalManager<ImGuiManager>().RegisterDrawCommand(customWindow.Draw);
            });
            
            base.OnEnabled();
        }
    }

    public class CustomWindow
    {
        public static bool IsShown { get; set; } = true;

        public static void Show()
        {
            IsShown = true;
        }

        public static void Close()
        {
            IsShown = false;
        }

        public virtual void Draw()
        {
        }

        protected void Separation(float verticalMargin)
        {
            // visual separation
            ImGui.Dummy(new Vector2(0f, verticalMargin));
            ImGui.Separator();
            ImGui.Dummy(new Vector2(0f, verticalMargin));
        }
    }
}