using AspNetCoreHero.Boilerplate.Application.Exceptions;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Repositories;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Application.Features
{
    public class UpdatePatientTreatmentCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class UpdatePatientTreatmentCommandHandler : IRequestHandler<UpdatePatientTreatmentCommand, Result<int>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IApplicationDbContext _context;
            public UpdatePatientTreatmentCommandHandler(IUnitOfWork unitOfWork,IApplicationDbContext context) 
            {
                _unitOfWork = unitOfWork;
                _context = context;
            }

            public async Task<Result<int>> Handle(UpdatePatientTreatmentCommand command, CancellationToken cancellationToken)
            {
                var patient = await _context.EntitySet<PatientTreatment>()
                                               .FirstOrDefaultAsync(x => x.Id == command.Id);

                if (patient == null)
                {
                    throw new ApiException($"Vizita me id {command.Id} nuk u gjend ne database.");

                }
                else
                {
                    await _context.UpdateAsync(patient);

                    return Result<int>.Success(patient.Id);
                }
            }
        }
    }
}