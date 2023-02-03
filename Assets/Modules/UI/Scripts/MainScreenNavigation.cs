using Modules.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Modules.UI.Scripts
{
    public class MainScreenNavigation : MonoBehaviour
    {
        private void OnDestroy()
        {
            InputService.Dispose();
            AliveService.Dispose();
            ResourcesService.Dispose();
        }

        public void ToPlay() =>
            SceneManager.LoadScene("GameScene");

        public void ToMainMenu() =>
            SceneManager.LoadScene("MainMenu");
        
        public void ToExit() =>
            Application.Quit();
    }
}