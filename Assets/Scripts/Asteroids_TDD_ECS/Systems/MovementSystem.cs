namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Physics;
    using Unity.Transforms;
    using Unity.Mathematics;

    public class MovementSystem : JobComponentSystem
    {
        private struct MovementJob : IJobForEach<Movement, MovementSpeed, Rotation, PhysicsVelocity>
        {
            public float DeltaTime;

            public void Execute( [ ReadOnly ] ref Movement movement, [ ReadOnly ] ref MovementSpeed speed, [ ReadOnly ] ref Rotation rotation, ref PhysicsVelocity velocity )
            {
                velocity.Linear += math.mul( math.normalize( rotation.Value ), movement.Value * speed.Value * DeltaTime );
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            return ProcessMovementJob( Time.deltaTime, inputDependencies );
        }

        public JobHandle ProcessMovementJob( float deltaTime, JobHandle inputDependencies = default( JobHandle ) )
        {
            MovementJob job = new MovementJob
            {
                DeltaTime = deltaTime
            };

            return job.Schedule( this, inputDependencies );
        }
    }
}