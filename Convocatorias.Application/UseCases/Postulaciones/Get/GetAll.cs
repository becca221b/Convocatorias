using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.Postulaciones.Get
{
    public sealed class GetAll
    {
        private readonly IPostulacionRepository _repository;

        public GetAll(IPostulacionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<Postulacion>> ExecuteAsync(CancellationToken ct)
        {
            var postulaciones = await _repository.GetAllAsync(ct);
            return postulaciones.ToList();
        }

    }
}
