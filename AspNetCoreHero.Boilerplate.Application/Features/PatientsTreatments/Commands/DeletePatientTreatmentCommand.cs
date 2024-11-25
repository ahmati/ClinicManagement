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
    public class DeletePatientTreatmentCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class DeletePatientTreatmentCommandHandler : IRequestHandler<DeletePatientTreatmentCommand, Result<int>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IApplicationDbContext _context;

            public DeletePatientTreatmentCommandHandler(IUnitOfWork unitOfWork,IApplicationDbContext dbContext)
            {
                _unitOfWork = unitOfWork;
                _context = dbContext;
            }

            public async Task<Result<int>> Handle(DeletePatientTreatmentCommand command, CancellationToken cancellationToken)
            {
                var patientTreatment = await _context.EntitySet<PatientTreatment>()
                                               .FirstOrDefaultAsync(x => x.Id == command.Id);

                await _context.DeleteAsync(patientTreatment);

                return Result<int>.Success(patientTreatment.Id);
            }
        }
    }
}