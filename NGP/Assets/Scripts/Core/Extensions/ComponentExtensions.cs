using UnityEngine;

namespace Core.Extensions
{
    public static class ComponentExtensions
    {
        public static bool TryGetComponentInParent<T>(this Component owner, out T component) where T : Behaviour
        {
            component = null;
        
            if (owner == null)
            {
                EditorOnly.LogWarning("Owner is null.");
                return false;
            }

            var parent = owner.transform.parent;
            if (parent == null)
            {
                EditorOnly.LogWarning("Owner's parent is null.");
                return false;
            }

            return parent.TryGetComponent(out component);
        }
    }
}