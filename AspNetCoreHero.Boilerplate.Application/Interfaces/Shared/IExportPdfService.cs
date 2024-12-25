using Application.DTOs.File;
using System.Threading.Tasks;

namespace Application.Interfaces.Shared
{
    public interface IExportPdfService
    {
        Task<FileDto> GeneratePdf(int patientId);
    }
}
