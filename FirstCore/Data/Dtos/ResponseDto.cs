using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCore.Data.Dtos
{
    public class ResponseDto
    {
        public int Status { get; set; }
        public bool Result { get; set; }
        public List<string> Errors { get; set; }
        public string Token { get; set; }
    }
}
