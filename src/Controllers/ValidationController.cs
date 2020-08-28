using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using simple_aspnetcore_react_shared_validation.Dtos;
using simple_aspnetcore_react_shared_validation.Services;
using System.Collections.Generic;

namespace simple_aspnetcore_react_shared_validation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ValidationController : ControllerBase
    {
        private readonly IValidationDescriptorService _validationDescriptorService;

        public ValidationController(IValidationDescriptorService validationDescriptorService)
        {
            _validationDescriptorService = validationDescriptorService;
        }

        [HttpGet(Name = "GetValidationDescriptors")]

        public IDictionary<string, Dictionary<string, List<PropertyValidatorInfo>>> GetValidationDescriptors()
        {
            return _validationDescriptorService.GetValidationDescriptors();
        }
    }
}
