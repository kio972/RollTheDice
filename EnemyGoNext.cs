using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoNext : MonoBehaviour
{
    private void Next()
    {
        SendMessageUpwards("GoNext", SendMessageOptions.DontRequireReceiver);
    }
}
