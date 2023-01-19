using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    private void InteractCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Interact"))
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    print("플레이어");
                }
                else if(hit.transform.gameObject.tag == "Enemy")
                {
                    EnemyController enemy = GetComponentInParent<EnemyController>();
                    if (enemy != null)
                    {

                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        InteractCheck();
    }
}
