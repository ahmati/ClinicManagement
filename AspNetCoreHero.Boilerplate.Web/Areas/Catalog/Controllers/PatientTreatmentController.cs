using AspNetCoreHero.Boilerplate.Application.Constants;
using AspNetCoreHero.Boilerplate.Application.Features.Patients.Commands.Delete;
using AspNetCoreHero.Boilerplate.Application.Features.Patients.Commands.Update;
using AspNetCoreHero.Boilerplate.Application.Features.Products.Commands.Create;
using AspNetCoreHero.Boilerplate.Application.Features.Products.Queries.GetAllPaged;
using AspNetCoreHero.Boilerplate.Application.Features.Products.Queries.GetById;
using AspNetCoreHero.Boilerplate.Application.Features;
using AspNetCoreHero.Boilerplate.Web.Areas.Catalog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreHero.Boilerplate.Web.Abstractions;
using Web.Areas.Catalog.Models;
using Application.Features.PatientsTreatmentHistory.Queries;
using System.Linq;
using AspNetCoreHero.Boilerplate.Application.Features.Products.Commands.Update;
using AspNetCoreHero.Boilerplate.Web.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Services;

namespace Web.Areas.Catalog.Controllers
{
    [Area("Catalog")]
    public class PatientTreatmentController : BaseController<PatientTreatmentController>
    {
        public IActionResult Index(int id)
        {
            var model = new PatientTreatmentViewModel
            {
                PatientId = id
            };

            return View(model);
        }
        public async Task<IActionResult> LoadAll(int patientId)
        {
            var response = await _mediator.Send(new GetPatientTreatmentsQuery { PatientId = patientId});
            var viewModel = _mapper.Map<List<PatientTreatmentViewModel>>(response);
            return PartialView("_ViewAll", viewModel);
        }

        [Authorize(Policy = Permissions.Users.View)]
        public async Task<JsonResult> OnGetCreateOrEdit(int patientId, int id = 0)
        {
            var staffUsers = await _mediator.Send(new GetAllStaffUsersQuery());

            
            if (id == 0)
            {
                var patientViewModel = new PatientTreatmentViewModel() 
                {
                    PatientId = patientId
                };

                if (staffUsers is not null)
                {
                    var staffUserViewModels = _mapper.Map<List<StaffUserViewModel>>(staffUsers);
                    patientViewModel.StaffUsers = new SelectList(staffUserViewModels, nameof(StaffUserViewModel.Id), nameof(StaffUserViewModel.FullName), null, null);
                }
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", patientViewModel) });
            }
            else
            {
                var response = await _mediator.Send(new GetPatientTreatmentByIdQuery() { Id = id });
                if (response.Succeeded)
                {
                    var patientViewModel = _mapper.Map<PatientTreatmentViewModel>(response.Data);

                    if (staffUsers is not null)
                    {
                        var staffUserViewModels = _mapper.Map<List<StaffUserViewModel>>(staffUsers);
                        patientViewModel.StaffUsers = new SelectList(staffUserViewModels, nameof(StaffUserViewModel.Id), nameof(StaffUserViewModel.FullName), null, null);
                    }
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", patientViewModel) });
                }
                return null;
            }
        }

        [HttpPost]
        public async Task<JsonResult> OnPostCreateOrEdit(int patientId, int id, PatientTreatmentViewModel patientTreatment)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    var createPatientTreatment = _mapper.Map<CreatePatientTreatmentCommand>(patientTreatment);
                    createPatientTreatment.PatientId = patientId;

                    var result = await _mediator.Send(createPatientTreatment);
                    if (result.Succeeded)
                    {
                        id = result.Data;
                        _notify.Success($"Trajtimi pacientit u ruajt me sukses.");

                    }
                    else _notify.Error(result.Message);
                }
                else
                {
                    var updatePatientTreatment = _mapper.Map<UpdatePatientTreatmentCommand>(patientTreatment); 
                    updatePatientTreatment.Id = id;

                    var result = await _mediator.Send(updatePatientTreatment); 
                    if (result.Succeeded) _notify.Information($"Pacienti me ID {result.Data} u perditesua me sukses.");
                }
                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    var image = file.OptimizeImageSize(700, 700);
                    await _mediator.Send(new UpdateTreatmentPictureCommand() { Id = id, Image = image });
                }
                var response = await _mediator.Send(new GetPatientTreatmentsQuery { PatientId = patientTreatment.PatientId});
                if (response is not null && response.Any())
                {
                    var viewModel = _mapper.Map<List<PatientTreatmentViewModel>>(response);
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = true, html = html });
                }
                else
                {
                    _notify.Error("Error");
                    return null;
                }
            }
            else
            {
                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", patientTreatment);
                return new JsonResult(new { isValid = false, html = html });
            }
        }

        [HttpPost]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var result = await _mediator.Send(new DeletePatientTreatmentCommand { Id = id });
            if (result.Succeeded)
            {
                var deletedPatient = result.Data;
                _notify.Information($"Fshirja u krye me suskes.");

                var response = await _mediator.Send(new GetPatientTreatmentsQuery());
                var viewModel = _mapper.Map<List<PatientTreatmentViewModel>>(response);
                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                return new JsonResult(new { isValid = true, html = html });

            }
            else
            {
                _notify.Error(result.Message);
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadImage(int id) 
        {
            var result = await _mediator.Send(new GetPatientTreatmentByIdQuery { Id = id });

            return File(result.Data.Picture, "image/png",
                $"{result.Data.Patient.Name}/ {result.Data.Treatment ?? string.Empty}.png");
        }
    }

}
