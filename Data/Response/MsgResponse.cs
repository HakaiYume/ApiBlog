using System;

namespace ApiBlog.Data.Response
{
    public partial class MsgResponse<T>
    {
        public MsgResponse()
        {
            Data = default!;
            Back = default!;
        }

        public string type { get; set; } = null!;

        public object? message { get; set; } = null!;

        public T Data { get; set; }

        public T Back { get; set; }
    }
}
