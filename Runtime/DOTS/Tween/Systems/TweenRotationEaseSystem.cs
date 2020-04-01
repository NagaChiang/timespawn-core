﻿using Unity.Entities;
using Unity.Jobs;

namespace Timespawn.Core.DOTS.Tween.Systems
{
    [UpdateInGroup(typeof(TweenEaseSystemGroups))]
    public class TweenRotationEaseSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;
            JobHandle jobHandle = Entities.ForEach((ref TweenRotationData tween) =>
            {
                TweenSystemUtils.UpdateTweenState(ref tween.State, deltaTime);
            }).Schedule(inputDeps);

            return jobHandle;
        }
    }
}