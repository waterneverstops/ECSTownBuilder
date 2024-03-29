﻿using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Building;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Links;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;
using TownBuilder.SO;
using TownBuilder.SO.RoadSetup;
using UnityEngine;

namespace TownBuilder.Systems.Building
{
    public class RoadViewRefreshSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;
        private readonly EcsCustomInject<PrefabSetup> _prefabSetupInjection = default;

        private MapGrid _grid;
        private RoadPrefabSetup _roadPrefabSetup;

        public void Init(IEcsSystems systems)
        {
            _grid = _levelContextInjection.Value.MapGrid;
            _roadPrefabSetup = _prefabSetupInjection.Value.RoadPrefabSetup;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var roadsToRefreshFilter = world.Filter<RefreshRoadModel>().Inc<Road>().Inc<ViewSwapperLink>().End();
            var ghostRoadsToRefreshFilter = world.Filter<RefreshRoadModel>().Inc<GhostRoad>().Inc<ViewSwapperLink>().End();

            var cellPool = world.GetPool<Cell>();
            var viewSwapperPool = world.GetPool<ViewSwapperLink>();

            ProcessRoadFilter(roadsToRefreshFilter, cellPool, viewSwapperPool);
            ProcessRoadFilter(ghostRoadsToRefreshFilter, cellPool, viewSwapperPool);
        }

        private void ProcessRoadFilter(EcsFilter filter, EcsPool<Cell> cellPool, EcsPool<ViewSwapperLink> viewSwapperPool)
        {
            foreach (var roadToRefresh in filter)
            {
                var position = cellPool.Get(roadToRefresh).Position;
                var newView = _roadPrefabSetup.GetSuitableViewVariant(GetRoadNeighbours(position));
                viewSwapperPool.Get(roadToRefresh).Value.SwapView(newView);
            }
        }

        private RoadNeighborsByDirections GetRoadNeighbours(Vector2Int position)
        {
            var neighbours = new RoadNeighborsByDirections
            {
                Up = IsRoadAtPosition(new Vector2Int(position.x, position.y + 1)),
                Left = IsRoadAtPosition(new Vector2Int(position.x - 1, position.y)),
                Down = IsRoadAtPosition(new Vector2Int(position.x, position.y - 1)),
                Right = IsRoadAtPosition(new Vector2Int(position.x + 1, position.y))
            };
            return neighbours;
        }
        
        private bool IsRoadAtPosition(Vector2Int position)
        {
            if (!_grid.IsPositionInbound(position)) return false;
            var packedEntityWithWorld = _grid[position];

            if (packedEntityWithWorld.Unpack(out var world, out var entity))
            {
                var roadPool = world.GetPool<Road>();
                var ghostPool = world.GetPool<GhostRoad>();
                return roadPool.Has(entity) || ghostPool.Has(entity);
            }

            return false;
        }
    }
}