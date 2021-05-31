using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace Place_my_bet.Models
{
    public class PlaceMyBetContext : DbContext
    {
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Mercado> Mercados { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Apuesta> Apuestas { get; set; }

        public PlaceMyBetContext() { }

        public PlaceMyBetContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("Server=127.0.0.1;Port=3306;Database=place_my_bet_3;Uid=root;password=;SslMode=none");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            CultureInfo culInfo = new System.Globalization.CultureInfo("es-ES");
            culInfo.NumberFormat.NumberDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            culInfo.NumberFormat.PercentDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = culInfo;

            DateTime date = DateTime.Now;
            string actualDate;
            actualDate = date.ToString("yyyy/MM/dd");

            modelBuilder.Entity<Evento>().HasData(new Evento(1, "Valencia C.F", "F.C Barcelona", actualDate,null));
            modelBuilder.Entity<Evento>().HasData(new Evento(2, "Cádiz C.F", "Madrid C.F", actualDate, null));
            modelBuilder.Entity<Evento>().HasData(new Evento(3, "Levante C.F", "Getafe C.F", actualDate, null));
            modelBuilder.Entity<Mercado>().HasData(new Mercado(1, 2.5, 1.9, 1.9, 100, 100, false, 1, null, null));
            modelBuilder.Entity<Mercado>().HasData(new Mercado(2, 3.5, 2.22, 1.66, 150, 200, false, 2, null, null));
            modelBuilder.Entity<Mercado>().HasData(new Mercado(3, 1.5, 1.9, 1.9, 450, 450, false, 3, null, null));
            modelBuilder.Entity<Mercado>().HasData(new Mercado(4, 1.5, 1.9, 1.9, 900, 900, false, 3, null, null));
            modelBuilder.Entity<Usuario>().HasData(new Usuario("Alba@yahoo.com", "Alba", "Keylin Abradelo", 33, "2020/11/12", null, null));
            modelBuilder.Entity<Usuario>().HasData(new Usuario("Gamelin@gmail.com", "Pedro", "Gamelín Prieto", 21, "2020/11/12", null, null));
            modelBuilder.Entity<Usuario>().HasData(new Usuario("Leon@gmail.com", "León", "Valiente López", 41, "2020/11/13", null, null));
            modelBuilder.Entity<Usuario>().HasData(new Usuario("Lupo@icloud.com", "Lobo", "Cabezali García", 24, "2020/11/13", null, null));
            modelBuilder.Entity<Usuario>().HasData(new Usuario("Yelstin@icloud.com", "Yelstin", "Huiri Kabeut", 18, "2020/11/14", null, null));
            modelBuilder.Entity<Cuenta>().HasData(new Cuenta(5168565334325460, "BBVA", 299.12, "Gamelin@gmail.com",null));
            modelBuilder.Entity<Cuenta>().HasData(new Cuenta(5203550659869059, "Santander", 1527.7, "Leon@gmail.com", null));
            modelBuilder.Entity<Cuenta>().HasData(new Cuenta(5219971858205527, "Bankia", 3359.12, "Lupo@icloud.com", null));
            modelBuilder.Entity<Cuenta>().HasData(new Cuenta(5328163982660763, "La Caixa", 1257.22, "Yelstin@icloud.com", null));
            modelBuilder.Entity<Cuenta>().HasData(new Cuenta(5409682368847308, "IGN", 2527.7, "Alba@yahoo.com", null));
            modelBuilder.Entity<Apuesta>().HasData(new Apuesta(1,"OVER", 1.9, 50, actualDate.AsDateTime(), 1, null,"Alba@yahoo.com",null));
            modelBuilder.Entity<Apuesta>().HasData(new Apuesta(2,"UNDER", 1.66, 50, actualDate.AsDateTime(), 2, null,"Lupo@icloud.com",null));
            modelBuilder.Entity<Apuesta>().HasData(new Apuesta(3,"OVER", 1.9, 50, actualDate.AsDateTime(), 3, null,"Yelstin@icloud.com", null));
            modelBuilder.Entity<Apuesta>().HasData(new Apuesta(4, "OVER", 1.9, 50, actualDate.AsDateTime(), 4, null,"Gamelin@gmail.com",null));

        }

    }
}