using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGrave : ActiveItem, IActivatable
{
    public void TryActivate()
    {
        try
        {
            Debug.Log("Activate");
            IsDisactive = false;
        }
        catch (System.Exception e)
        {
            Debug.LogException(e, this);
            throw;
        }

    }
}
