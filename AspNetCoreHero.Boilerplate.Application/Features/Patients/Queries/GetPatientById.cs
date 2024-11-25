using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Application.Features
{
    public class GetPatientById : IRequest<Result<Patient>>
    {
        public int Id { get; set; }

        public class GetProductByIdQueryHandler : IRequestHandler<GetPatientById, Result<Patient>>
        {
            private readonly IMapper _mapper;
            private readonly IApplicationDbContext _context;
            public GetProductByIdQueryHandler(IMapper mapper, IApplicationDbContext context)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<Patient>> Handle(GetPatientById query, CancellationToken cancellationToken)
            {
                var patient = await _context.EntitySet<Patient>()
                                         .AsNoTracking()
                                         .Where(x => x.Id == query.Id)
                                         .FirstOrDefaultAsync();

                return Result<Patient>.Success(patient);
            }
        }
    }
}
