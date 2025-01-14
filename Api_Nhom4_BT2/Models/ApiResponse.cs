using System.Text.Json.Serialization;

namespace Api_Nhom4_BT2.Models
{
    public class ApiResponse<T>
    {
        public int code { get; set; }
        public string status { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? description { get; set; }

        public ApiResponse(int code, string status, T? data = default, string? description = null)
        {
            this.code = code;
            this.status = status;
            this.data = data;
            this.description = description;
        }

        public static ApiResponse<T> success(T data)
        {
            return new ApiResponse<T>(0, "success", data);
        }

        public static ApiResponse<T> fail(string description) {
            return new ApiResponse<T>(1, "fail", default(T)!, description);
        }

        public ApiResponse<T> WithDescription(string description)
        {
            this.description = description;
            return this;
        }

    }
}
