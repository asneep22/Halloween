using QFSW.MOP2;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ActiveItem : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _boxCollider2D;
    [SerializeField] private PlayerItemUser _itemUser;

    [Header("Звуки")]
    [SerializeField] private string _audioSourcePoolName;
    [SerializeField] private List<AudioClip> _pickUpClips;
    [SerializeField] private List<AudioClip> _startActivateClips;
    [SerializeField] private List<AudioClip> _activateClips;

    public List<AudioClip> StartActivateClips
    {
        get => _startActivateClips;
    }

    public List<AudioClip> ActivateClips
    {
        get => _activateClips;
    }
    public string AudioSourcePoolName
    {
        get => _audioSourcePoolName;
    }
  

    public bool IsPlayerInThePickUpZone
    {
        get;
        set;
    }
    public bool IsDisactive
    {
        get;
        set;
    } = true;

    private void Start()
    {
        _boxCollider2D.isTrigger = true;
    }

    private void Update()
    {
        if (IsPlayerInThePickUpZone && IsDisactive)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                AudioPlayer.PlayRandom(transform, MasterObjectPooler.Instance.GetObjectComponent<AudioSource>(AudioSourcePoolName), _pickUpClips);
                _itemUser.PickUp(transform);
                _itemUser.ItemCollider = _boxCollider2D;
                _boxCollider2D.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerItemUser itemUser))
        {
            _itemUser = itemUser;
            IsPlayerInThePickUpZone = true;
            itemUser.StartShowHint();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerItemUser itemUser))
        {
            _itemUser = null;
            IsPlayerInThePickUpZone = false;
            itemUser.StopShowHint();
        }
    }
}
