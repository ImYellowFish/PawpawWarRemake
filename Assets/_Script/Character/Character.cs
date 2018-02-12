using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public Transform ch_transform;

    public void Init()
    {
        ch_transform = transform;

        // Add sub components & init
    }
}
