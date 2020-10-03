using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VS.Core.Controllers;
using VS.Core.Mediator;
using VS.Customer.Api.Application.Commands;

namespace VS.Customer.Api.Controllers
{
    [Route("api/customers")]
    public class CustomerController : BaseController
    {
        private readonly IMediatorHandler _mediator;
        public CustomerController(IMediatorHandler mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var command = new RegisterCustomerCommand(Guid.NewGuid(), "Guilherme", "gui.jau12@icloud.com");
            var result = await _mediator.SendCommand(command);

            return GenerateResponse(result);
        }
    }
}
