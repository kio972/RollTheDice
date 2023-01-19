using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownBuildingFX : MonoBehaviour
{
    SpriteRenderer sprite;
    public void SetColor(Color color)
    {
        if(TownController.instance.buildingfx == this)
        {
            sprite.color = Color.white;
            return;
        }

        if(sprite != null)
            sprite.color = color;
    }

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
}
