using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    [CreateAssetMenu(fileName = "SpriteData", menuName = "Sprite/SpriteData", order = 1)]
    public class SpriteData : ScriptableObject {
        public List<Sprite> sprites;
    }
}
