namespace ApiLoginJdepaz.Core.Models.Generic
{
    public class GenericResponse<T>
    {
        public HttpCodeStatus status { get; set; }
        public T Item { get; set; }
    }
}
