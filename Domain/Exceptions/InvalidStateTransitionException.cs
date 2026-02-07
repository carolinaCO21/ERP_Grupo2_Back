namespace API.Domain.Exceptions
{
    /// <summary>
    /// Se lanza cuando se intenta realizar una transición de estado no permitida en un pedido.
    /// Controla que se respete la máquina de estados del ciclo de vida del pedido.
    /// </summary>
    public class InvalidStateTransitionException : DomainException
    {
        public string CurrentState { get; }
        public string AttemptedState { get; }

        public InvalidStateTransitionException(string currentState, string attemptedState)
            : base(
                $"No se puede cambiar el estado del pedido de '{currentState}' a '{attemptedState}'.",
                "INVALID_STATE_TRANSITION")
        {
            CurrentState = currentState;
            AttemptedState = attemptedState;
        }
    }
}
