namespace API.Domain.Exceptions
{
    /// <summary>
    /// Excepción base para todos los errores de dominio del ERP.
    /// Proporciona una jerarquía de excepciones que permite distinguir
    /// errores de negocio de errores técnicos en las capas superiores.
    /// </summary>
    public abstract class DomainException : Exception
    {
        /// <summary>
        /// Código de error interno para identificar el tipo de excepción en el frontend.
        /// </summary>
        public string ErrorCode { get; }

        protected DomainException(string message, string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        protected DomainException(string message, string errorCode, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
