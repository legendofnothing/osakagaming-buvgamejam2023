using UnityEngine;

namespace Core
{
    public static class CheckLayerMask
    {
        /// <summary>
        /// Check layer tags without passing string. Fuckery of how Unity detects layers
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public static bool IsInLayerMask(GameObject obj, LayerMask layerMask) {
            return (layerMask.value & (1 << obj.layer)) > 0;
        }
    }
}
