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
using Domain.Entities;

namespace AspNetCoreHero.Boilerplate.Application.Features
{
    public class GetAllStaffUsersQuery : IRequest<List<StaffUser>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }

    public class GetAllStaffUSersQueryHandler : IRequestHandler<GetAllStaffUsersQuery, List<StaffUser>>
    {

        private readonly IApplicationDbContext _context;    

        private readonly IMapper _mapper;

        public GetAllStaffUSersQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<StaffUser>> Handle(GetAllStaffUsersQuery request, CancellationToken cancellationToken)
        {
            var staff = await _context.EntitySet<StaffUser>()
                                         .ToListAsync();

            return staff;
        }
    }
}
