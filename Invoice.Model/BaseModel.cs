using System;

namespace Invoice.Model
{
    public class BaseModel
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime CreatedTime { get; set; }
        public virtual DateTime ModifiedTime { get; set; }
        public virtual string Username { get; set; }
    }
}
