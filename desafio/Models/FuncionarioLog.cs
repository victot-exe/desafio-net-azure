using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;

namespace desafio.Models
{


    public class FuncionarioLog : Funcionario, ITableEntity
    {
        public FuncionarioLog(){ }

        public FuncionarioLog(Funcionario funcionario, TipoAcao tipoAcao, string partitionKey, string rowKey)
        {
            base.Id = funcionario.Id;
            base.Nome = funcionario.Nome;
            base.Endereco = funcionario.Endereco;
            base.Ramal = funcionario.Ramal;
            base.EmailProfissional = funcionario.EmailProfissional;
            base.Departamento = funcionario.Departamento;
            base.Salario = funcionario.Salario;
            base.DataAimissao = funcionario.DataAimissao;

            this.TipoAcao = tipoAcao;
            this.JSON = JsonSerializer.Serialize(funcionario);
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        public string PartitionKey {get; set;}
        public string RowKey {get; set;}
        public DateTimeOffset? Timestamp {get; set;}
        public ETag ETag {get; set;}
        public TipoAcao TipoAcao{get; set;}
        public string JSON{get; set;}
    }
}

// :
//             base(funcionario.Id, funcionario.Nome, funcionario.Endereco, funcionario.Ramal, funcionario.EmailProfissional, funcionario. Departamento, funcionario.Salario, funcionario.DataAimissao)