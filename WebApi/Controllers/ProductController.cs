﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Products.Commands;
using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Commands.DeleteProductById;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Products.Queries.GetAllProducts;
using Application.Features.Products.Queries.GetProductById;
using Application.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Authorize(Roles = "Organizations")]
    [Produces("application/json")]
    [Route("{__tenant__}/api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController
    {


        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductsParameter filter)
        {
            return Ok(await Mediator.Send(new GetAllProductsQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetProductByIdQuery { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteProductByIdCommand { Id = id }));
        }


    }
}
