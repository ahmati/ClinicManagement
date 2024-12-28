using AspNetCoreHero.Boilerplate.Application.Extensions;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Repositories;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Application.Features
{
    public class GetPatientTreatmentsQuery : IRequest<List<PatientTreatment>>
    {
        public int PatientId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }

    public class GetPatientTreatmentsQueryHandler : IRequestHandler<GetPatientTreatmentsQuery, List<PatientTreatment>>
    {
        private readonly IRepositoryAsync<Patient> _repository;

        private readonly IApplicationDbContext _context;    

        private readonly IMapper _mapper;

        public GetPatientTreatmentsQueryHandler(IRepositoryAsync<Patient> repository, IApplicationDbContext context, IMapper mapper)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PatientTreatment>> Handle(GetPatientTreatmentsQuery request, CancellationToken cancellationToken)
        {
            var patients = await _context.EntitySet<PatientTreatment>()
                                         .Include(x => x.Patient)
                                         .Where(x => x.PatientId == request.PatientId)
                                         .ToListAsync();

            return patients;
        }
    }
}
