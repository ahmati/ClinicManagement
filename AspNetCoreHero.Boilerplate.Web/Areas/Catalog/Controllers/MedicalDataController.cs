using Application.Features.Patients.Queries;
using AspNetCoreHero.Boilerplate.Application.Features;
using AspNetCoreHero.Boilerplate.Application.Features.Patients.Commands.Update;
using AspNetCoreHero.Boilerplate.Web.Abstractions;
using AspNetCoreHero.Boilerplate.Web.Areas.Catalog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Areas.Catalog.Models;

namespace Web.Areas.Catalog.Controllers
{
    [Area("Catalog")]
    public class MedicalDataController :BaseController<MedicalDataController>
    {
        public IActionResult Index(int id)
        {
            var model = new MedicalDataViewModel() {Id = id};
            return View(model);
        }
        public async Task<IActionResult> LoadAll(int id)
        {
            var response = await _mediator.Send(new GetPatientMedicalDataQuery() { Id = id});
            if (response.Succeeded)
            {
                var viewModel = _mapper.Map<MedicalDataViewModel>(response.Data) ?? new MedicalDataViewModel() { SemundjeGjaku = true};
                return PartialView("_ViewAll", viewModel);
            }
            return null;
        }

        [HttpPost]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, MedicalDataViewModel medicalData)
        {
            if (ModelState.IsValid)
            {
                if (id != 0)
                {
                    medicalData.Id = id;    
                    var updateProductCommand = _mapper.Map<UpdateMedicalDataCommand>(medicalData);
                    var result = await _mediator.Send(updateProductCommand);
                    if (result.Succeeded) _notify.Information($"Pacienti me ID {result.Data} u perditesua me sukses.");
                }
                
                var response = await _mediator.Send(new GetPatientMedicalDataQuery() { Id = id });
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<MedicalDataViewModel>(response.Data);
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
                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", medicalData);
                return new JsonResult(new { isValid = false, html = html });
            }
        }
    }
}
