using NEOSimulation.Components.Orbital;
using Nez;

namespace NEOSimulation.Components
{
    public class SelectedBody : Component
    {
        public Body Current;

        public void SetSelected(Body newBody)
        {
            Current.OnDeselected();
            Current = newBody;
            Current.OnSelected();
        }
    }
}