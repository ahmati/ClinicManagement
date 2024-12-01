using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Results;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Patients.Queries
{
    public class GetPatientMedicalDataQuery : IRequest<Result<MedicalData>>
    {
        public int Id { get; set; }

        public class GetPatientMedicalDataQueryHandler : IRequestHandler<GetPatientMedicalDataQuery, Result<MedicalData>>
        {
            private readonly IApplicationDbContext _context;
            public GetPatientMedicalDataQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Result<MedicalData>> Handle(GetPatientMedicalDataQuery query, CancellationToken cancellationToken)
            {
                var patient = await _context.EntitySet<MedicalData>()
                                         .AsNoTracking()
                                         .Where(x => x.Id == query.Id)
                                         .FirstOrDefaultAsync();

                return Result<MedicalData>.Success(patient);
            }
        }
    }
}