using UnityEngine;
using UnityEngine.SceneManagement;

namespace Modules.UI.Scripts
{
    public class MainScreenNavigation : MonoBehaviour
    {
        public void ToPlay() =>
            SceneManager.LoadScene("GameScene");

        public void ToMainMenu() =>
            SceneManager.LoadScene("MainMenu");
        
        public void ToExit() =>
            Application.Quit();
    }
}