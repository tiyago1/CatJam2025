using _Game.Enums;

namespace _Game.Scripts
{
    public class FoodRequestView : RequestView
    {
        public void Initialize(FoodType type)
        {
            Image.sprite = Sprites[(int)type];
        }
    }
}