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
using Web.Areas.Catalog.Models;

namespace AspNetCoreHero.Boilerplate.Web.Areas.Catalog.Controllers
{
    [Area("Catalog")]
    public class StaffController : BaseController<StaffController>
    {
        private readonly IExportPdfService exportPdfService;
        public StaffController(IExportPdfService exportPdfService)
        {
            this.exportPdfService = exportPdfService;
        }
        public IActionResult Index()
        {
            var model = new StaffUserViewModel();
            return View(model);
        }
        public async Task<IActionResult> LoadAll()
        {
            var response = await _mediator.Send(new GetAllStaffUsersQuery());
            var viewModel = _mapper.Map<List<StaffUserViewModel>>(response);
            return PartialView("_ViewAll", viewModel);
        }

        [Authorize(Policy = Permissions.Users.View)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            if (id == 0)
            {
                var patientViewModel = new StaffUserViewModel();
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", patientViewModel) });
            }
            else
            {
                var response = await _mediator.Send(new GetStaffUserById() { Id = id });
                if (response.Succeeded)
                {
                    var patientViewModel = _mapper.Map<StaffUserViewModel>(response.Data);
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", patientViewModel) });
                }
                return null;
            }
        }

        [HttpPost]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, StaffUserViewModel patient)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    var createPatientCommand = _mapper.Map<CreateStaffUserCommand>(patient);
                    var result = await _mediator.Send(createPatientCommand);
                    if (result.Succeeded)
                    {
                        id = result.Data;
                        _notify.Success($"U krijua me sukses.");
                    }
                    else _notify.Error(result.Message);
                }
                else
                {
                    var updateProductCommand = _mapper.Map<UpdateStaffUserCommand>(patient);
                    var result = await _mediator.Send(updateProductCommand);
                    if (result.Succeeded) _notify.Information($"Stafi me ID {result.Data} u perditesua me sukses.");
                }
                
                var response = await _mediator.Send(new GetAllStaffUsersQuery());
                if (response is not null && response.Any())
                {
                    var viewModel = _mapper.Map<List<StaffUserViewModel>>(response);
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = true, html = html });
                }
                else
                {
                    _notify.Error("Ka nje problem me sistemin");
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
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var result = await _mediator.Send(new DeleteStaffUserCommand { Id = id });
            if (result.Succeeded)
            {
                var deletedPatient = result.Data;
                _notify.Information($"Pacienti {deletedPatient.Name} {deletedPatient.Surname} u fshi.");
                
                var response = await _mediator.Send(new GetAllStaffUsersQuery());
                var viewModel = _mapper.Map<List<StaffUserViewModel>>(response);
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
