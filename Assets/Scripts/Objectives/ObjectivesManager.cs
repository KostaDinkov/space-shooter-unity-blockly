using Assets.Scripts.GameEvents;
using Game.GameEvents;
using Game.Systems;
using UnityEngine;


namespace Game.Objectives
{
    /// <summary>
    /// Keeps track of the completeness of a challenge based on the completed objectives;
    /// </summary>
    public class ObjectivesManager
    {
        private readonly GameEventManager eventManager;
        private readonly Objectives objectives;
        private readonly GameEvent objectiveCompletedEvent;
        private readonly GameEvent challangeCompletedEvent;

        public ObjectivesManager()
        {
            objectives = GameData.Instance.CurrentChallenge.GetComponent<Objectives>();
            foreach (var objective in objectives.ObjectivesList)
            {
                objective.Init();
            }

            this.objectiveCompletedEvent =
                (GameEvent) Resources.Load("Scriptable Objects/GameEvents/ObjectiveCompleted");
            this.challangeCompletedEvent =
                (GameEvent) Resources.Load("Scriptable Objects/GameEvents/ChallangeCompleted");
            this.eventManager = GameEventManager.Instance;
            this.eventManager.Subscribe(objectiveCompletedEvent, IsChallangeCompleted);
        }

        void IsChallangeCompleted()
        {
            foreach (var objective in objectives.ObjectivesList)
            {
                if (!objective.IsComplete())
                {
                    return;
                }
            }

            eventManager.Publish(challangeCompletedEvent);
        }
    }
}