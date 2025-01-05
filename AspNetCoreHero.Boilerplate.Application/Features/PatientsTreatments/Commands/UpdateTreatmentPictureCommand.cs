using AspNetCoreHero.Boilerplate.Application.Exceptions;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Repositories;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Application.Features.Products.Commands.Update
{
    public class UpdateTreatmentPictureCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }

        public class UpdateTreatmentPictureCommandHandler : IRequestHandler<UpdateTreatmentPictureCommand, Result<int>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IProductRepository _productRepository;

            public UpdateTreatmentPictureCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Result<int>> Handle(UpdateTreatmentPictureCommand command, CancellationToken cancellationToken)
            {
                var treatment = await _context.EntitySet<PatientTreatment>()
                                                               .FirstOrDefaultAsync(x => x.Id == command.Id);
                if (treatment == null)
                {
                    throw new ApiException($"Treatment Not Found.");
                }
                else
                {
                    treatment.Picture = command.Image;
                    await _context.UpdateAsync(treatment);

                    return Result<int>.Success(treatment.Id);
                }
            }
        }
    }
}