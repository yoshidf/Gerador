﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IdentitySample.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
		public string Id { get; set; }

		[Required(AllowEmptyStrings = false)]
		[Display(Name = "Nome")]
		public string Nome { get; set; }

		[Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

		[Required(AllowEmptyStrings = false)]
		[Display(Name = "Empresa")]
		public int IDEmpresa { get; set; }

		public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}