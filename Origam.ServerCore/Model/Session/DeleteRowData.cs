using System;
using System.ComponentModel.DataAnnotations;

namespace Origam.ServerCore.Model.Session
{
    public class DeleteRowData
    {
        [RequireNonDefault]
        public Guid SessionFormIdentifier { get; set; }
        [Required]
        public string Entity { get; set; }
        [Required]
        public object RowId { get; set; }
    }
}