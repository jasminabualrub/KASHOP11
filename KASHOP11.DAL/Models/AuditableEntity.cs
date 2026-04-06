using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.DAL.Models
{ 
    public enum EntityStatus
    {
            Active=1,
            Inactive=2,
    }
    public class AuditableEntity
    {
        public string ? createdById { get; set; }
        public string ? updatedById { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime  ? updatedOn { get; set; }
        public EntityStatus status { get; set; } = EntityStatus.Active;
        public ApplicationUser ? createdBy { get; set; }
        public ApplicationUser ? updatedBy { get; set; }
    }
}
