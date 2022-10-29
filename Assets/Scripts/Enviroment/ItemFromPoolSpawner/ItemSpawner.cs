using QFSW.MOP2;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(YPositionSetterToZOnStart))]
public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private string _poolName;
    [SerializeField] private Vector3 _scale;
    public GameObject SpawedGameObject
    {
        get;
        private set;
    }

    [Header("Респавн")]
    [SerializeField] private float _checkTime;


    public void Start()
    {
        Spawn();
        StartCoroutine(RespawnObject());
    }

    private void Spawn()
    {
        Transform poolObjecTransform = MasterObjectPooler.Instance.GetObject(_poolName).transform;
        poolObjecTransform.SetParent(transform.parent);
        poolObjecTransform.localPosition = transform.localPosition;
        poolObjecTransform.localScale = _scale;
        SpawedGameObject = poolObjecTransform.gameObject;

        if (poolObjecTransform.TryGetComponent(out ActiveItem activeItem))
        {
            activeItem.IsDisactive = true;
        }
    }

    private IEnumerator RespawnObject()
    {
        while (true)
        {
            yield return new WaitForSeconds(_checkTime);

            if (!SpawedGameObject.activeInHierarchy)
            {
                Spawn();
            }
        }
    }


}
