using System;

namespace Mtwx.Web.Entities
{
    public class EntityBase
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}