using Core.Enumerations;

namespace Api.Responses
{
    public class ApiResponse<T>
    {
        public int Status { get; set; }
        public string Mensaje { get; set; }
        public T Data { get; set; }
        public Metadata2? Meta { get; set; }
        public int? TotalRecords { get; set; }

        public ApiResponse(string mensaje, int status)
        {
            Status = status;
            Mensaje = mensaje;
        }

        public ApiResponse(T data, int status)
        {
            Data = data;
            Status = status;
            Mensaje = "success";
        }

        public ApiResponse(T data, int status, int totalRecords)
        {
            Data = data;
            Status = status;
            TotalRecords = totalRecords;
        }

        public ApiResponse(T data, int status, string mensaje)
        {
            Data = data;
            Status = status;
            Mensaje = mensaje;
        }

        public ApiResponse(T data, ResponseHttp status, string mensaje)
        {
            Data = data;
            Status = (int)status;
            Mensaje = mensaje;
        }

        public ApiResponse(T data, int status, string mensaje, int totalRecords)
        {
            Data = data;
            Status = status;
            Mensaje = mensaje;
            TotalRecords = totalRecords;
        }
    }

    public class Metadata2
    {
        public int? RegistrosTotales { get; set; }
        public int? RegistrosPorPagina { get; set; }
        public int? NoPagina { get; set; }
    }


}