using Application.Interfaces.Shared;
using AspNetCoreHero.Boilerplate.Application.Constants;
using AspNetCoreHero.Boilerplate.Application.Features;
using AspNetCoreHero.Boilerplate.Application.Features.Patients.Commands.Delete;
using AspNetCoreHero.Boilerplate.Application.Features.Patients.Commands.Update;
using AspNetCoreHero.Boilerplate.Web.Areas.Catalog.Models;
using AspNetCoreHero.Boilerplate.Web.Controllers;
using AspNetCoreHero.Boilerplate.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Web.Areas.Catalog.Controllers
{
    [Area("Catalog")]
    public class PatientController : BaseController<PatientController>
    {
        private readonly IExportPdfService exportPdfService;
        public PatientController(IExportPdfService exportPdfService)
        {
            this.exportPdfService = exportPdfService;
        }
        public IActionResult Index()
        {
            var model = new PatientViewModel();
            return View(model);
        }
        public async Task<IActionResult> LoadAll()
        {
            var response = await _mediator.Send(new GetAllPatientsQuery());
            if (response.Succeeded)
            {
                var viewModel = _mapper.Map<List<PatientViewModel>>(response.Data);
                return PartialView("_ViewAll", viewModel);
            }
            return null;
        }

        [Authorize(Policy = Permissions.Users.View)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            if (id == 0)
            {
                var patientViewModel = new PatientViewModel();
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", patientViewModel) });
            }
            else
            {
                var response = await _mediator.Send(new GetPatientById() { Id = id });
                if (response.Succeeded)
                {
                    var patientViewModel = _mapper.Map<PatientViewModel>(response.Data);
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", patientViewModel) });
                }
                return null;
            }
        }

        [HttpPost]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, PatientViewModel patient)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    var createPatientCommand = _mapper.Map<CreatePatientCommand>(patient);
                    var result = await _mediator.Send(createPatientCommand);
                    if (result.Succeeded)
                    {
                        id = result.Data;
                        _notify.Success($"Pacienti u krijua me sukses.");
                    }
                    else _notify.Error(result.Message);
                }
                else
                {
                    var updateProductCommand = _mapper.Map<UpdatePatientCommand>(patient);
                    var result = await _mediator.Send(updateProductCommand);
                    if (result.Succeeded) _notify.Information($"Pacienti me ID {result.Data} u perditesua me sukses.");
                }
                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    var image = file.OptimizeImageSize(700, 700);
                    await _mediator.Send(new UpdatePatientImageCommand() { Id = id, Image = image });
                }
                var response = await _mediator.Send(new GetAllPatientsQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<PatientViewModel>>(response.Data);
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = true, html = html });
                }
                else
                {
                    _notify.Error(response.Message);
                    return null;
                }
            }
            else
            {
                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", patient);
                return new JsonResult(new { isValid = false, html = html });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePatientData(int id)
        {
            var result = await exportPdfService.GeneratePdf(id);

            return File(result.File, "application/pdf", result.Name);
        }

        [HttpPost]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var result = await _mediator.Send(new DeletePatientCommand { Id = id });
            if (result.Succeeded)
            {
                var deletedPatient = result.Data;
                _notify.Information($"Pacienti {deletedPatient.Name} {deletedPatient.Surname} u fshi.");
                
                var response = await _mediator.Send(new GetAllPatientsQuery());
                var viewModel = _mapper.Map<List<PatientViewModel>>(response.Data);
                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                return new JsonResult(new { isValid = true, html = html });
                
            }
            else
            {
                _notify.Error(result.Message);
                return null;
            }
        }
    }
}
