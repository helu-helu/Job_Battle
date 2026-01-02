using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Check collisions in radius for 2D", story: "Check [Agent] collisions in [Radius] radius for 2D", category: "Action", id: "b70a1348b25a561250953f808dc2974c")]
    internal partial class CheckCollisionsInRadiusFor2DAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Agent;
        [SerializeReference] public BlackboardVariable<float> Radius;
        [SerializeReference] public BlackboardVariable<string> Tag;
        [Tooltip("[Out Value] This field is assigned with the collided object, if a collision was found.")]
        [SerializeReference] public BlackboardVariable<GameObject> CollidedObject;

        protected override Status OnStart()
        {
            if (Agent.Value == null)
            {
                LogFailure("No agent set.");
                return Status.Failure;
            }

            // if (CollidedObject is IBlackboardVariableCast)
            // {
            //     var caster = CollidedObject as IBlackboardVariableCast;
            //     LogFailure($"Invalid CollidedObject variable: Expecting \"GameObject\" but is \"{caster.SourceTypeName}\". Please provide a valid GameObject variable.");
            //     return Status.Failure;
            // }

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(Agent.Value.transform.position, Radius.Value);

            for (int i = 0; i < hitColliders.Length; i++)
            {
                Collider2D hitCollider = hitColliders[i];
                if (hitCollider.gameObject == Agent.Value
                    || (Tag != null && Tag.Value != string.Empty && !hitCollider.CompareTag(Tag.Value)))
                {
                    continue;
                }

                CollidedObject.Value = hitCollider.gameObject;

                return Status.Success;
            }

            return Status.Failure;
        }
    }
