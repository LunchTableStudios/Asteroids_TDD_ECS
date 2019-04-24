namespace Asteroids_TDD_ECS
{
    using Unity.Entities;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Burst;
    using Unity.Transforms;
    using Unity.Mathematics;
    
    public class RotationSystem : JobComponentSystem
    {
        private EntityQuery rotationEntityQuery;

        [ BurstCompile ]
        private struct RotationJob : IJobChunk
        {
            [ ReadOnly ] public ArchetypeChunkComponentType<RotationInput> rotationInputType;
            [ ReadOnly ] public ArchetypeChunkComponentType<RotationSpeed> rotationSpeedType;
            [ ReadOnly ] public ArchetypeChunkComponentType<DeltaTime> deltaTimeType;
            public ArchetypeChunkComponentType<Rotation> rotationType;

            public void Execute( ArchetypeChunk chunk, int index, int firstEntityIndex )
            {
                NativeArray<Rotation> rotations = chunk.GetNativeArray( rotationType );

                for( int i = 0; i < chunk.Count; i++ )
                {
                    Rotation rotation = rotations[i];

                    rotations[i] = new Rotation{
                        Value = math.mul( math.normalize( rotation.Value ), CalculateRotationDelta( chunk, index ) )
                    };
                }
            }

            private quaternion CalculateRotationDelta( ArchetypeChunk chunk, int index )
            {
                float input = chunk.GetNativeArray( rotationInputType )[ index ].Value;
                float speed = chunk.GetNativeArray( rotationSpeedType )[ index ].Value;
                float deltaTime = chunk.GetNativeArray( deltaTimeType )[ index ].Value;

                return quaternion.EulerXYZ( new float3( 0, 0, input * speed * deltaTime ) );
            }
        }

        protected override void OnCreate()
        {
            rotationEntityQuery = GetEntityQuery(
                ComponentType.ReadOnly<RotationInput>(),
                ComponentType.ReadOnly<RotationSpeed>(),
                ComponentType.ReadOnly<DeltaTime>(),
                typeof( Rotation )
            );
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            ArchetypeChunkComponentType<RotationInput> rotationInputType = GetArchetypeChunkComponentType<RotationInput>( true );
            ArchetypeChunkComponentType<RotationSpeed> rotationSpeedType = GetArchetypeChunkComponentType<RotationSpeed>( true );
            ArchetypeChunkComponentType<DeltaTime> deltaTimeType = GetArchetypeChunkComponentType<DeltaTime>( true );
            ArchetypeChunkComponentType<Rotation> rotationType = GetArchetypeChunkComponentType<Rotation>( false );
            
            RotationJob job = new RotationJob
            {
                rotationInputType = rotationInputType,
                rotationSpeedType = rotationSpeedType,
                deltaTimeType = deltaTimeType,
                rotationType = rotationType
            };

            return job.Schedule( rotationEntityQuery, inputDependencies );
        }
    }
}