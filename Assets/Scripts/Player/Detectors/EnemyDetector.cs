using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    internal List<Collider> _enemiesInRange;

    [SerializeField] private string layerToDetect;

    private void Start()
    {
        if (layerToDetect == "")
        {
            layerToDetect = "Enemy";
        }
        _enemiesInRange = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(layerToDetect))
            _enemiesInRange.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(layerToDetect))

            _enemiesInRange.Remove(other);
    }

    public bool playerInRange()
    {
        return _enemiesInRange.Select(col => col.gameObject).Contains(PlayerProperty.player);
    }
}