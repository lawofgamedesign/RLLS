namespace Utilities
{
    using UnityEngine;

    [CreateAssetMenu]
    public class AnimationCurves : ScriptableObject
    {
        [SerializeField]
        public AnimationCurve rapidGrowth;

        [SerializeField]
        public AnimationCurve rapidDecline;
    }
}
