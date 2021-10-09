using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using NEOSimulation.Entities;
using NEOSimulation.GlobalManagers;
using NEOSimulation.Utils;
using Vector2 = System.Numerics.Vector2;

namespace NEOSimulation.ImGui
{
    using ImGui = ImGuiNET.ImGui;

    public class ObjectFinderWindow : CustomWindow
    {
        private string searchString = "";
        private string currentDisplayedSearchResult = "";
        private int currentItem;
        private string[] displayedItems;
        
        public override void Draw()
        {
            if (IsShown == false) return;
            
            ImGui.Begin("Object Finder", ImGuiWindowFlags.NoResize);
            ImGui.SetWindowPos(new Vector2(0, 450));
            ImGui.SetWindowSize(new Vector2(300, 230));
            
            var selectedBodyManager = MainScene.Instance.SelectedBodyManager;
            if(selectedBodyManager.Current == null)
            {
                ImGui.End();
                return;
            }
            
            ImGui.SetNextItemWidth(ImGui.GetWindowWidth() - 15f);
            ImGui.InputText("", ref searchString, 100);
            
            if(searchString != currentDisplayedSearchResult)
            {
                var celestialBodies = MainScene.Instance.BodyArray;
                var sortedNames = new List<string>();

                for (var i = 0; i < celestialBodies.Length; i++)
                {
                    if (celestialBodies[i].Name.ToLower().Contains(searchString.ToLower()))
                    {
                        sortedNames.Add(celestialBodies[i].Name);
                    }
                }

                displayedItems = sortedNames.ToArray();
                currentDisplayedSearchResult = searchString;
            }

            if (searchString == String.Empty)
            {
                var celestialBodies = MainScene.Instance.BodyArray;
                var names = new string[celestialBodies.Length];

                for (var i = 0; i < celestialBodies.Length; i++)
                {
                    names[i] = celestialBodies[i].Name;
                }

                displayedItems = names;
            }

            Separation(5f);
            
            ImGui.SetNextItemWidth(ImGui.GetWindowWidth() - 15f);

            if (ImGui.ListBox("", ref currentItem, displayedItems, displayedItems.Length))
            {
                selectedBodyManager.SetSelected(MainScene.Instance.GetBodyByName(displayedItems[currentItem]));
            }
        }
    }
}