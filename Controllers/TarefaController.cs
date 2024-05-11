using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // Buscar a tarefa com o ID especificado no banco de dados
            var tarefa = _context.Tarefas.Find(id);

            // Validar se a tarefa foi encontrada
            if (tarefa == null)
            {
                // Retornar NotFound se a tarefa não foi encontrada
                return NotFound();
            }

            // Retornar OK com a tarefa encontrada
            return Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // Buscar todas as tarefas no banco utilizando o EF
            var todasAsTarefas = _context.Tarefas.ToList();

            // Retornar OK com as tarefas encontradas
            return Ok(todasAsTarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // Buscar as tarefas no banco utilizando o EF que contenham o título recebido por parâmetro
            var tarefasComTitulo = _context.Tarefas.Where(t => t.Titulo.Contains(titulo)).ToList();

            // Retornar OK com as tarefas encontradas
            return Ok(tarefasComTitulo);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // Buscar as tarefas no banco utilizando o EF que tenham o status recebido por parâmetro
            var tarefasComStatus = _context.Tarefas.Where(t => t.Status == status).ToList();

            // Retornar OK com as tarefas encontradas
            return Ok(tarefasComStatus);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            // Validar os dados da tarefa
            if (tarefa.Data == DateTime.MinValue)
            {
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            }

            // Adicionar a tarefa ao EF e salvar as mudanças
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();

            // Retornar uma resposta HTTP 201 (Created) com a tarefa criada
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            // Buscar a tarefa no banco de dados pelo ID
            var tarefaBanco = _context.Tarefas.Find(id);

            // Verificar se a tarefa foi encontrada
            if (tarefaBanco == null)
            {
                return NotFound();
            }

            // Validar os dados da tarefa
            if (tarefa.Data == DateTime.MinValue)
            {
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            }

            // Atualizar as informações da tarefa existente com os dados recebidos via parâmetro
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            // Atualizar a tarefa no EF e salvar as mudanças
            _context.SaveChanges();

            // Retornar uma resposta HTTP 200 (OK)
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            // Buscar a tarefa no banco de dados pelo ID
            var tarefaBanco = _context.Tarefas.Find(id);

            // Verificar se a tarefa foi encontrada
            if (tarefaBanco == null)
            {
                return NotFound();
            }

            // Remover a tarefa encontrada do contexto e salvar as mudanças
            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();

            // Retornar uma resposta HTTP 204 (NoContent) para indicar sucesso
            return NoContent();
        }
    }
}
