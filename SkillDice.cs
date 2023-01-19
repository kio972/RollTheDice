using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDice : MonoBehaviour
{
    List<float[]> pos;
    public List<Transform> dices;

    public void SetDice()
    {
        if(dices != null)
        {
            foreach(Transform t in dices)
                Destroy(t.gameObject);
        }

        dices = new List<Transform>();

        if(GameData.quickSlotSkills != null)
        {
            for (int i = 0; i < GameData.quickSlotSkills.Count; i++)
            {
                string path = "Prefab/Battle/SkillDice/Skill" + GameData.quickSlotSkills[i];
                Transform dice = Resources.Load<Transform>(path);
                if (dice != null)
                {
                    dice = Instantiate(dice, transform);

                    float dicePos = pos[GameData.quickSlotSkills.Count - 1][i];
                    float random = Random.Range(-0.3f, 0.3f);
                    Vector3 postion = new Vector3(transform.position.x + dicePos + random, transform.position.y, transform.position.z + random);
                    dice.position = postion;

                    random = Random.Range(-180, 180);
                    Vector3 rotation = new Vector3(0, random, 0);
                    dice.rotation = Quaternion.Euler(rotation);

                    dices.Add(dice);
                }
            }
        }
    }


    public void Init()
    {
        float[] five = new float[5] { -6, -3, 0, 3, 6 };
        float[] four = new float[4] { -4.5f, -1.6f, 1.6f, 4.5f };
        float[] three = new float[3] { -3f, 0, 3f };
        float[] two = new float[2] { -1.6f, 1.6f };
        float[] one = new float[1] { 0 };

        pos = new List<float[]>();
        pos.Add(one);
        pos.Add(two);
        pos.Add(three);
        pos.Add(four);
        pos.Add(five);

        SetDice();
    }
}
