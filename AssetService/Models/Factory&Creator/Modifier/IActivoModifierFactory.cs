namespace AssetService.Models.Factory_Creator.Modifier
{
    public interface IActivoModifierFactory
    {
        IActivoModifier Obtener(TipoActivo tipo);
    }
}
