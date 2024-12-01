using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Repositories;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Boilerplate.Domain.Entities.Catalog;
using AspNetCoreHero.Results;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Application.Features
{
    public partial class CreatePatientCommand : IRequest<Result<int>>
    {
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

    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Result<int>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        private IUnitOfWork _unitOfWork { get; set; }

        public CreatePatientCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper, IApplicationDbContext context)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<int>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = _mapper.Map<Patient>(request);

            patient.MedicalData = new MedicalData();

            await _context.CreateAsync(patient);

            return Result<int>.Success(patient.Id);
        }
    }
}
