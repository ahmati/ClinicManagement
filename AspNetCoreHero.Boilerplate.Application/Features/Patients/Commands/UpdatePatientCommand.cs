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
        public DateTime BornDate { get; set; }
        public string Gender { get; set; }
        public string ParentName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Profession { get; set; }
        public string JobAddress { get; set; }

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
            patient.Surname = request.Surname;
            patient.PhoneNo = request.PhoneNo;
            patient.BornDate = request.BornDate;
            patient.Gender = request.Gender;
            patient.Address = request.Address;
            patient.Profession = request.Profession;
            patient.JobAddress = request.JobAddress;
            patient.ProfilePicture = request.ProfilePicture;

            await _context.UpdateAsync(patient);

            return Result<int>.Success(patient.Id);
        }
    }
}
