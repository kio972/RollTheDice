using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Button btn;
    public Transform tipHealth;
    public Transform tipRelic;
    
    // Start is called before the first frame update
    void Awake()
    {
        btn = GetComponentInChildren<Button>();
        
        tipHealth = transform.Find("Tip1");
        if(tipHealth != null)
            tipHealth.gameObject.SetActive(false);
        
        tipRelic = transform.Find("Tip2");
        if(tipHealth == null)
            tipRelic.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
