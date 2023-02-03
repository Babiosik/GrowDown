using Cysharp.Threading.Tasks;
using Modules.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Modules.UI.Scripts
{
    public class MainScreenNavigation : MonoBehaviour
    {
        private const int DelaySceneChange = 200;
        private void OnDestroy()
        {
            InputService.Dispose();
            AliveService.Dispose();
            ResourcesService.Dispose();
        }

        public void ToPlay() =>
            UniTask.Delay(DelaySceneChange)
                .GetAwaiter()
                .OnCompleted(() => SceneManager.LoadScene("GameScene"));

        public void ToMainMenu() =>
            UniTask.Delay(DelaySceneChange)
                .GetAwaiter()
                .OnCompleted(() => SceneManager.LoadScene("MainMenu"));
        
        public void ToExit() =>
            UniTask.Delay(DelaySceneChange)
                .GetAwaiter()
                .OnCompleted(Application.Quit);
    }
}