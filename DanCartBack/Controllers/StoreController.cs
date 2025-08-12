using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ECommerceAdmin.Services;
using ECommerceAdmin.DTOs;

namespace ECommerceAdmin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStores()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var stores = await _storeService.GetUserStoresAsync(userId);
            return Ok(stores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStore(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var store = await _storeService.GetStoreAsync(id, userId);
            if (store == null)
                return NotFound();

            return Ok(store);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStore([FromBody] CreateStoreDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var store = await _storeService.CreateStoreAsync(model, userId);
            return CreatedAtAction(nameof(GetStore), new { id = store.Id }, store);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStore(int id, [FromBody] UpdateStoreDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var store = await _storeService.UpdateStoreAsync(id, model, userId);
            if (store == null)
                return NotFound();

            return Ok(store);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _storeService.DeleteStoreAsync(id, userId);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/upload-logo")]
        public async Task<IActionResult> UploadLogo(int id, IFormFile file)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var logoUrl = await _storeService.UploadLogoAsync(id, file, userId);
            if (string.IsNullOrEmpty(logoUrl))
                return BadRequest("Failed to upload logo");

            return Ok(new { logoUrl });
        }
    }
}
