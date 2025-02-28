using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LojaAPI.Models;

public static class JsonUtils
{
    private static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dados", "lojas.json");

    // Lê os dados do JSON
    public static List<Loja> LerLojas()
    {
        Console.WriteLine($"📂 Tentando ler o arquivo JSON em: {Path.GetFullPath(filePath)}");

        if (!File.Exists(filePath))
        {
            Console.WriteLine("⚠ Arquivo JSON não encontrado! Criando uma lista vazia.");
            return new List<Loja>(); // Retorna uma lista vazia se não existir
        }

        string json = File.ReadAllText(filePath);
        Console.WriteLine($"📜 JSON lido: {json}"); // Depuração para ver se o JSON está correto

        var lojas = JsonSerializer.Deserialize<List<Loja>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Permite nomes camelCase no JSON
        }) ?? new List<Loja>();

        Console.WriteLine($"📊 Lojas carregadas do JSON: {JsonSerializer.Serialize(lojas, new JsonSerializerOptions { WriteIndented = true })}");

        return lojas;
    }
}
