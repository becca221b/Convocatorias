using Convocatorias.Application.Interfaces;
using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.Candidatos.Create
{
    public sealed class CandidatoCreateUseCase : IRequestHandler<CandidatoRequest, Guid>
    {
        private readonly ICandidatoRepository _candidatoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CandidatoCreateUseCase(ICandidatoRepository candidatoRepository, IUnitOfWork unitOfWork)
        {
            _candidatoRepository = candidatoRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CandidatoRequest request, CancellationToken cancellationToken)
        {
            /*
            //Verificar que el email no exista
            bool emailExists = _candidatoRepository.EmailExists(request.Email, cancellationToken);
            if (emailExists)
            {
                throw new ArgumentException("El correo electrónico ya está en uso.", nameof(request.Email));
            }*/

            //Crear el candidato
            var candidato = new Candidato(request.Nombre, request.Apellido, request.Email);
            await _candidatoRepository.AddAsync(candidato, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return candidato.Id;
        }
    }
}
