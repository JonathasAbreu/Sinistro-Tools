using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LojaAPI.Models;
using LojaAPI.Services;

namespace LojaAPI.Services
{
    public class LojaService
    {
        private readonly string _dadosLojas = "dados/lojas.json";
    


public List<Loja> GetLojas()
        {
            if (!File.Exists(_dadosLojas)) return new List<Loja>();

            string json = File.ReadAllText(_dadosLojas);
            return JsonSerializer.Deserialize<List<Loja>>(json) ?? new List<Loja>();
        }

        public void AddLoja(Loja loja)
        {
            var lojas = GetLojas();
            lojas.Add(loja);

            string novoJson = JsonSerializer.Serialize(lojas, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_dadosLojas, novoJson);
        }
    }
}
    