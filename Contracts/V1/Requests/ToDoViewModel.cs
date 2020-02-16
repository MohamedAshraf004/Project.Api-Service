using System;
using System.ComponentModel.DataAnnotations;

namespace Project.Api.Contracts.V1.Requests
{
    public class ToDoViewModel
    {
        public string Name { get; set; }
        public string Descreption { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
