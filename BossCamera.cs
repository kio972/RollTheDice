using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    [SerializeField]
    private EnemyController boss;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(boss != null)
        {
            transform.position = boss.charImg.transform.position + new Vector3(0, 0, -10);
        }
    }
}
