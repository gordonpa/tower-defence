using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster")]
public class MonsterData : ScriptableObject
{
    public string id;           // Ψһ��ʶ
    public string displayName;
    public Sprite icon;
    public Sprite fullImage;    // ��ͼ
    [TextArea] public string description;
    public bool unlocked = false;
}
