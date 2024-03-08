using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace desafio.Models
{
    public class Funcionario
    {
        public int Id {get; set;}
        public string Nome {get; set;}
        public string Endereco {get; set;}
        public string Ramal {get; set;}
        public string EmailProfissional {get; set;}
        public string Departamento {get; set;}
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Salario {get; set;}
        public DateTimeOffset DataAimissao {get; set;}

        public Funcionario(){}
        public Funcionario(int id, string nome, string endereco, string ramal, string emailProfissional, string departamento, decimal salario, DateTimeOffset dataAdmissao)
        {
            this.Id = id;
            this.Nome = nome;
            this.Endereco = endereco;
            this.Ramal = ramal;
            this.EmailProfissional = emailProfissional;
            this.Departamento  = departamento;
            this.Salario  = salario;
            this.DataAimissao  = dataAdmissao;
        }
    }
}