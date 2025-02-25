using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Property_Manager_WebApp.Models;

public partial class Message
{
    public int MessageId { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? SentAt { get; set; }

    [NotMapped]
    public virtual User? Receiver { get; set; } = null!;

    [NotMapped]
    public virtual User? Sender { get; set; } = null!;
}
