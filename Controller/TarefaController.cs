using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioTarefaAPI.Context;
using Microsoft.AspNetCore.Mvc;
using ProjetoTarefaAPI.Models;

namespace DesafioTarefaAPI.Controllers
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
            // IMPREMENTADO: Buscar o Id no banco utilizando o EF
            var tarefa = _context.Tarefas.Find(id);
            
            // IMPREMENTADO: Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
            // caso contrário retornar OK com a tarefa encontrada

            if(tarefa == null)
                return NotFound();
            return Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var tarefa = _context.Tarefas.ToList();
            // IMPREMENTADO: Buscar todas as tarefas no banco utilizando o EF
            return Ok(tarefa);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
             var tarefa = _context.Tarefas.Where(x => x.Titulo.Contains(titulo));
            // IMPREMENTADO: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            // Dica: Usar como exemplo o endpoint ObterPorData
            return Ok(tarefa);
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
            // IMPREMENTADO: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
            // Dica: Usar como exemplo o endpoint ObterPorData
            var tarefa = _context.Tarefas.Where(x => x.Status == status);
            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            
             // IMPREMENTADO: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
             _context.Add(tarefa);
             _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);




        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);
                

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            

             // IMPREMENTADO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
             tarefaBanco.Titulo = tarefa.Titulo;
             tarefaBanco.Descricao = tarefa.Descricao;
             tarefaBanco.Data = tarefa.Data;
             tarefaBanco.Status = tarefa.Status;


             // IMPREMENTADO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
             _context.Tarefas.Update(tarefaBanco);
             _context.SaveChanges();

             
             
            return Ok(tarefaBanco);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

             // IMPREMENTADO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
             _context.Tarefas.Remove(tarefaBanco);
             _context.SaveChanges();

            
            return NoContent();
        }
    }
}