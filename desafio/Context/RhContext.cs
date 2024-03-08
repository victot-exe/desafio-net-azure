using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using desafio.Models;
using Microsoft.EntityFrameworkCore;

namespace desafio.Context
{
    public class RhContext : DbContext
    {
        public RhContext(DbContextOptions<RhContext> options) : base (options){}

        public DbSet<Funcionario> Funcionarios {get; set;}
    }
}