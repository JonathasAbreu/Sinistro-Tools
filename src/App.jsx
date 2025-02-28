import { useState, useEffect } from "react";


function App() {
  const [lojas, setLojas] = useState([]);
  const [busca, setBusca] = useState("");

  const handleSearch = () => {
    if (!busca.trim()) return;

    let endpoint = isNaN(busca)
      ? `https://localhost:7030/api/lojas/nome/${busca}`
      : `https://localhost:7030/api/lojas/codigo/${busca}`;

    fetch(endpoint)
      .then((res) => res.json())
      .then((data) => setLojas(Array.isArray(data) ? data : [data]))
      .catch((err) => console.error("Erro ao buscar lojas:", err));
  };

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold">Pesquisar no Tools</h1>

      <div className="flex gap-2 my-4">
        <input
          type="text"
          className="border p-2 w-full"
          placeholder="Buscar por nome ou código..."
          value={busca}
          onChange={(e) => setBusca(e.target.value)}
        />
        <button
          onClick={handleSearch}
        >
          Buscar
        </button>
      </div>

      {lojas.length > 0 && (
        <table className="w-full border-collapse border">
          <thead>
            <tr className="bg-gray-200">
              <th className="border p-2">Código da Loja</th>
              <th className="border p-2">Nome</th>
              <th className="border p-2">Responsável</th>
              <th className="border p-2">Maior Margem</th>
              <th className="border p-2">Menor Margem</th>
              <th className="border p-2">Observação</th>
            </tr>
          </thead>
          <tbody>
            {lojas.map((loja) => (
              <tr key={loja.codigo}>
                <td className="border p-2">{loja.codigo}</td>
                <td className="border p-2">{loja.nome}</td>
                <td className="border p-2">{loja.responsavel}</td>
                <td className="border p-2">{loja.maiorMargem}%</td>
                <td className="border p-2">{loja.menorMargem}%</td>
                <td className="border p-2">{loja.observacao}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default App;
