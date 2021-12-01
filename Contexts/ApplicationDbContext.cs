using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiParquimetros.Controllers;
using WebApiParquimetros.Models;

namespace WebApiParquimetros.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)

        {

        }
        public DbSet<ApplicationUser> NetUsers { get; set; }
       //public DbSet<UserRoles> roles { get; set; }
        public DbSet<Agentes> tbagentes { get; set; }
        public DbSet<Zonas> tbzonas { get; set; }
        public DbSet<Comerciantes> tbcomerciantes { get; set; }
        public DbSet<Comisiones> tbcomisiones { get; set; }
        public DbSet<Ciudades> tbciudades { get; set; }
        public DbSet<Lugares> tblugares { get; set; }
        public DbSet<Vehiculos> tbvehiculos { get; set; }
        public DbSet<Tarjetas> tbtarjetas { get; set; }
        public DbSet<Saldos> tbsaldo { get; set; }
        public DbSet<Multas> tbmultas { get; set; }
        public DbSet<Movimientos> tbmovimientos { get; set; }
        //public DbSet<AspNetRoles> AspNetRoles { get; set; }
        public DbSet<Opciones> tbopciones { get; set; }
        public DbSet<Concesiones> tbconcesiones { get; set; }
        public DbSet<Permisos> tbpermisos { get; set; }
        public DbSet<Tarifas> tbtarifas { get; set; }
        public DbSet<Espacios> tbespacios { get; set; }
        public DbSet<TiposUsuarios> tbtiposusuarios { get; set; }
        public DbSet<Secciones> tbsecciones { get; set; }
        public DbSet<Parametros> tbparametros { get; set; }
        public DbSet<DetalleMovimientos> tbdetallemovimientos { get; set; }
        public DbSet<DetalleSaldo> tbdetallesaldo { get; set; }
        public DbSet<DetalleMulta> tbdetallemulta { get; set; }

        public DbSet<CatalogoOpciones> tbcatopciones { get; set; }
        public DbSet<ConcesionesOpciones> tbpcionesconcesion { get; set; }
        public DbSet<ResumenDiario> tbresumendiario { get; set; }
        public DbSet<ResumenSemanal> tbresumensemanal { get; set; }
        public DbSet<ResumenMensual> tbresumenmensual { get; set; }
        public DbSet<CatCiudades> tbcatciudades{ get; set; }
        public DbSet<UsuariosConcesiones> tbusersconcesiones { get; set; }

    }


}
