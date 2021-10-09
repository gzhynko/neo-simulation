using ImGuiNET;
using NEOSimulation.Entities;
using NEOSimulation.GlobalManagers;
using NEOSimulation.Utils;
using Vector2 = System.Numerics.Vector2;

namespace NEOSimulation.ImGui
{
    using ImGui = ImGuiNET.ImGui;

    public class ObjectInfoWindow : CustomWindow
    {
        public override void Draw()
        {
            if (IsShown == false) return;
            
            ImGui.Begin("Object Information", ImGuiWindowFlags.NoResize);
            ImGui.SetWindowPos(Vector2.Zero);
            ImGui.SetWindowSize(new Vector2(300, 200));

            var selectedBodyManager = MainScene.Instance.SelectedBodyManager;
            if(selectedBodyManager.Current == null)
            {
                ImGui.End();
                return;
            }

            var selectedCelestialBody = selectedBodyManager.Current.Entity as CelestialBody;
            if(selectedCelestialBody == null)
            {
                ImGui.End();
                return;
            }
            
            var sun = MainScene.Instance.GetBodyByName("Sun");
            var earth = MainScene.Instance.GetBodyByName("Earth");

            bool enableEarthOnlyView = MainScene.Instance.EarthOnlyView;
            
            ImGui.Text($"Name: {selectedCelestialBody.Name}");
            ImGui.Text($"Sun Distance: { Computation.DistanceInAuBetweenBodies(sun, selectedBodyManager.Current) :F} au");
            ImGui.Text($"Earth Distance: { Computation.DistanceInAuBetweenBodies(earth, selectedBodyManager.Current) :F} au");

            Separation(5f);
            
            if (ImGui.CollapsingHeader("Object Parameters"))
            {
                ImGui.Text($"SemiMajorAxis: {selectedCelestialBody.SemiMajorAxis :F} au");
                ImGui.Text($"Eccentricity: {selectedCelestialBody.Eccentricity :F}");
                ImGui.Text($"Inclination: {selectedCelestialBody.Inclination :F} deg");
                ImGui.Text($"AscendingNode: {selectedCelestialBody.AscendingNode :F} deg");
                ImGui.Text($"ArgumentOfPerihelion: {selectedCelestialBody.ArgumentOfPerihelion :F} deg");
                ImGui.Text($"Epoch: {selectedCelestialBody.Epoch}");
                ImGui.Text($"Diameter: {selectedCelestialBody.DiameterKm :F} km");
            }
            
            Separation(5f);

            ImGui.Checkbox("Earth-only View", ref enableEarthOnlyView);
            
            ImGui.End();
            
            if(enableEarthOnlyView) MainScene.Instance.EnableEarthOnlyView(selectedBodyManager.Current);
            else MainScene.Instance.DisableEarthOnlyView();
        }
    }
}