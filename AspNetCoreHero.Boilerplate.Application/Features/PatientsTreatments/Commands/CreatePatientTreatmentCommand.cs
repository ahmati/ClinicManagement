using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Repositories;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Boilerplate.Domain.Entities.Catalog;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Application.Features
{
    public partial class CreatePatientTreatmentCommand : IRequest<Result<int>>
    {
        public int Tooth { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public DateTime DateOfIntervention { get; set; }
        public string Payment { get; set; }
        public int StaffUserId { get; set; }  
        public int PatientId { get; set; }
    }

    public class CreatePatientTreatmentCommandHandler : IRequestHandler<CreatePatientTreatmentCommand, Result<int>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        private IUnitOfWork _unitOfWork { get; set; }

        public CreatePatientTreatmentCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper, IApplicationDbContext context)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<int>> Handle(CreatePatientTreatmentCommand request, CancellationToken cancellationToken)
        {
            var patient = _mapper.Map<PatientTreatment>(request);

            await _context.CreateAsync(patient); 

            return Result<int>.Success(patient.Id);
        }
    }
}
