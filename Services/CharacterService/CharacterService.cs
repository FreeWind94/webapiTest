using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapiTest.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public CharacterService(IMapper mapper, DataContext context)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var character = _mapper.Map<Character>(newCharacter);
        _context.Characters.Add(character);
        await _context.SaveChangesAsync();
        var dbCaracters = await _context.Characters.ToListAsync();
        return dbCaracters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
        var character = await _context.Characters.FindAsync(id);
        if (character is null)
        {
            return new Exception($"Character with Id '{id}' not found.");
        }

        _context.Characters.Remove(character);
        await _context.SaveChangesAsync();

        var dbCaracters = await _context.Characters.ToListAsync();
        return dbCaracters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        var dbCaracters = await _context.Characters.ToListAsync();
        return dbCaracters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var dbCharacter = await _context.Characters.FindAsync(id);
        if (dbCharacter is null)
        {
            return new Exception($"Character with Id '{id}' not found.");
        }
        return _mapper.Map<GetCharacterDto>(dbCharacter);
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        var character = await _context.Characters.FindAsync(updatedCharacter.Id);
        if (character is null)
        {
            return new Exception($"Character with Id '{updatedCharacter.Id}' not found.");
        }

        character.Name = updatedCharacter.Name;
        character.HitPoints = updatedCharacter.HitPoints;
        character.Strength = updatedCharacter.Strength;
        character.Defence = updatedCharacter.Defence;
        character.Intelligence = updatedCharacter.Intelligence;
        character.Class = updatedCharacter.Class;

        await _context.SaveChangesAsync();

        var dbCaracters = await _context.Characters.ToListAsync();
        return dbCaracters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
    }
}