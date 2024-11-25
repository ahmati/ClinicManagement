using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Repositories;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Application.Features.Patients.Commands.Delete
{
    public class DeletePatientCommand : IRequest<Result<Patient>>
    {
        public int Id { get; set; }

        public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, Result<Patient>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IApplicationDbContext _context;

            public DeletePatientCommandHandler(IUnitOfWork unitOfWork,IApplicationDbContext dbContext)
            {
                _unitOfWork = unitOfWork;
                _context = dbContext;
            }

            public async Task<Result<Patient>> Handle(DeletePatientCommand command, CancellationToken cancellationToken)
            {
                var patient = await _context.EntitySet<Patient>()
                                               .FirstOrDefaultAsync(x => x.Id == command.Id);

                await _context.DeleteAsync(patient);

                return Result<Patient>.Success(patient);
            }
        }
    }
}