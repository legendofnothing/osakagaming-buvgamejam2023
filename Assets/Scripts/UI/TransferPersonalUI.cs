using Core.EventDispatcher;
using UnityEngine;
using EventType = Core.EventDispatcher.EventType;

namespace UI {
    public class TransferPersonalUI : MonoBehaviour {
        public void TransferResToDef() {
            this.SendMessage(EventType.OnTransferResearchersToDefenders);
        }

        public void TransferDefToRes() {
            this.SendMessage(EventType.OnTransferDefendersToResearchers);
        }
    }
}
