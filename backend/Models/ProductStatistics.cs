namespace ECommerce.Models
{
    /// <summary>
    /// Record që mban statistikat e llogarituara nga ProductService.GetStatistics().
    /// Logjika e kalkulimit ndodh 100% në Service layer — jo në UI dhe jo në Repository.
    /// </summary>
    public record ProductStatistics(
        int     Count,
        decimal Total,
        decimal Average,
        decimal Max,
        decimal Min
    );
}
