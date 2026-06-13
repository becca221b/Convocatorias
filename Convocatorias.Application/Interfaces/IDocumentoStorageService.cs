using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.Interfaces
{
    public interface IDocumentoStorageService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct = default);
        Task DeleteAsync(string fileUrl, CancellationToken ct = default);
    }
}
