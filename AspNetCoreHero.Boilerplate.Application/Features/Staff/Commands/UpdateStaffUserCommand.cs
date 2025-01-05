using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Repositories;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Boilerplate.Domain.Entities.Catalog;
using AspNetCoreHero.Results;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Application.Features
{
    public partial class UpdateStaffUserCommand : IRequest<Result<int>> 
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BornDate { get; set; }
        public string Role { get; set; }
        public string PhoneNo { get; set; }
    }

    public class UpdateStaffUserCommandHandler : IRequestHandler<UpdateStaffUserCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        private IUnitOfWork _unitOfWork { get; set; }

        public UpdateStaffUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<int>> Handle(UpdateStaffUserCommand request, CancellationToken cancellationToken)
        {
            var staff = await _context.GetById<StaffUser>(request.Id);  

            if (staff == null)
            {
                return Result<int>.Fail($"Rekordi me id {request.Id} nuk u gjend ne database.");
            }

            //patient = _mapper.Map<Patient>(request);
            staff.Name = request.Name;
            staff.Surname = request.Surname;
            staff.BornDate = request.BornDate;
            staff.Role = request.Role;
            staff.PhoneNo = request.PhoneNo;

            await _context.UpdateAsync(staff);

            return Result<int>.Success(staff.Id);
        }
    }
}
