using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Infrastructure.DTOs
{
    public class Permissions
    {
        public List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
            $"Permissions.{module}.Create",
            $"Permissions.{module}.View",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete",
            };
        }
        public class ProductsDetail
        {
            public const string View = "Permissions.ProductsDetail.View";
            public const string Create = "Permissions.ProductsDetail.Create";
            public const string Edit = "Permissions.ProductsDetail.Edit";
            public const string Delete = "Permissions.ProductsDetail.Delete";
            public const string Detail = "Permissions.ProductsDetail.Detail";
        }
    }
}
