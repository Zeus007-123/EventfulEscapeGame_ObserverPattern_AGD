using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIView : MonoBehaviour
{
    [SerializeField] private Button playButton;

    private void OnEnable() => playButton.onClick.AddListener(onPlayButtonClicked);
    private void onPlayButtonClicked() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
}
