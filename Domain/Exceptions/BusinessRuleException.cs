namespace API.Domain.Exceptions
{
    /// <summary>
    /// Se lanza cuando se viola una regla de negocio del ERP.
    /// Cubre validaciones como: entidades inactivas, productos fuera de catálogo,
    /// operaciones no permitidas según el estado del pedido, etc.
    /// </summary>
    public class BusinessRuleException : DomainException
    {
        public BusinessRuleException(string message)
            : base(message, "BUSINESS_RULE_VIOLATION")
        {
        }

        public BusinessRuleException(string message, string errorCode)
            : base(message, errorCode)
        {
        }
    }
}
