import { useState, useEffect } from "react";

function App() {
  const [lojas, setLojas] = useState([]);
  const [busca, setBusca] = useState("");
  const [isAdmin, setIsAdmin] = useState(false);
  const [editando, setEditando] = useState(null); // 👈 Loja sendo editada
  const [dadosEditados, setDadosEditados] = useState({}); // 👈 Dados alterados

  useEffect(() => {
    const params = new URLSearchParams(window.location.search);
    setIsAdmin(params.get("admin") === "true");
  }, []);

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

  // Função para ativar modo edição
  const ativarEdicao = (loja) => {
    setEditando(loja.codigo);
    setDadosEditados({ ...loja }); // Copia os dados da loja para edição
  };

  // Função para salvar alterações
  const salvarAlteracoes = (loja) => {
    console.log("Salvando loja:", loja); // 👀 Verificar o objeto loja
    console.log("Código da loja:", loja.codigo); // 👀 Verificar se o código está correto

    if (!loja.codigo) {
      console.error("Erro: código da loja está indefinido!");
      return;
    }

    fetch(`https://localhost:7030/api/lojas/${loja.codigo}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(dadosEditados),
    })
      .then((res) => {
        if (!res.ok) throw new Error("Erro ao salvar alterações");
        return res.json();
      })
      .then((data) => {
        setLojas((lojas) =>
          lojas.map((loja) => (loja.codigo === data.codigo ? data : loja))
        );
        setEditando(null);
      })
      .catch((err) => console.error("Erro ao salvar:", err));
  };

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold">Consulta de Lojas</h1>

      <div className="flex gap-2 my-4">
        <input
          type="text"
          className="border p-2 w-full"
          placeholder="Buscar por nome ou código..."
          value={busca}
          onChange={(e) => setBusca(e.target.value)}
        />
        <button
          className="bg-blue-500 text-white px-4 py-2"
          onClick={handleSearch}
        >
          Buscar
        </button>
      </div>

      {lojas.length > 0 && (
        <table className="w-full border-collapse border">
          <thead>
            <tr className="bg-gray-200">
              <th className="border p-2">Nome</th>
              <th className="border p-2">Responsável</th>
              <th className="border p-2">Telefone</th>
              <th className="border p-2">Maior Margem</th>
              <th className="border p-2">Menor Margem</th>
              <th className="border p-2">Observação</th>
              {isAdmin && <th className="border p-2">Ações</th>}
            </tr>
          </thead>
          <tbody>
            {lojas.map((loja) => (
              <tr key={loja.codigo}>
                <td className="border p-2">
                  {editando === loja.codigo ? (
                    <input
                      className="border p-1"
                      value={dadosEditados.nome}
                      onChange={(e) =>
                        setDadosEditados({
                          ...dadosEditados,
                          nome: e.target.value,
                        })
                      }
                    />
                  ) : (
                    loja.nome
                  )}
                </td>
                <td className="border p-2">
                  {editando === loja.codigo ? (
                    <input
                      className="border p-1"
                      value={dadosEditados.responsavel}
                      onChange={(e) =>
                        setDadosEditados({
                          ...dadosEditados,
                          responsavel: e.target.value,
                        })
                      }
                    />
                  ) : (
                    loja.responsavel
                  )}
                </td>
                <td className="border p-2">{loja.telefone}</td>
                <td className="border p-2">{loja.maiorMargem}%</td>
                <td className="border p-2">{loja.menorMargem}%</td>
                <td className="border p-2">{loja.observacao}</td>
                {isAdmin && (
                  <td className="border p-2">
                    {editando === loja.codigo ? (
                      <button className="bg-green-500 text-white px-2 py-1" onClick={() => salvarAlteracoes(loja)}>
                      Salvar
                    </button>
                    
                    ) : (
                      <button
                        className="bg-yellow-500 text-white px-2 py-1 mr-2"
                        onClick={() => ativarEdicao(loja)}
                      >
                        Editar
                      </button>
                    )}
                  </td>
                )}
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default App;
