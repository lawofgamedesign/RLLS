namespace Title
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class SceneLoader : MonoBehaviour
    {
        private const string GAME_SCENE = "BlockRoom";
        private const string TRAIN_SCENE = "Training";


        public void StartGame()
        {
            SceneManager.LoadScene(GAME_SCENE);
        }


        public void StartTraining()
        {
            SceneManager.LoadScene(TRAIN_SCENE);
        }
    }
}
