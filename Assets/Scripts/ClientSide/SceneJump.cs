using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneJump : MonoBehaviour
{
    public static void JumpTo(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
    }
}

enum ScenesIndex
{
    Authorization = 0,
    UserScene = 1,
    AdminScene = 2,
    RhytmGameScene = 3,
}