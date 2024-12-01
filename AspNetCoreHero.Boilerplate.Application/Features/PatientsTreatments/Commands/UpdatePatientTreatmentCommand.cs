using AspNetCoreHero.Boilerplate.Application.Exceptions;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Repositories;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Application.Features
{
    public class UpdatePatientTreatmentCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int Tooth { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public DateTime DateOfIntervention { get; set; }
        public string Payment { get; set; }

        public class UpdatePatientTreatmentCommandHandler : IRequestHandler<UpdatePatientTreatmentCommand, Result<int>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IApplicationDbContext _context;
            public readonly IMapper _mapper;
            public UpdatePatientTreatmentCommandHandler(IUnitOfWork unitOfWork,IApplicationDbContext context, IMapper mapper) 
            {
                _unitOfWork = unitOfWork;
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<int>> Handle(UpdatePatientTreatmentCommand command, CancellationToken cancellationToken)
            {
                var treatment = await _context.EntitySet<PatientTreatment>()
                                               .FirstOrDefaultAsync(x => x.Id == command.Id);

                if (treatment == null)
                {
                    throw new ApiException($"Vizita me id {command.Id} nuk u gjend ne database.");

                }
                else
                {
                    //patient = _mapper.Map<PatientTreatment>(command);

                    treatment.DateOfIntervention = command.DateOfIntervention;
                    treatment.Tooth = command.Tooth;
                    treatment.Diagnosis = command.Diagnosis;
                    treatment.Treatment = command.Treatment;
                    treatment.Payment = command.Payment;

                    await _context.UpdateAsync(treatment);

                    return Result<int>.Success(treatment.Id);
                }
            }
        }
    }
}