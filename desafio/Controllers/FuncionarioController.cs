using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Data.Tables;
using desafio.Context;
using desafio.Models;
using Microsoft.AspNetCore.Mvc;

namespace desafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FuncionarioController : ControllerBase
    {
        //Trazendo o DB poder usar na API, será feito no ctor
        private readonly RhContext _context;
        //Trazendo a tabela de logs, será feito no ctor
        private readonly string _connectionString;
        private readonly string _tableName;

        public FuncionarioController(RhContext context, IConfiguration configuration)
        {
            this._context = context;
            //adicinando o endereço da tabela na connectionString
            this._connectionString = configuration.GetValue<string>("ConnectionStrings:SAConnectionString");
            this._tableName = configuration.GetValue<string>("ConnectionStrings:AzureTableName");
        }
    //Método para reuzo onde é possível acessar a tabela
        private TableClient GetTableClient()
        {
            var serviceClient = new TableServiceClient(_connectionString);
            var tableClient  = serviceClient.GetTableClient(_tableName);
        //Verifica se não existe uma tabela com o nome e cria, caso exista ele retorna a mesma.
            tableClient.CreateIfNotExists();
            return tableClient;
        }
//Metodos HTTP
    //Get
        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
        //Encontrando o Funcionário por id no DB
            var funcionario = _context.Funcionarios.Find(id);
        //Verificando se foi encontrando
            if(funcionario == null)
                return NotFound();
        //Caso Encontre
            return Ok(funcionario);
        }

    //Post
    [HttpPost]
    public IActionResult Criar(Funcionario funcionario)
    {
        //Adicionando o novo funcionario ao banco
        _context.Funcionarios.Add(funcionario);
        _context.SaveChanges();//Salvando alteracoes

        //Adicionando o FuncionarioLog na tabela
        var tableClient = this.GetTableClient();
        var funcionarioLog = new FuncionarioLog(funcionario, TipoAcao.Inclusao, funcionario.Departamento, Guid.NewGuid().ToString());
        //Salvando na tabela
        tableClient.UpsertEntity(funcionarioLog);
    
        //Retornando uma consulta chamando o HttpGet
        return CreatedAtAction(nameof(ObterPorId), new {id = funcionario.Id}, funcionario);
    }
    //Put
    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, Funcionario funcionario)
    {
        //Encontrando no banco o funcionario para atualizar
        var funcionarioBanco = _context.Funcionarios.Find(id);
        //Verificando se encontrou
        if(funcionarioBanco == null)
            return NotFound();
        
        //Caso encontrado atribui os novos valores
        funcionarioBanco.Nome = funcionario.Nome;
        funcionarioBanco.Endereco = funcionario.Endereco;
        funcionarioBanco.Ramal = funcionario.Ramal;
        funcionarioBanco.EmailProfissional = funcionario.EmailProfissional;
        funcionarioBanco.Departamento = funcionario.Departamento;
        funcionarioBanco.Salario = funcionario.Salario;
        funcionarioBanco.DataAimissao = funcionario.DataAimissao;
        //Salvando os valores no DB
        _context.Funcionarios.Update(funcionarioBanco);
        _context.SaveChanges();
        //Adicionando o funcionarioLog a tabela
        var tableClient = this.GetTableClient();
        var funcionarioLog = new FuncionarioLog(funcionarioBanco, TipoAcao.Atualizacao, funcionarioBanco.Departamento, Guid.NewGuid().ToString());
        tableClient.UpsertEntity(funcionarioLog);

        return Ok();
    }
    //Delete
    [HttpDelete("{id}")]
    public IActionResult Deletar(int id)
    {
        //Procurando o contato
        var funcionarioBanco = _context.Funcionarios.Find(id);
        //Verificando se encontrou
        if(funcionarioBanco == null)
            return NotFound();
        //Caso encontre é só remover
        _context.Funcionarios.Remove(funcionarioBanco);
        _context.SaveChanges();
        //Adicinando o log na tabela de logs
        var tableClient = this.GetTableClient();
        var funcionarioLog = new FuncionarioLog(funcionarioBanco, TipoAcao.Remocao, funcionarioBanco.Departamento, Guid.NewGuid().ToString());
        tableClient.UpsertEntity(funcionarioLog);

        return NoContent();
    }

    }
}