using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Repositories;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Boilerplate.Domain.Entities.Catalog;
using AspNetCoreHero.Results;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Application.Features
{
    public partial class CreateStaffUserCommand : IRequest<Result<int>>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BornDate { get; set; }
        public string Role { get; set; } 
        

    }

    public class CreateStaffUserCommandHandler : IRequestHandler<CreateStaffUserCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        private IUnitOfWork _unitOfWork { get; set; }

        public CreateStaffUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<int>> Handle(CreateStaffUserCommand request, CancellationToken cancellationToken)
        {
            var staff = _mapper.Map<StaffUser>(request);

            await _context.CreateAsync(staff); 

            return Result<int>.Success(staff.Id);
        }
    }
}
