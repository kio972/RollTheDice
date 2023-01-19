using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxButton : MonoBehaviour
{
    private SpriteRenderer image;
    private bool rising = false;
    private float elapsed = 0;
    private float speed = 0.5f;

    private bool MouseOverCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Interact"))
            {
                FxButton fxButton = hit.transform.GetComponent<FxButton>();
                if (fxButton == this)
                    return true;
            }
        }

        return false;
    }

    private void SetColor()
    {
        if (image != null)
        {
            if(MouseOverCheck())
            {
                image.color = Color.white;
                OnEnable();
                return;
            }

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

            Color color = new Color(1, 1, 1, elapsed);
            image.color = color;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        rising = true;
        elapsed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        SetColor();
    }
}
