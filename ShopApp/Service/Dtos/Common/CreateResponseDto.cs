using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dtos.Common
{
    public class CreateResponseDto
    {
        public CreateResponseDto(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
