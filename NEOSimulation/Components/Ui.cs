using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;

namespace NEOSimulation.Components
{
    public class Ui : UICanvas
    {
        private Label inputBlockedLabel;

        public override void OnAddedToEntity()
        {
            var table = Stage.AddElement( new Table() );

            table.SetFillParent( true );
            table.Center().Top();

            inputBlockedLabel = new Label("[Input Blocked]", new LabelStyle(Graphics.Instance.BitmapFont, Color.Red, 2f));
            table.Add(inputBlockedLabel);
            
            base.OnAddedToEntity();
        }

        public override void Update()
        {
            base.Update();

            inputBlockedLabel.SetVisible(MainScene.Instance.InputBlocked);
        }
    }
}