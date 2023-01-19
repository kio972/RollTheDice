using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillType
{
    Common,

}

public class SkillLearn : MonoBehaviour
{
    public SkillDesc skillDesc;
    public Button closeBtn;
    public CommonSkillTree skillTree;

    
    private void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    public void SetSkillLearnDesc(SkillType skill)
    {
        switch(skill)
        {
            case SkillType.Common:
                break;
        }
    }

    public void Init()
    {
        skillDesc = GetComponentInChildren<SkillDesc>(true);
        if(skillDesc != null)
        {
            skillDesc.Init();
            skillDesc.gameObject.SetActive(false);
        }

        closeBtn = UtillHelper.GetComponent<Button>("Close", transform, false);
        if (closeBtn != null)
            closeBtn.onClick.AddListener(CloseWindow);

        skillTree = GetComponentInChildren<CommonSkillTree>(true);
        if(skillTree != null)
            skillTree.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
