using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectUI : MonoBehaviour
{
    public void OnClickStage1()
    {
        GameManager.Instance.SetStage(1);
        SceneManager.LoadScene("Stage1");
    }

    public void OnClickStage2()
    {
        GameManager.Instance.SetStage(2);
        SceneManager.LoadScene("Stage2");
    }
}