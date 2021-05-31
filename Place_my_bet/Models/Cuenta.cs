using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Place_my_bet.Models
{
    public class Cuenta
    {
        public Cuenta(long tarjetaId, string nombre_Banco, double saldo_Actual, string usuarioId, Usuario usuario)
        {
            TarjetaId = tarjetaId;
            Nombre_Banco = nombre_Banco;
            Saldo_Actual = saldo_Actual;
            UsuarioId = usuarioId;
            Usuario = usuario;
        }

        public Cuenta() { }

        [Key]
        public long TarjetaId { get; set; }
        [Required]
        public string Nombre_Banco { get; set; }
        public double Saldo_Actual { get; set; }
        [Required]
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

       
    }
}