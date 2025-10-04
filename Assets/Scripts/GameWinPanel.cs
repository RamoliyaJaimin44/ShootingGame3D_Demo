using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameWinPanel : MonoBehaviour
{
    public Button _restartBtn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _restartBtn.onClick.AddListener(RestartBtnClick);
    }

    void RestartBtnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
