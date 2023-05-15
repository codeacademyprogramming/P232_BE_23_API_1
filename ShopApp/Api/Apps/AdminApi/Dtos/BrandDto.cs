using System.ComponentModel.DataAnnotations;

namespace Api.Apps.AdminApi.Dtos
{
    public class BrandDto
    {
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
