using LojaAPI.Models;
using LojaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace LojaAPI.Controllers
{
    [ApiController]
    [Route("api/lojas")]
    public class LojaController : ControllerBase
    {
        private readonly LojaService _lojaService;

        public LojaController(LojaService lojaService)
        {
            _lojaService = lojaService;
        }

        [HttpGet]
        public IActionResult GetLojas()
        {
            var lojas = _lojaService.GetLojas();
            return Ok(lojas);
        }

        [HttpGet("nome/{nome}")]
        public IActionResult GetLojaPorNome(string nome)
        {
            var loja = _lojaService.GetLojaPorNome(nome);
            if (loja == null)
            {
                return NotFound("Loja não encontrada.");
            }
            return Ok(loja);
        }

        [HttpGet("codigo/{codigo}")]
        public IActionResult GetLojaPorCodigo(int codigo)
        {
            var loja = _lojaService.GetLojaPorCodigo(codigo);
            if (loja == null)
            {
                return NotFound("Loja não encontrada.");
            }
            return Ok(loja);
        }
    }
}
