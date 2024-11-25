using AspNetCoreHero.Boilerplate.Application.Exceptions;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Repositories;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Application.Features.Patients.Commands.Update
{
    public class UpdatePatientImageCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }

        public class UpdatePatientImageCommandHandler : IRequestHandler<UpdatePatientImageCommand, Result<int>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IApplicationDbContext _context;

            public UpdatePatientImageCommandHandler(IUnitOfWork unitOfWork,IApplicationDbContext context)
            {
                _unitOfWork = unitOfWork;
                _context = context;
            }

            public async Task<Result<int>> Handle(UpdatePatientImageCommand command, CancellationToken cancellationToken)
            {
                var patient = await _context.EntitySet<Patient>()
                                               .FirstOrDefaultAsync(x => x.Id == command.Id);

                if (patient == null)
                {
                    throw new ApiException($"Pacienti me id {command.Id} nuk u gjend ne database.");

                }
                else
                {
                    patient.ProfilePicture = command.Image;
                    await _context.UpdateAsync(patient);

                    return Result<int>.Success(patient.Id);
                }
            }
        }
    }
}