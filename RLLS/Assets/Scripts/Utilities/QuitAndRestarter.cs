namespace Utilities
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class QuitAndRestarter : MonoBehaviour
    {
        public static QuitAndRestarter instance;


        /// <summary>
        /// Make this a singleton.
        /// </summary>
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }


        /// <summary>
        /// Each frame, listen for key presses.
        /// </summary>
        private void Update()
        {
            QuitCheck();
            RestartCheck();
        }


        /// <summary>
        /// Quit the game if the player hits ESC.
        /// </summary>
        private void QuitCheck()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        }


        /// <summary>
        /// Reload the current scene if the player hits Enter.
        /// 
        /// Note that this restarts the current scene, not the entire game. (I.e., it does not reload to the title scene.)
        /// </summary>
        private void RestartCheck()
        {
            if (Input.GetKeyDown(KeyCode.Return)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
