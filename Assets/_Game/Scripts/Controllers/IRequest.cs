using _Game.Enums;
using UnityEngine;
using Zenject;

namespace _Game.Scripts
{
    public abstract class BaseRequest : MonoBehaviour
    {
        [Inject] protected PlayerController Player;
        public RequestType Type;
        protected Cat Cat;

        public virtual void Initialize(Cat cat)
        {
            Cat = cat;

        }
        public abstract void Solve();

        public bool IsValid() => Player.ActiveRequest == Type;
    }
}