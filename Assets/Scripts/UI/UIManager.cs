using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Transform _fears;
    [SerializeField] private TextMeshProUGUI _fearsCount;
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
