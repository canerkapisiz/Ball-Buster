using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kutu : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    public void EfektOynat()
    {
        gameManager.KutuParcalamaEfekt(transform.position);
        gameObject.SetActive(false);
        Debug.Log("geldi");
    }
}
