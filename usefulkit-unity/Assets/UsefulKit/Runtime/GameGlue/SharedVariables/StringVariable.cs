using UnityEngine;

namespace GameGlue
{
    [CreateAssetMenu(fileName = "StringVariable", menuName = "GameGlue/StringVariable", order = 0)]
    public class StringVariable : SharedVariable<string>
    { }
}