using AssetService.DTOs;
using System.Reflection.Metadata.Ecma335;

namespace AssetService.Models.Factory_Creator.Modifier
{
    public class ActivoModifierFactory : IActivoModifierFactory
    {
        private readonly IEnumerable<IActivoModifier> _modifiers;
        public ActivoModifierFactory(IEnumerable<IActivoModifier> modifiers)
        {
            _modifiers = modifiers;
        }
        public IActivoModifier Obtener(TipoActivo tipo) => _modifiers.FirstOrDefault(m => m.Tipo == tipo) ?? throw new Exception($"No existe un modifier para el tipo {tipo}");
    }
}
