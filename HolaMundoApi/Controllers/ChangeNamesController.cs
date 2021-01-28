using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HolaMundoChangeNameApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//El código está enfocado a pasar el nombre como variable Patch 

namespace HolaMundoChangeNameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangeNamesController : ControllerBase
    {
        private readonly ChangeNameCTX _context;
        public ChangeNamesController(ChangeNameCTX context)
        {
            _context = context;
        }
        
        [HttpGet]
        public string GetHello(string s)
        {
            return "HOLA MUNDO";
        }

        //Filtra por id 
        [HttpGet("{id}")]

        public async Task<ActionResult<ChangeName>> GetChangeName(int id)
        {
            var employe = await _context.ChangeName.FindAsync(id);
            if (employe == null)
            {
                return NotFound();

            }
            return employe;
        }
        private bool EmployeExists(int id)
        {
            return _context.ChangeName.Any(e => e.id == id);
        }
        [HttpPost]
        public async Task<ActionResult<ChangeName>> PostChangeName(ChangeName changeName)
        {
            _context.ChangeName.Add(changeName);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetHello", new { id = changeName.id }, changeName);

        }
        //Método Patch que comprubea si existe el id en la base de datos temporal, devolverá un código en función del resultado de la ejecución del código  
        [HttpPatch("ChangeHello/{id}")]
        public async Task<IActionResult> ChangeHello(int id,string helloWorldName)
        {
            if (string.IsNullOrWhiteSpace(helloWorldName))
            {
                return BadRequest("Id no existe");
            }
            var ChangeName = await _context.ChangeName.FindAsync(id);
            if(ChangeName == null)
            {
                return NotFound();

            }
            if(await _context.ChangeName.Where(x => x.HelloWorldName == helloWorldName && x.id != id).AnyAsync())
            {
                return BadRequest("El id ya Existe");

            }
            ChangeName.HelloWorldName = helloWorldName;
            await _context.SaveChangesAsync();
            return StatusCode(200, ChangeName);
        }

        //Metodo que ejecutará el cambio 
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, JsonPatchDocument<ChangeName> _ChangeName)
        {
            var ChangeName = await _context.ChangeName.FindAsync(id);
            if (ChangeName == null)
            {
                return NotFound();
            }
            _ChangeName.ApplyTo(ChangeName, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);
            await _context.SaveChangesAsync();
            return Ok(ChangeName);

        }

    }
}
