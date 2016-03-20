using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManager {
    public class ApiResponse<T> {
        public T data {get; set; }
        public Boolean error { get; set; }
        public Int32 status { get; set; }

        public static ApiResponse<T> Error(T data, int status = 500) {
            ApiResponse<T> r = new ApiResponse<T>();
            r.error = true;
            r.status = status;
            r.data = data;

            return r;
        }

        public static ApiResponse<T> Success(T data) {
            ApiResponse<T> r = new ApiResponse<T>();
            r.error = false;
            r.status = 200;
            r.data = data;

            return r;
        }

        public ApiResponse() { }
    }
}
