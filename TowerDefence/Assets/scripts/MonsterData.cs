using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster")]
public class MonsterData : ScriptableObject
{
    public string id;           // 唯一标识
    public string displayName;
    public Sprite icon;
    public Sprite fullImage;    // 大图
    [TextArea] public string description;
    public bool unlocked = false;
}
