using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Entity {
    public class EntityMoveTo : MonoBehaviour {
        private NavMeshAgent agent;

        private void Start() {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        public void SetDestination(GameObject target) {
            var agentDrift = 0.0001f;
            var driftPos = target.transform.position + (Vector3)(agentDrift * Random.insideUnitCircle);
            agent.SetDestination(driftPos);
        }
        
        public void SetDestination(Vector3 target) {
            var agentDrift = 0.0001f;
            var driftPos = target + (Vector3)(agentDrift * Random.insideUnitCircle);
            agent.SetDestination(driftPos);
        }

        public void ChangeMovementState(bool state) => agent.isStopped = state;
    }
}
