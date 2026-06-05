using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace Convocatorias.Application.UseCases.Candidatos.Create
{
    public sealed record CandidatoRequest
    (
        string Nombre,
        string Apellido,
        string Email
    ): IRequest<Guid>;
}