using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectsSO kitchenObjectSO;

    private ClearCounter clearCounter;

    public KitchenObjectsSO GetKitchenObjectSO() { 
        return kitchenObjectSO; 
    }



    public void SetClearCounter(ClearCounter clearCounter)
    {
        this.clearCounter = clearCounter;
    }

    public ClearCounter GetClearCounter()
    {
        return clearCounter;
    }

}
