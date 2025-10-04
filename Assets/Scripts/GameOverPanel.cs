using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public Button _restarBtn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _restarBtn.onClick.AddListener(RestartBtnClick);
    }

    void RestartBtnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
