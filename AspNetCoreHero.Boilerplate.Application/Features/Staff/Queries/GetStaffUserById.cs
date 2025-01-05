using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Results;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Application.Features
{
    public class GetStaffUserById : IRequest<Result<StaffUser>>
    {
        public int Id { get; set; }

        public class GetStaffUserByIdQueryHandler : IRequestHandler<GetStaffUserById, Result<StaffUser>>
        {
            private readonly IMapper _mapper;
            private readonly IApplicationDbContext _context;
            public GetStaffUserByIdQueryHandler(IMapper mapper, IApplicationDbContext context)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<StaffUser>> Handle(GetStaffUserById query, CancellationToken cancellationToken)
            {
                var staff = await _context.EntitySet<StaffUser>()
                                         .AsNoTracking()
                                         .Where(x => x.Id == query.Id)
                                         .FirstOrDefaultAsync();

                return Result<StaffUser>.Success(staff);
            }
        }
    }
}
