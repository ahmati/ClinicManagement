using AspNetCoreHero.Boilerplate.Application.Extensions;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Repositories;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Results;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Application.Features
{
    public class GetAllPatientsQuery : IRequest<List<Patient>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }

    public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, List<Patient>>
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

        public async Task<List<Patient>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            var patients = await _context.EntitySet<Patient>()
                                         .ToListAsync();

            return patients;
        }
    }
}
