using AspNetCoreHero.Boilerplate.Application.Extensions;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Repositories;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Results;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Application.Features
{
    public class GetAllPatientsQuery : IRequest<PaginatedResult<PatientDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }

    public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, PaginatedResult<PatientDto>>
    {
        private readonly IRepositoryAsync<Patient> _repository;

        private readonly IApplicationDbContext _context;    

        private readonly IMapper _mapper;

        public GetAllPatientsQueryHandler(IRepositoryAsync<Patient> repository, IApplicationDbContext context, IMapper mapper)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<PatientDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            var patients = await _context.EntitySet<Patient>()
                                         .ProjectTo<PatientDto>(_mapper.ConfigurationProvider)
                                         .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return patients;
        }
    }
}
