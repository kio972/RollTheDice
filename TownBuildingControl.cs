using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownBuildingControl : MonoBehaviour
{
    private TownBuildingFX[] buildings;
    private float elapsed = 0;
    private bool rising = true;
    public float speed = 1;

    private void SinkColor()
    {
        if (rising)
        {
            elapsed += Time.deltaTime * speed;
            if (elapsed > 1)
            {
                elapsed = 1;
                rising = false;
            }
        }
        else
        {
            elapsed -= Time.deltaTime * speed;
            if (elapsed < 0)
            {
                elapsed = 0;
                rising = true;
            }
        }

        for (int i = 0; i < buildings.Length; i++)
        {
            if (buildings[i] != null)
            {
                Color color = new Color(1, 1, 1, elapsed);
                buildings[i].SetColor(color);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        buildings = GetComponentsInChildren<TownBuildingFX>();
    }
    
    // Update is called once per frame
    void Update()
    {
        SinkColor();
    }
}
