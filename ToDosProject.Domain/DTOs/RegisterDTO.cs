using System.ComponentModel.DataAnnotations;

namespace ToDosProject.Domain.DTOs
{
	public class RegisterDTO
	{
		[Required(ErrorMessage = "{0} obrigatório!")]
		[EmailAddress(ErrorMessage = "O campo deve conter um {0} válido!")]
		[Display(Name = "E-mail")]
		public string? Email { get; set; }

		[Required(ErrorMessage = "{0} obrigatória!")]
		[StringLength(100, ErrorMessage = "A {0} deve ter no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Senha")]
		public string? Password { get; set; }
	}

}
