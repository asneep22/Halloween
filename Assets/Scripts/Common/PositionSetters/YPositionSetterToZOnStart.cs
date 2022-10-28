using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YPositionSetterToZOnStart : MonoBehaviour
{
    private void Start()
    {
        transform.position = new(transform.position.x, transform.position.y, transform.position.y);  
    }

}
