using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace webapiTest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CharacterController : ControllerBase
{
    private readonly ICharacterService _characterService;

    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    private ActionResult<ServiceResponse<T>> ToOk<T>(ServiceResponse<T> response) {
        return response.Match<ActionResult<ServiceResponse<T>>>(r =>
        {
            return Ok(r);
        }, r =>
        {
            return NotFound(r);
        });
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<ServiceResponse<List<Character>>>> Get()
    {
        var response = await _characterService.GetAllCharacters();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
    {
        var response = await _characterService.GetCharacterById(id);
        return ToOk(response);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var response = await _characterService.AddCharacter(newCharacter);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        var response = await _characterService.UpdateCharacter(updatedCharacter);
        return ToOk(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> DeleteCharacter(int id)
    {
        var response = await _characterService.DeleteCharacter(id);
        return ToOk(response);
    }
}