using CarRental.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Shared.CustomController
{
    public class CustomBaseController : ControllerBase
    {
        public IActionResult CreateActionResultInstance<T>(ResponseDto<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode
                = response.StatusCode
            };
        }
    }
}
