using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatableData : ScriptableObject
{
    public event System.Action OnValuseUpdated;
    public bool autoUpdate;

    protected virtual void OnValidate()
    {
        NotifyOfUpdateValues();
    }
    public void NotifyOfUpdateValues()
    {
        if(OnValuseUpdated != null)
        {
            OnValuseUpdated();
        }      
    }
}
