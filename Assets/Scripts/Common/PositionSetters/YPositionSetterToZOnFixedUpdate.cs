using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YPositionSetterToZOnFixedUpdate : MonoBehaviour
{

    private void FixedUpdate()
    {
        transform.position = new(transform.position.x, transform.position.y, transform.position.y);
    }

}
