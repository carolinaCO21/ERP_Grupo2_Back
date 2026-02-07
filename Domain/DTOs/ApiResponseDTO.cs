namespace API.Domain.DTOs
{
    /// <summary>
    /// DTO genérico que estandariza todas las respuestas de la API.
    /// Envuelve los datos en un formato consistente que incluye
    /// estado de la operación, mensaje descriptivo y los datos de respuesta.
    /// </summary>
    /// <typeparam name="T">Tipo de datos que contiene la respuesta.</typeparam>
    public class ApiResponseDTO<T>
    {
        /// <summary>Indica si la operación fue exitosa.</summary>
        public bool Success { get; set; }

        /// <summary>Mensaje descriptivo del resultado (éxito o error).</summary>
        public string Message { get; set; }

        /// <summary>Datos de la respuesta. Puede ser null en caso de error.</summary>
        public T? Data { get; set; }

        /// <summary>
        /// Inicializa una nueva respuesta API con los datos proporcionados.
        /// </summary>
        public ApiResponseDTO(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        /// <summary>Crea una respuesta exitosa con datos y mensaje.</summary>
        public static ApiResponseDTO<T> Ok(T data, string message = "Operación realizada correctamente.")
        {
            return new ApiResponseDTO<T>(true, message, data);
        }

        /// <summary>Crea una respuesta de error con un mensaje descriptivo.</summary>
        public static ApiResponseDTO<T> Error(string message)
        {
            return new ApiResponseDTO<T>(false, message);
        }
    }
}
