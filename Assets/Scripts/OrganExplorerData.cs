using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(menuName = "Organ/Organ Data")]

public class OrganExplorerData : ScriptableObject
{
    public string organName;
    [TextArea] public string definition;
    [TextArea] public string function;
    public Sprite imageSlide1;
    public Sprite imageSlide2;
    public VideoClip videoClip;
}
