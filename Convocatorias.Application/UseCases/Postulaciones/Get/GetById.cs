using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.Postulaciones.Get
{
    public sealed class GetById
    {
        private readonly IPostulacionRepository _repository;

        public GetById(IPostulacionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Postulacion> ExecuteAsync(Guid id, CancellationToken ct)
        {
            var postulacion = await _repository.GetByIdAsync(id, ct);
            if (postulacion == null)
            {
                throw new KeyNotFoundException($"No se encontró una postulacion con el ID {id}");
            }
            return postulacion;
        }
    }
}
