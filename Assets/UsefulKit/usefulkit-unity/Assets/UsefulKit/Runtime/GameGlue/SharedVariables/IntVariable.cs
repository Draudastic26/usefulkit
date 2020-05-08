using UnityEngine;

namespace GameGlue
{
    [CreateAssetMenu(fileName = "IntVariable", menuName = "GameGlue/IntVariable", order = 0)]
    public class IntVariable : SharedVariable<int>
    { }
}