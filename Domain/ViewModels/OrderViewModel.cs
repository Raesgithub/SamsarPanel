using System;

namespace Domain.ViewModels
{
    public class OrderViewModel   
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Cdate { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public bool IsNew { get; set; }
    }
}
