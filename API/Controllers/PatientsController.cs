using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class PatientsController : ControllerBase
    {
        private readonly IPatientService patientService;

        public PatientsController(IPatientService patientService)
        {
            this.patientService = patientService;
        }
        [HttpPost("index/All")]
        public async Task<IActionResult> IndexAllPatients()
        {
            await patientService.DeleteAllPatients();
            await patientService.IndexAllPatientsAsync();
            return Ok();
        }

        [HttpPost("index/update")]
        public async Task<IActionResult> updateFromDatabase()
        {
            await patientService.UpdatePatientsFromDatabaseAsync();
            return Ok();
        }
    }
}
