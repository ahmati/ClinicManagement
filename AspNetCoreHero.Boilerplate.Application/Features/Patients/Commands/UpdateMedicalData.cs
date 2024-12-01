using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Repositories;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Boilerplate.Domain.Entities.Catalog;
using AspNetCoreHero.Results;
using AutoMapper;
using Domain.Entities;
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
    public partial class UpdateMedicalDataCommand : IRequest<Result<int>> 
    { 
        public int Id { get; set; }
        public bool? HasIllness { get; set; }
        public string? IllnessDetails { get; set; }
        public bool? IsUnderTreatment { get; set; }
        public string? DoctorsName { get; set; }
        public string? MedicationsTreatment { get; set; }

        // Liste e semundjeve te mundshme 
        public bool? SemundjeGjaku { get; set; }
        public bool? TensionILarte { get; set; }
        public bool? TensionIUlet { get; set; }
        public bool? NderhyrjeKirugjikaleNeZemer { get; set; }
        public bool? PropblemeZemre { get; set; }
        public bool? EtheRaumatizmale { get; set; }
        public bool? Glaucoma { get; set; }
        public bool? AlergjiNgaLlastikuDorezave { get; set; }
        public bool? AlergjiNgaMedikamentet { get; set; }
        public bool? AlergjiNgaMetalet { get; set; }
        public bool? SemundjeMendore { get; set; }
        public bool? Epilepsi { get; set; }
        public bool? PerdoruesDroge { get; set; }
        public bool? AzemBronkiale { get; set; }
        public bool? Turbekuloze { get; set; }
        public bool? MjekimTumori { get; set; }
        public bool? Shtatzen { get; set; }
        public bool? AIDS { get; set; }
        public bool? SemundjeMelcie { get; set; }

        public string? ArsyejaEParaqitjesNeKlinike { get; set; }
    }

    public class UpdateMedicalDataCommandHandler : IRequestHandler<UpdateMedicalDataCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        private IUnitOfWork _unitOfWork { get; set; }

        public UpdateMedicalDataCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<int>> Handle(UpdateMedicalDataCommand request, CancellationToken cancellationToken)
        {
            var medicalData = await _context.GetById<MedicalData>(request.Id);  

            if (medicalData == null)
            {
                return Result<int>.Fail($"Te dhenat mjekesore me id {request.Id} nuk u gjenden ne database.");
            }

            //medicalData = _mapper.Map<MedicalData>(request);
            medicalData.HasIllness = request.HasIllness;
            medicalData.IllnessDetails = request.IllnessDetails;
            medicalData.IsUnderTreatment = request.IsUnderTreatment;
            medicalData.DoctorsName = request.DoctorsName;
            medicalData.MedicationsTreatment = request.MedicationsTreatment;
            medicalData.SemundjeGjaku = request.SemundjeGjaku;
            medicalData.TensionILarte = request.TensionILarte;
            medicalData.TensionIUlet = request.TensionIUlet;
            medicalData.NderhyrjeKirugjikaleNeZemer = request.NderhyrjeKirugjikaleNeZemer;
            medicalData.PropblemeZemre = request.PropblemeZemre;
            medicalData.EtheRaumatizmale = request.EtheRaumatizmale;
            medicalData.Glaucoma = request.Glaucoma;
            medicalData.AlergjiNgaLlastikuDorezave = request.AlergjiNgaLlastikuDorezave;
            medicalData.AlergjiNgaMedikamentet = request.AlergjiNgaMedikamentet;
            medicalData.AlergjiNgaMetalet = request.AlergjiNgaMetalet;
            medicalData.SemundjeMendore = request.SemundjeMendore;
            medicalData.Epilepsi = request.Epilepsi;
            medicalData.PerdoruesDroge = request.PerdoruesDroge;
            medicalData.AzemBronkiale = request.AzemBronkiale;
            medicalData.Turbekuloze = request.Turbekuloze;
            medicalData.MjekimTumori = request.MjekimTumori;
            medicalData.Shtatzen = request.Shtatzen;
            medicalData.AIDS = request.AIDS;
            medicalData.SemundjeMelcie = request.SemundjeMelcie;
            medicalData.ArsyejaEParaqitjesNeKlinike = request.ArsyejaEParaqitjesNeKlinike;

            await _context.UpdateAsync(medicalData);

            return Result<int>.Success(medicalData.Id);
        }
    }
}
