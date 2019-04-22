namespace Asteroids_TDD_ECS
{
    using Unity.Entities;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Burst;
    using Unity.Transforms;
    using Unity.Physics;
    using Unity.Mathematics;

    public class MovementSpeedSystem : JobComponentSystem
    {
        private struct MovementSpeedJob : IJobForEach<MovementSpeed, Rotation, DeltaTime, PhysicsVelocity>
        {
            public void Execute( [ ReadOnly ] ref MovementSpeed speed, [ ReadOnly ] ref Rotation rotation, [ ReadOnly ] ref DeltaTime deltaTime, ref PhysicsVelocity velocity )
            {
                velocity.Linear += math.mul( math.normalize( rotation.Value ), new float3( 0, speed.Value * deltaTime.Value, 0 ) );
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            MovementSpeedJob job = new MovementSpeedJob();
            return job.Schedule( this, inputDependencies );
        }
    }
}