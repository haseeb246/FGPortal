using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Models
{
    public class ResponseDto
    {
        public ResponseDto()
        {
            IsSuccess = true;
            ResultSetList = new List<object>();
        }
        public bool IsSuccess { get; set; }
        public object ResultSet { get; set; }
        public List<object> ResultSetList { get; set; }
        public string FilePath { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
    }
}
