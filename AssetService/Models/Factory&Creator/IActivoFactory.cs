using AssetService.DTOs;

namespace AssetService.Models.Factory_Creator
{
    //Un Factory es un patrón de diseño que centraliza la creación de objetos,
    //desacopla el código de las clases concretas,
    //mejora la escalabilidad del sistema y permite trabajar con polimorfismo.
    public interface IActivoFactory
    {
        Activo Crear(ActivoDtoRequest dto);
    }
}
