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

        public bool AddLoja(Loja loja, bool isAdmin)
        {
            if (!isAdmin)
            {
                Console.WriteLine("Acesso negado! Somente administradores podem adicionar lojas.");
                return false;
            }

            lock (_lock)
            {
                try
                {
                    var lojas = GetLojas();
                    lojas.Add(loja);

                    string diretorio = Path.GetDirectoryName(_dadosLojas) ?? "";
                    if (!Directory.Exists(diretorio))
                    {
                        Directory.CreateDirectory(diretorio);
                    }

                    string novoJson = JsonSerializer.Serialize(lojas, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(_dadosLojas, novoJson);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao escrever no arquivo JSON: {ex.Message}");
                    return false;
                }
            }
        }

        public Loja? GetLojaPorNome(string nome)
        {
            var lojas = GetLojas();
            return lojas.FirstOrDefault(l => l.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
        }

        public Loja? GetLojaPorCodigo(int codigo)
        {
            var lojas = GetLojas();
            return lojas.FirstOrDefault(l => l.Codigo == codigo);
        }
        public bool DeleteLoja(int codigo)
        {
            var lojas = GetLojas();
            var lojaParaRemover = lojas.FirstOrDefault(l => l.Codigo == codigo);

            if (lojaParaRemover == null)
            {
                return false;
            }

            lojas.Remove(lojaParaRemover);

            string novoJson = JsonSerializer.Serialize(lojas, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_dadosLojas, novoJson);

            return true;
        }

        public bool UpdateLoja(int codigo, Loja lojaAtualizada)
        {
            var lojas = GetLojas();
            var lojaIndex = lojas.FindIndex(l => l.Codigo == codigo);

            if (lojaIndex == -1)
            {
                return false;
            }

            lojas[lojaIndex] = lojaAtualizada;

            string novoJson = JsonSerializer.Serialize(lojas, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_dadosLojas, novoJson);

            return true;
        }

    }
}