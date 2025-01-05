using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Repositories;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Results;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Application.Features.Patients.Commands.Delete
{
    public class DeleteStaffUserCommand : IRequest<Result<StaffUser>>
    {
        public int Id { get; set; } 

        public class DeleteStaffUserCommandHandler : IRequestHandler<DeleteStaffUserCommand, Result<StaffUser>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IApplicationDbContext _context;

            public DeleteStaffUserCommandHandler(IUnitOfWork unitOfWork,IApplicationDbContext dbContext)
            {
                _unitOfWork = unitOfWork;
                _context = dbContext;
            }

            public async Task<Result<StaffUser>> Handle(DeleteStaffUserCommand command, CancellationToken cancellationToken)
            {
                var staff = await _context.EntitySet<StaffUser>()
                                               .FirstOrDefaultAsync(x => x.Id == command.Id);

                await _context.DeleteAsync(staff);

                return Result<StaffUser>.Success(staff); 
            }
        }
    }
}