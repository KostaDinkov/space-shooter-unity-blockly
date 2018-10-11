using Assets.Scripts.GameEvents;
using Game.GameEvents;
using Game.Systems;

namespace Game.Objectives
{
    /// <summary>
    ///     Keeps track of the completeness of a challenge based on the completed objectives;
    /// </summary>
    public class ObjectivesManager
    {
        private readonly GameEvent challangeCompletedEvent;
        private readonly GameEventManager eventManager;
        private readonly GameEvent objectiveCompletedEvent;
        private readonly Objectives objectives;

        public ObjectivesManager()
        {
            this.objectives = GameData.Instance.CurrentChallenge.GetComponent<Objectives>();
            foreach (var objective in this.objectives.ObjectivesList)
            {
                objective.Init();
            }


            this.eventManager = GameEventManager.Instance;
            this.eventManager.Subscribe(
                new GameEvent {EventType = GameEventType.ObjectiveCompleted}, this.IsChallangeCompleted);
        }

        private void IsChallangeCompleted()
        {
            foreach (var objective in this.objectives.ObjectivesList)
            {
                if (!objective.IsComplete())
                {
                    return;
                }
            }

            this.eventManager.Publish(new GameEvent {EventType = GameEventType.ChallangeCompleted});
        }
    }
}