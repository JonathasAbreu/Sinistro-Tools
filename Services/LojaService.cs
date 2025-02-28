using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using LojaAPI.Models;

namespace LojaAPI.Services
{
    public class LojaService
    {
        private readonly string _dadosLojas = Path.Combine(Directory.GetCurrentDirectory(), "Dados", "lojas.json");
        private readonly object _lock = new();

        public List<Loja> GetLojas()
        {
            if (!File.Exists(_dadosLojas)) return new List<Loja>();

            try
            {
                string json = File.ReadAllText(_dadosLojas);

                return JsonSerializer.Deserialize<List<Loja>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Loja>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler o arquivo JSON: {ex.Message}");
                return new List<Loja>();
            }
        }

        public Loja? GetLojaPorNome(string nome)
        {
            var lojas = GetLojas();
            return lojas.FirstOrDefault(l => l.Nome != null && l.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
        }

        public Loja? GetLojaPorCodigo(int codigo)
        {
            var lojas = GetLojas();
            return lojas.FirstOrDefault(l => l.Codigo == codigo);
        }
    }
}