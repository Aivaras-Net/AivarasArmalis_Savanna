using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Savanna.Core.Interfaces;
using Savanna.Web.Constants;
using Savanna.Web.Models;
using Savanna.Web.Services.Interfaces;
using System.Security.Claims;

namespace Savanna.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameSaveController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IGameSaveService _gameSaveService;
        private readonly IGameRenderer _gameRenderer;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<GameSaveController> _logger;

        public GameSaveController(
            IGameService gameService,
            IGameSaveService gameSaveService,
            IGameRenderer gameRenderer,
            UserManager<ApplicationUser> userManager,
            ILogger<GameSaveController> logger,
            IDatabaseInitializer databaseInitializer)
        {
            _gameService = gameService;
            _gameSaveService = gameSaveService;
            _gameRenderer = gameRenderer;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetSaves()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var saves = await _gameSaveService.GetSavesForUserAsync(userId);
                var saveViewModels = saves.Select(s => new GameSaveViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    CreatedAt = s.CreatedAt,
                    TotalAnimals = 0
                }).ToList();

                return Ok(saveViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, WebConstants.ErrorGettingSavesMessage);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveGame([FromBody] GameSaveViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                if (!_gameService.IsGameRunning)
                {
                    return BadRequest(WebConstants.NoActiveGameToSaveMessage);
                }

                string gameStateJson = _gameService.SerializeGameState();
                var save = await _gameSaveService.CreateSaveAsync(model.Name, gameStateJson, userId);

                return Ok(new { Id = save.Id, Message = WebConstants.SaveGameSuccessMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, WebConstants.ErrorSavingGameMessage);
                return StatusCode(500, string.Format(WebConstants.SaveGameErrorMessage, ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> LoadGame(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var save = await _gameSaveService.GetSaveByIdAsync(id);
                if (save == null || save.UserId != userId)
                {
                    return NotFound();
                }

                var renderer = _gameRenderer.CreateRenderer(_gameService.LogMessage);
                _gameService.LoadGameState(save.GameStateJson, renderer);

                return Ok(WebConstants.LoadGameSuccessMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, WebConstants.ErrorLoadingGameMessage);
                return StatusCode(500, string.Format(WebConstants.LoadGameErrorMessage, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSave(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var success = await _gameSaveService.DeleteSaveAsync(id, userId);
                if (!success)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, WebConstants.ErrorDeletingSaveMessage);
                return StatusCode(500, ex.Message);
            }
        }
    }
}