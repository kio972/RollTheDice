using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StackUpdater : MonoBehaviour
{
    TextMeshProUGUI stack;
    private float curStack = 0;
    Coroutine change;

    IEnumerator ChangeText()
    {
        float nextStack = GameManager.instance.player.stack;

        Color color = Color.white;
        if (curStack < nextStack) // 스택증가
            color = Color.yellow;
        else // 스택감소
            color = Color.red;

        stack.color = color;
        float changeStack = nextStack - curStack;
        stack.text = changeStack.ToString();
        curStack = nextStack;

        float elapsed = 0;
        while(elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        elapsed = 0;
        while(elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            stack.color = new Color(stack.color.r, stack.color.g, stack.color.b, 1 - (elapsed * 2f));
            yield return null;
        }

        stack.color = Color.white;
        stack.text = nextStack.ToString();

        

        change = null;
        yield return null;
    }

    public void UpdateStack()
    {
        if (curStack == GameManager.instance.player.stack)
            return;

        if(change != null)
            StopCoroutine(change);
        change = StartCoroutine(ChangeText());
    }

    //public void UpdateStack2()
    //{
    //    stack.text = GameManager.instance.player.stack.ToString();
    //}

    public void Init()
    {
        stack = GetComponent<TextMeshProUGUI>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
