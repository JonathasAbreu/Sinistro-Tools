using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LojaAPI.Models;

public static class JsonUtils
{
    private static readonly string filePath = "../Dados/lojas.json"; // Caminho do JSON

    // Lê os dados do JSON
    public static List<Loja> LerLojas()
    {
            Console.WriteLine($"📂 Tentando ler o arquivo JSON em: {filePath}");

        if (!File.Exists(filePath))
        {
            return new List<Loja>(); // Retorna uma lista vazia se não existir
        }

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<Loja>>(json) ?? new List<Loja>();
    }

    // Salva os dados no JSON
    public static void SalvarLojas(List<Loja> lojas)
    {
        string json = JsonSerializer.Serialize(lojas, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }
}
