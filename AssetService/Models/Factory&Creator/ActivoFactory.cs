using AssetService.DTOs;

namespace AssetService.Models.Factory_Creator
{
    public class ActivoFactory : IActivoFactory
    {
        private readonly Dictionary<TipoActivo, IActivoCreator> _creators;
        public ActivoFactory(IEnumerable<IActivoCreator> creators)
        {
            _creators = creators.ToDictionary(c => c.Tipo, c => c);
        }
        public Activo Crear(ActivoDtoRequest dto)
        {
            if (!_creators.TryGetValue(dto.Tipo, out var creator))
                throw new Exception("El tipo de activo indicado no es soportado.");
            return creator.crear(dto);
        }
    }
}
