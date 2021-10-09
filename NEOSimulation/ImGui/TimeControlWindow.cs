using System;
using System.Globalization;
using ImGuiNET;
using NEOSimulation.GlobalManagers;
using NEOSimulation.Types;
using NEOSimulation.Utils;
using Vector2 = System.Numerics.Vector2;

namespace NEOSimulation.ImGui
{
    using ImGui = ImGuiNET.ImGui;

    public class TimeControlWindow : CustomWindow
    {
        private bool DateInputShown = false;
        private string DateInputText = "";
        
        public override void Draw()
        {
            if (IsShown == false) return;

            ImGui.Begin("Time Control", ImGuiWindowFlags.NoResize);
            ImGui.SetWindowPos(new Vector2(0, 220));
            ImGui.SetWindowSize(new Vector2(250, DateInputShown ? 200 : 170));

            var timeManager = MainScene.Instance.TimeManager;
            if(timeManager.Entity == null)
            {
                ImGui.End();
                return;
            }
            
            ImGui.Text($"Date: {timeManager.CurrentDate}");
            
            ImGui.Dummy(new Vector2(0, 5f));

            if (DateInputShown == false)
            {
                if (ImGui.Button("Change Date"))
                {
                    ImGui.SetWindowSize(new Vector2(250, 200));

                    DateInputShown = true;
                    timeManager.CurrentPlayDirection = TimePlayDirection.None;
                    DateInputText = timeManager.CurrentDate.ToString();
                }
            }
            else
            {
                ImGui.InputText("", ref DateInputText, 30);
                if (ImGui.Button("Done"))
                {
                    if (DateTime.TryParse(DateInputText, out var resultDate))
                    {
                        DateInputShown = false;
                        timeManager.ChangeDate(resultDate);
                    }
                }
            }

            Separation(5f);

            ImGui.Columns(3, "playColumn", false);
            ImGui.SetColumnOffset(1, 90);
            ImGui.SetColumnOffset(2, 140);

            if (ImGui.Button("Play Back"))
            {
                timeManager.CurrentPlayDirection = TimePlayDirection.Backward;
            }
            
            ImGui.NextColumn();
            
            if (ImGui.Button("Stop"))
            {
                timeManager.CurrentPlayDirection = TimePlayDirection.None;
            }
            
            ImGui.NextColumn();
            
            if (ImGui.Button("Play Forward"))
            {
                timeManager.CurrentPlayDirection = TimePlayDirection.Forward;
            }
            
            ImGui.Columns();
            
            ImGui.Dummy(new Vector2(0, 10f));

            ImGui.Columns(2, "stepColumn", false);
            ImGui.SetColumnOffset(1, 140);

            if (ImGui.Button("Step Back"))
            {
                timeManager.StepBack();
            }
            
            ImGui.NextColumn();
            
            if (ImGui.Button("Step Forward"))
            {
                timeManager.StepForward();
            }
            
            ImGui.Columns();

            ImGui.End();
        }
    }
}