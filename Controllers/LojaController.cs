using LojaAPI.Models;
using LojaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Xml;
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

        [HttpPost]
        public IActionResult AddLoja([FromBody] Loja loja, [FromQuery] bool isAdmin)
        {
            if (!_lojaService.AddLoja(loja, isAdmin))
            {
                return Forbid("Apenas administradores podem alterar informações das lojas.");
            }
            return CreatedAtAction(nameof(GetLojas), new { nome = loja.Nome }, loja);
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

        [HttpDelete("codigo/{codigo}")]
        public IActionResult DeleteLoja(int codigo, [FromQuery] bool isAdmin)
        {
            if (!isAdmin)
            {
                return Forbid("Apenas administradores podem excluir lojas.");
            }

            bool sucesso = _lojaService.DeleteLoja(codigo);
            if (!sucesso)
            {
                return NotFound($"Nenhuma loja encontrada com o código {codigo}.");
            }

            return NoContent();
        }

        [HttpPut("codigo/{codigo}")]
        public IActionResult UpdateLoja(int codigo, [FromBody] Loja lojaAtualizada, [FromQuery] bool isAdmin)
        {
            if (!isAdmin)
            {
                return Forbid("Apenas administradores podem atualizar lojas.");
            }

            bool sucesso = _lojaService.UpdateLoja(codigo, lojaAtualizada);
            if (!sucesso)
            {
                return NotFound($"Nenhuma loja encontrada com o código {codigo}.");
            }

            return NoContent();
        }

        [HttpPut("{codigo}")]
        public IActionResult AtualizarLoja(int codigo, [FromBody] Loja lojaAtualizada)
        {
            Console.WriteLine($"Recebido PUT para loja código: {codigo}"); // 👈 Teste no console

            var lojas = JsonUtils.LerLojas(); // Lê os dados do JSON
            Console.WriteLine($"📊 Lojas carregadas do JSON: {Newtonsoft.Json.JsonConvert.SerializeObject(lojas, Newtonsoft.Json.Formatting.Indented)}");
            var lojaExistente = lojas.FirstOrDefault(l => l.Codigo == codigo);

            if (lojaExistente == null)
                return NotFound("Loja não encontrada");

            // Atualiza os dados da loja
            lojaExistente.Nome = lojaAtualizada.Nome;
            lojaExistente.Responsavel = lojaAtualizada.Responsavel;
            lojaExistente.MaiorMargem = lojaAtualizada.MaiorMargem;
            lojaExistente.MenorMargem = lojaAtualizada.MenorMargem;
            lojaExistente.Observacao = lojaAtualizada.Observacao;

            JsonUtils.SalvarLojas(lojas); // Salva no JSON atualizado
            return Ok(lojaExistente);
        }
    }
}