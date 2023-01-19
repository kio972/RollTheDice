using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectZone : MonoBehaviour
{
    public int tileIndex;
    protected int effectId;
    public int EffectId { get { return effectId; } }

    public virtual void Init() { }
}
