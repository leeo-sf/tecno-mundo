using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecnoMundo.Domain.Entities.Base
{
    public class BaseMessage
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("order_time")]
        public DateTime CreatedAt { get; set; }

        public BaseMessage()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }

        public BaseMessage(Guid id, DateTime createdAt)
        {
            Id = id;
            CreatedAt = createdAt;
        }
    }
}
