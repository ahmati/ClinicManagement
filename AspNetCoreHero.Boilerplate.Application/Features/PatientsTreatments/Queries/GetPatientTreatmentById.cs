using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PatientsTreatmentHistory.Queries
{
    public class GetPatientTreatmentByIdQuery : IRequest<Result<PatientTreatment>>
    {
        public int Id { get; set; }

        public class GetPatientTreatmentByIdQueryHandler : IRequestHandler<GetPatientTreatmentByIdQuery, Result<PatientTreatment>>
        {
            private readonly IMapper _mapper;
            private readonly IApplicationDbContext _context;

            public GetPatientTreatmentByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<PatientTreatment>> Handle(GetPatientTreatmentByIdQuery query, CancellationToken cancellationToken)
            {
                PatientTreatment patientTreatment = await _context.EntitySet<PatientTreatment>()
                                         .Include(x => x.Patient)
                                         .Where(x => x.Id == query.Id)
                                         .FirstOrDefaultAsync();

                return Result<PatientTreatment>.Success(patientTreatment);
            }
        }
    }
}
