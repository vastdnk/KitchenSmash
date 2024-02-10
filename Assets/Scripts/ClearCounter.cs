using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{


    [SerializeField] private KitchenObjectsSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;

    public void Interact()
    {
        Debug.Log("Interact!");
        Transform tomatoTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
        tomatoTransform.localPosition = Vector3.zero;
    }
}
