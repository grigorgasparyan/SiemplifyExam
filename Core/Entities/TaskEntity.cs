using Core.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities
{
    public class TaskEntity
    {
        [Key]
        public string Id { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? ModificationTime { get; set; }
        [Required]
        public string TaskText { get; set; }
        public TaskStatusEnum Status { get; set; }
        public string ConsumerID { get; set; }
    }
}
