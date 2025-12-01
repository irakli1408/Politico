using Politico.Application.DTO.ArticeleModuls;

namespace Politico.Application.DTO.AboutOrg
{
    public sealed class AboutOrganizationAdminDto : AboutOrganizationDto
    {
        public long? CoverId { get; set; }
        public List<CommonMediaItemDto> Media { get; set; } = new();
    }

}
