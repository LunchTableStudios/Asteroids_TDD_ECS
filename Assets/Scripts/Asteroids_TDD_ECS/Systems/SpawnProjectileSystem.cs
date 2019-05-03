namespace Asteroids_TDD_ECS
{
    using Unity.Entities;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Transforms;

    // [ UpdateInGroup( typeof( SimulationSystemGroup ) ) ]
    public class SpawnProjectileSystem : JobComponentSystem
    {
        private EntityQuery m_weaponFiringQuery;
        private BeginInitializationEntityCommandBufferSystem m_projectileEntityCommandBuffer;

        private struct SpawnProjectileJob : IJobChunk
        {
            [ ReadOnly ] public ArchetypeChunkComponentType<Weapon> WeaponType;
            [ ReadOnly ] public ArchetypeChunkComponentType<Rotation> RotationType;
            [ ReadOnly ] public ArchetypeChunkComponentType<Translation> TranslationType;
            [ ReadOnly ] public EntityCommandBuffer CommandBuffer;
            public uint LastSystemVersion;

            public void Execute( ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex )
            {
                bool weaponFiredChanged = chunk.DidChange<Weapon>( WeaponType, LastSystemVersion );

                if( !weaponFiredChanged  )
                    return;

                var chunkWeapons = chunk.GetNativeArray( WeaponType );
                var chunkTranslations = chunk.GetNativeArray( TranslationType );
                var chunkRotations = chunk.GetNativeArray( RotationType );

                for( int i = 0; i < chunk.Count; i++ )
                {
                    Weapon weapon = chunkWeapons[i];
                    Translation translation = chunkTranslations[i];
                    Rotation rotation = chunkRotations[i];

                    Entity projectile = CommandBuffer.Instantiate( weapon.ProjectilePrefab );
                    CommandBuffer.SetComponent( projectile, new Translation{ Value = translation.Value } );
                    CommandBuffer.SetComponent( projectile, new Rotation{ Value = rotation.Value } );
                }
            }
        }

        protected override void OnCreate()
        {
            m_weaponFiringQuery = GetEntityQuery(
                typeof( Weapon ),
                typeof( WeaponFired ),
                typeof( Rotation ),
                typeof( Translation )
            );

            m_projectileEntityCommandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

            m_weaponFiringQuery.SetFilterChanged( typeof( Weapon ) );
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            return ProcessSpawnProjectileJob( m_weaponFiringQuery, m_projectileEntityCommandBuffer, inputDependencies );
        }

        public JobHandle ProcessSpawnProjectileJob( EntityQuery query, BeginInitializationEntityCommandBufferSystem buffer, JobHandle inputDependencies = default( JobHandle ) )
        {
            SpawnProjectileJob job = new SpawnProjectileJob
            {
                LastSystemVersion = this.LastSystemVersion,
                WeaponType = GetArchetypeChunkComponentType<Weapon>( true ),
                RotationType = GetArchetypeChunkComponentType<Rotation>( true ),
                TranslationType = GetArchetypeChunkComponentType<Translation>( true ),
                CommandBuffer = buffer.CreateCommandBuffer()
            };

            JobHandle handle = job.Schedule( query, inputDependencies );

            buffer.AddJobHandleForProducer( handle );

            return handle;
        }
    }
}