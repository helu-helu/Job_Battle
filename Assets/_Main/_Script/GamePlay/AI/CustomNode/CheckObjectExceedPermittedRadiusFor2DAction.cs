using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckObjectExceedPermittedRadiusFor2D", story: "Check [Agent] if [Object] exceed the permitted [Radius] radius for 2D.", category: "Action", id: "ac60c98ead17c4c13f0eab1808dfe597")]
public partial class CheckObjectExceedPermittedRadiusFor2DAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Object;
    [SerializeReference] public BlackboardVariable<float> Radius;

    protected override Status OnUpdate()
    {
        if (Agent.Value == null)
        {
            LogFailure("No agent set.");
            return Status.Failure;
        }

        if (Object.Value == null)
        {
            LogFailure("No object set.");
            return Status.Failure;
        }

        float distance = Vector2.Distance(Agent.Value.transform.position, Object.Value.transform.position);
        if (distance > Radius.Value)
        {
            return Status.Success;
        }

        return Status.Failure;
    }
}
