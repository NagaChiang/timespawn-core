﻿using Unity.Entities;
using Unity.Jobs;

namespace Timespawn.Core.DOTS.Tween.Systems
{
    [UpdateInGroup(typeof(TweenCompleteSystemGroup))]
    public class TweenMovementCompleteSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            BeginSimulationEntityCommandBufferSystem beginSimulationECBSystem = DotsUtils.GetSystemFromDefaultWorld<BeginSimulationEntityCommandBufferSystem>();
            EndSimulationEntityCommandBufferSystem endSimulationECSSystem = DotsUtils.GetSystemFromDefaultWorld<EndSimulationEntityCommandBufferSystem>();
            EntityCommandBuffer.Concurrent beginSimulationCommandBuffer = beginSimulationECBSystem.CreateCommandBuffer().ToConcurrent();
            EntityCommandBuffer.Concurrent endSimulationCommandBuffer = endSimulationECSSystem.CreateCommandBuffer().ToConcurrent();

            JobHandle removeCompleteTagJob = Entities.WithAll<TweenMovementCompleteTag>().ForEach((Entity entity, int entityInQueryIndex) =>
            {
                endSimulationCommandBuffer.RemoveComponent<TweenMovementCompleteTag>(entityInQueryIndex, entity);
            }).Schedule(inputDeps);

            endSimulationECSSystem.AddJobHandleForProducer(removeCompleteTagJob);

            JobHandle completeStateJob = Entities.ForEach((Entity entity, int entityInQueryIndex, ref TweenMovement tween) =>
            {
                if (TweenSystemUtils.CompleteTweenState(ref tween.State))
                {
                    endSimulationCommandBuffer.RemoveComponent<TweenMovement>(entityInQueryIndex, entity);
                    beginSimulationCommandBuffer.AddComponent(entityInQueryIndex, entity, new TweenMovementCompleteTag());
                }
            }).Schedule(removeCompleteTagJob);

            beginSimulationECBSystem.AddJobHandleForProducer(completeStateJob);
            endSimulationECSSystem.AddJobHandleForProducer(completeStateJob);

            return completeStateJob;
        }
    }
}