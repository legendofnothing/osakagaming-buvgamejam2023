using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Weapons {
    public abstract class WeaponBase : MonoBehaviour {
        public enum Type {
            Gun,
            Throwable,
        }

        public enum Slot {
            Primary,
            Secondary,
            Unarmed,
        }

        [TitleGroup("Config")]
        public Slot slot;
        public Type type; 
        [HideIf("slot", Slot.Unarmed)] public float damage;
        public float speed = 1800;
        public float maxSpeed = 8f;
        [ReadOnly] public float currentSpeed;
        [HideIf("slot", Slot.Unarmed)]  public LayerMask interactLayers;
        [HideIf("slot", Slot.Unarmed)] [ReadOnly] public float currentDamage;
        
        [TitleGroup("Throwable Config")] 
        [HideIf("type", Type.Gun)] [HideIf("slot", Slot.Unarmed)] public Ease throwableEaseType;

        protected virtual void Start() {
            currentSpeed = speed;
            currentDamage = damage;
        }

        public abstract void Attack();
        public virtual bool CanSwitch() => true;

        protected virtual void Update() {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Get mouse postion
            var faceIndex = Mathf.Sign(mousePosition.x - transform.position.x);
            
            transform.localScale = faceIndex <= 0 ? new Vector3(1, 1, 1) : new Vector3(1, -1, 1);;
        }
    }
}