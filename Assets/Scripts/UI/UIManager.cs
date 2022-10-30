using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject _gameMenu;
    [SerializeField] private Transform _fears;
    [SerializeField] private TextMeshProUGUI _fearsCount;
    [SerializeField] private FadeChanger _fadeChanger;
    [SerializeField] private int sceneWinIndex = 3;
    private bool _isWin;
    private bool _isLose;

    public bool IsLose
    {
        get => _isLose;
        set{
            if (value == true)
            {
                _fadeChanger.StartFadeInAndChangeScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public bool IsWin
    {
        get =>_isWin;
        set
        {
            if (value == true)
            {
                _fadeChanger.StartFadeInAndChangeScene(sceneWinIndex);
            }
        }
    }

    public GameObject GameMenu
    {
        get => _gameMenu;
    }

    public Transform Fears
    {
        get => _fears;
    }
    public TextMeshProUGUI FearsCount
    {
        get => _fearsCount;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }

        Destroy(this);
    }

}
