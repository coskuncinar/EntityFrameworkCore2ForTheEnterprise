﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineStore.API.Common.Controllers;
using OnlineStore.API.Common.Filters;
using OnlineStore.API.Common.Responses;
using OnlineStore.API.Warehouse.Requests;
using OnlineStore.API.Warehouse.Security;
using OnlineStore.Core.Business.Contracts;
using OnlineStore.Core.Business.Requests;

namespace OnlineStore.API.Warehouse.Controllers
{
#pragma warning disable CS1591
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WarehouseController : OnlineStoreController
    {
        readonly ILogger Logger;
        readonly IWarehouseService Service;

        public WarehouseController(ILogger<WarehouseController> logger, IWarehouseService service)
            : base()
        {
            Logger = logger;
            Service = service;
        }
#pragma warning restore CS1591

        /// <summary>
        /// Search products
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A sequence that contains the products</returns>
        /// <response code="200">Returns a list of products</response>
        /// <response code="401">If client is not authenticated</response>
        /// <response code="403">If client is not autorized</response>
        /// <response code="500">If there was an internal error</response>
        [HttpPost("search-product")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [OnlineStoreActionFilter]
        [Authorize(Policy = Policies.SearchProductsPolicy)]
        public async Task<IActionResult> SearchProductsAsync([FromBody] SearchProductsRequest request)
        {
            Logger?.LogDebug("{0} has been invoked", nameof(SearchProductsAsync));

            // Get response from business logic
            var response = await Service.GetProductsAsync(request.PageSize, request.PageNumber, request.ProductCategoryID);

            // Return as http response
            return response.ToHttpResponse();
        }

        /// <summary>
        /// Gets the product inventory for product and location
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A sequence of inventory transactions by product and warehouse</returns>
        /// <response code="200">Returns the inventory for product</response>
        /// <response code="401">If client is not authenticated</response>
        /// <response code="403">If client is not autorized</response>
        /// <response code="404">If id is not exists</response>
        /// <response code="500">If there was an internal error</response>
        [HttpPost("product-inventory")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [OnlineStoreActionFilter]
        [Authorize(Policy = Policies.GetProductInventoryPolicy)]
        public async Task<IActionResult> GetProductInventoryAsync([FromBody]GetProductInventoryRequest request)
        {
            Logger?.LogDebug("{0} has been invoked", nameof(GetProductInventoryAsync));

            // Get response from business logic
            var response = await Service.GetProductInventories(request.ProductID, request.LocationID);

            // Return as http response
            return response.ToHttpResponse();
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <returns>A sequence of inventory transactions by product and warehouse</returns>
        /// <response code="200">Returns the inventory for product</response>
        /// <response code="401">If client is not authenticated</response>
        /// <response code="403">If client is not autorized</response>
        /// <response code="404">If id is not exists</response>
        /// <response code="500">If there was an internal error</response>
        [HttpPost("product")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [OnlineStoreActionFilter]
        [Authorize(Policy = Policies.WarehouseManagerPolicy)]
        public async Task<IActionResult> PostProductAsync(PostProductRequest request)
        {
            Logger?.LogDebug("{0} has been invoked", nameof(PostProductAsync));

            var entity = request.GetProduct();

            entity.CreationUser = UserInfo.UserName;

            // Get response from business logic
            var response = await Service.CreateProductAsync(entity);

            // Return as http response
            return response.ToHttpResponse();
        }

        /// <summary>
        /// Updates the unit price for an existing product
        /// </summary>
        /// <returns>A confirmation for product unit price modification</returns>
        /// <response code="201">If unit price it was updated successfully</response>
        /// <response code="401">If client is not authenticated</response>
        /// <response code="403">If client is not autorized</response>
        /// <response code="404">If id is not exists</response>
        /// <response code="500">If there was an internal error</response>
        [HttpPut("update-product-unit-price/{id}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [OnlineStoreActionFilter]
        [Authorize(Policy = Policies.WarehouseManagerPolicy)]
        public async Task<IActionResult> PutProductUnitPriceAsync(int? id, [FromBody]UpdateProductUnitPriceRequest request)
        {
            Logger?.LogDebug("{0} has been invoked", nameof(PutProductUnitPriceAsync));

            // Get response from business logic
            Service.UserInfo = UserInfo;

            var response = await Service.UpdateProductUnitPriceAsync(id, request);

            // Return as http response
            return response.ToHttpResponse();
        }
    }
}
