using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FamousSentence", menuName = "ScriptableObjects/FamousWordsSO", order = 2)]
public class FamousWordsSO : ScriptableObject
{
    public List<FamousSentence> FamousSentences;

}
[System.Serializable]
public struct FamousSentence
{
    [TextArea(10,20)] public string Sentence;
}