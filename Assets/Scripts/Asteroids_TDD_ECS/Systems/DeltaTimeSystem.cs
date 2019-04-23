namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Burst;

    public class DeltaTimeSystem : JobComponentSystem
    {
        [ BurstCompile ]
        private struct DeltaTimeJob : IJobForEach<DeltaTime>
        {
            public float deltaTime;

            public void Execute( ref DeltaTime deltaTimeComponent )
            {
                deltaTimeComponent.Value = deltaTime;
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            DeltaTimeJob job = new DeltaTimeJob
            {
                deltaTime = Time.deltaTime
            };
            return job.Schedule( this, inputDependencies );
        }
    }
}