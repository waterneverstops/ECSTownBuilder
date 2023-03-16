using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Grid;
using TownBuilder.MonoComponents;

namespace TownBuilder.Systems.UGui
{
    public class UpdatePopulationText : IEcsInitSystem, IEcsRunSystem
    {
        private const string PopulationTextTemplate = "Population: {0}";
        
        private readonly EcsCustomInject<UIMediator> _uiMediatorInject = default;

        private UIMediator _mediator;
        
        public void Init(IEcsSystems systems)
        {
            _mediator = _uiMediatorInject.Value;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var houseFilter = world.Filter<House>().End();

            var housePool = world.GetPool<House>();
            
            var population = 0;
            
            foreach (var houseEntity in houseFilter)
            {
                population += housePool.Get(houseEntity).Population;
            }
            
            _mediator.PopulationText.text = string.Format(PopulationTextTemplate, population);
        }
    }
}