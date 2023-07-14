using System;
using Core;
using UnityEngine;

namespace Base {
    public class Base : Singleton<Base> {
        public float hp;
        public float currentHp;

        public void Start() {
            currentHp = hp;
        }
        
        
    }
}
