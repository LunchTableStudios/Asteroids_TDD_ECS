namespace Asteroids_TDD_ECS
{
    using Unity.Entities;
    using Unity.Collections;
    using Unity.Mathematics;
    using Unity.Jobs;
    using Unity.Transforms;
    using Unity.Burst;

    [ BurstCompile ]
    public class RotationSpeedSystem : JobComponentSystem
    {
        private struct RotationSpeedJob : IJobForEach<Rotation, RotationSpeed, DeltaTime>
        {
            public void Execute( ref Rotation rotation, [ ReadOnly ] ref RotationSpeed speed, [ ReadOnly ] ref DeltaTime deltaTime )
            {
                rotation.Value = math.mul( math.normalize( rotation.Value ), quaternion.AxisAngle( new float3( 0, 0, 1 ), speed.Value * deltaTime.Value ) );
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            RotationSpeedJob job = new RotationSpeedJob();
            return job.Schedule( this, inputDependencies );
        }
    }
}