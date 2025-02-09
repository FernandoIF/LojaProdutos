using System.ComponentModel.DataAnnotations;
using LojaProdutos.Enums;
using LojaProdutos.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LojaProdutos.Dto.Usuario
{
    public class CriarUsuarioDTO
    {
        [Required(ErrorMessage = "Digite o nome!")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Digite o e-mail!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Informe o cargo!")]
        public CargoEnum Cargo { get; set; }
        [Required(ErrorMessage = "Digite o Logradouro!")]
        public string Logradouro { get; set; }
        [Required(ErrorMessage = "Digite o Bairro!")]
        public string Bairro { get; set; }
        [Required(ErrorMessage = "Digite o numero!")]
        public string Numero { get; set; }
        [Required(ErrorMessage = "Digite o CEP!")]
        public string CEP { get; set; }
        [Required(ErrorMessage = "Digite o estado!")]
        public string Estado { get; set; }
        public string? Complemento { get; set; }
        [Required(ErrorMessage = "Digite a senha!")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "Digite a confirmação de senha!"), Compare("Senha", ErrorMessage = "Senhas divergentes :/")]
        public string ConfirmarSenha { get; set; }
    }
}
