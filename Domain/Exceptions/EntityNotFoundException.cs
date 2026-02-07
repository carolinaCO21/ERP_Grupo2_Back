namespace API.Domain.Exceptions
{
    /// <summary>
    /// Se lanza cuando no se encuentra una entidad solicitada en la base de datos.
    /// Permite especificar el tipo de entidad y el identificador buscado.
    /// </summary>
    public class EntityNotFoundException : DomainException
    {
        public string EntityName { get; }
        public object EntityId { get; }

        public EntityNotFoundException(string entityName, object entityId)
            : base($"No se encontró {entityName} con identificador '{entityId}'.", "ENTITY_NOT_FOUND")
        {
            EntityName = entityName;
            EntityId = entityId;
        }
    }
}
