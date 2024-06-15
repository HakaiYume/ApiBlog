using ApiBlog.Data.Response;

namespace ApiBlog.Services
{
    public static class Messages<T>
    {
        public static MsgResponse<T> Ok(object result, T? data = default, T? back = default)
        {
            var msgResponse = new MsgResponse<T> { type = "Ok", message = result };
            
            if (data != null) msgResponse.Data = data;

            if (back != null) msgResponse.Back = back;

            return msgResponse;
        }

        public static MsgResponse<T> Succeed(string Object, string Operation){
            return new MsgResponse<T> { type = "Ok", message = $"El {Object} se {Operation} corretamente."};
        }

        public static MsgResponse<T> NotFound(string entidad, string propiedad, string valor)
        {
            return new MsgResponse<T> { type = "NotFound", message = $"El {entidad} con {propiedad} = {valor} no existe o no puede ser encontrado" };
        }

        public static object InternalServerError(Exception ex)
        {
            return new { message = $"Se produjo un error interno al procesar la solicitud. Error: {ex}" };
        }

        public static object ApiHijaError(string codeDistrito, string exDistrito, string codeComunidad, string exComunidad, object? peticion = default)
        {
            return new {
                    Error = $"Se produjo un error inesperado en el llamado a las APIs hijas",
                    RespuestaDistrito = "Estatus: " + codeDistrito + ". Mensage: " + exDistrito,
                    RespuestaComunidad = "Estatus: " + codeComunidad + ". Mensage: " + exComunidad,
                    CuerpoPeticion = peticion
                };
        }

        public static MsgResponse<T> Unauthorized(string Nombre)
        {
            return new MsgResponse<T> {type = "Unauthorized", message = $"El Usuario {Nombre} no cuenta con los permisos para realizar esta accion." };
        }
    }
}
