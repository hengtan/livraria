using Microsoft.AspNetCore.Mvc;
using Livraria.Application.Commands;
using Livraria.Application.Handlers;
using Livraria.Application.Interfaces;

namespace Livraria.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivroController : ControllerBase
    {
        private readonly CriarLivroHandler _criarLivroHandler;
        private readonly AtualizarLivroHandler _atualizarLivroHandler;
        private readonly ExcluirLivroHandler _excluirLivroHandler;
        private readonly ConsultarLivrosQueryHandler _consultarLivrosQueryHandler;

        public LivroController(
            CriarLivroHandler criarLivroHandler,
            AtualizarLivroHandler atualizarLivroHandler,
            ExcluirLivroHandler excluirLivroHandler,
            ConsultarLivrosQueryHandler consultarLivrosQueryHandler)
        {
            _criarLivroHandler = criarLivroHandler;
            _atualizarLivroHandler = atualizarLivroHandler;
            _excluirLivroHandler = excluirLivroHandler;
            _consultarLivrosQueryHandler = consultarLivrosQueryHandler;
        }

        // Consultar todos os livros
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var livros = await _consultarLivrosQueryHandler.HandleAsync();
            return Ok(livros);
        }

        // Consultar livro por ID
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var livro = await _consultarLivrosQueryHandler.HandleByIdAsync(id);
            if (livro == null) return NotFound();
            return Ok(livro);
        }

        // Criar um novo livro
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLivroCommand command)
        {
            await _criarLivroHandler.HandleAsync(command);
            return Ok(new { Message = $"Livro {command.Titulo} criado com sucesso! " });
        }

        // Atualizar um livro existente
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLivroCommand command)
        {
            if (id != command.Id) return BadRequest("IDs não coincidem.");
            await _atualizarLivroHandler.HandleAsync(command);
            return Ok(new { Message = $"Livro {command.Titulo} atualizado com sucesso!" });
        }

        // Excluir um livro
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var livro = _consultarLivrosQueryHandler.HandleByIdAsync(id);
            await _excluirLivroHandler.HandleAsync(id);
            return Ok(new { Message = $"Livro {livro.Result?.Titulo} excluído com sucesso!" });
        }
    }
}
