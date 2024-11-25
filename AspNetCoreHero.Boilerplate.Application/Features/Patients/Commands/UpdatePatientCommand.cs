using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Repositories;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Boilerplate.Domain.Entities.Catalog;
using AspNetCoreHero.Results;
using AutoMapper;
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
    public partial class UpdatePatientCommand : IRequest<Result<int>> 
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] ProfilePicture { get; set; }
    }

    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        private IUnitOfWork _unitOfWork { get; set; }

        public UpdatePatientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<int>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _context.GetById<Patient>(request.Id);  

            if (patient == null)
            {
                return Result<int>.Fail($"Pacienti me id {request.Id} nuk u gjend ne database.");
            }

            //patient = _mapper.Map<Patient>(request);
            patient.Name = request.Name;
            patient.PhoneNumber = request.PhoneNumber;
            patient.Surname = request.Surname;
            patient.ProfilePicture = request.ProfilePicture;

            await _context.UpdateAsync(patient);

            return Result<int>.Success(patient.Id);
        }
    }
}
