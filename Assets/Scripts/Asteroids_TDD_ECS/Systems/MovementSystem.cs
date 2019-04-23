namespace Asteroids_TDD_ECS
{
    using Unity.Entities;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Burst;
    using Unity.Physics;
    using Unity.Transforms;
    using Unity.Mathematics;

    public class MovementSystem : JobComponentSystem
    {
        private EntityQuery movementEntityQuery;

        private struct MovementJob : IJobChunk
        {
            [ ReadOnly ] public ArchetypeChunkComponentType<MovementInput> movementInputType;
            [ ReadOnly ] public ArchetypeChunkComponentType<MovementSpeed> movementSpeedType;
            [ ReadOnly ] public ArchetypeChunkComponentType<DeltaTime> deltaTimeType;
            [ ReadOnly ] public ArchetypeChunkComponentType<Rotation> rotationType;
            public ArchetypeChunkComponentType<PhysicsVelocity> physicsVelocityType;

            public void Execute( ArchetypeChunk chunk, int index, int firstEntityIndex )
            {
                NativeArray<PhysicsVelocity> velocities = chunk.GetNativeArray( physicsVelocityType );

                for( int i = 0; i < chunk.Count; i++ )
                {
                    PhysicsVelocity velocity = velocities[i];

                    velocities[i] = new PhysicsVelocity{
                        Linear = velocity.Linear + CalculateImpulse( chunk, index )
                    };
                }
            }

            public float3 CalculateImpulse( ArchetypeChunk chunk, int index )
            {
                float input = chunk.GetNativeArray( movementInputType )[ index ].Value;
                float speed = chunk.GetNativeArray( movementSpeedType )[ index ].Value;
                quaternion direction = chunk.GetNativeArray( rotationType )[ index ].Value;
                float deltaTime = chunk.GetNativeArray( deltaTimeType )[ index ].Value;

                float3 impulse = math.mul( math.normalize( direction ), new float3( 0, input * speed * deltaTime, 0 ) );
                
                return impulse;
            }
        }

        protected override void OnCreate()
        {
            movementEntityQuery = GetEntityQuery(
                ComponentType.ReadOnly<MovementInput>(),
                ComponentType.ReadOnly<MovementSpeed>(),
                ComponentType.ReadOnly<DeltaTime>(),
                ComponentType.ReadOnly<Rotation>(),
                typeof( PhysicsVelocity )
            );
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            ArchetypeChunkComponentType<MovementInput> movementInputType = GetArchetypeChunkComponentType<MovementInput>( true );
            ArchetypeChunkComponentType<MovementSpeed> movementSpeedType = GetArchetypeChunkComponentType<MovementSpeed>( true );
            ArchetypeChunkComponentType<DeltaTime> deltaTimeType = GetArchetypeChunkComponentType<DeltaTime>( true );
            ArchetypeChunkComponentType<Rotation> rotationType = GetArchetypeChunkComponentType<Rotation>( true );
            ArchetypeChunkComponentType<PhysicsVelocity> physicsVelocityType = GetArchetypeChunkComponentType<PhysicsVelocity>( false );

            MovementJob job = new MovementJob
            {
                movementInputType = movementInputType,
                movementSpeedType = movementSpeedType,
                deltaTimeType = deltaTimeType,
                rotationType = rotationType,
                physicsVelocityType = physicsVelocityType
            };

            return job.Schedule( movementEntityQuery, inputDependencies );
        }
    }
}