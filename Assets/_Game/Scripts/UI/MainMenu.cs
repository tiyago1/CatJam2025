using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        public void OnStartButtonClicked()
        {
            SceneManager.LoadScene(1);
        }
    }
}