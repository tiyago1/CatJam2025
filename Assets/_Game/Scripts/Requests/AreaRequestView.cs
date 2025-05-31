using _Game.Enums;

namespace _Game.Scripts
{
    public class AreaRequestView : RequestView
    {
        public void Initialize(AreaType type)
        {
            Image.sprite = Sprites[(int)type];
        }
    }
}