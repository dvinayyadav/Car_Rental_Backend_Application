using System.ComponentModel.DataAnnotations;

public class BookingDto
{
    public int BookingId { get; set; }

    [Required]
    public int UserId { get; set; } // Foreign Key to User

    [Required]
    public int CarId { get; set; } // Foreign Key to Car

    [Required]
    public DateTime BookingDate { get; set; }

    [Required]
    public DateTime PickupDate { get; set; }

    [Required]
    public DateTime ReturnDate { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }

    // Instead of full Cancellation objects, only store Cancellation IDs
    public List<int> CancellationIds { get; set; } = new List<int>();
}
