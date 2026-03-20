namespace Models;

public class Review
{
    private int _id;
    private int _productId;
    private int _userId;
    private int _rating;
    private string _comment;
    private DateTime _createdAt;

    public Review(int id, int productId, int userId, int rating, string comment)
    {
        _id = id;
        _productId = productId;
        _userId = userId;
        _rating = rating;
        _comment = comment;
        _createdAt = DateTime.Now;
    }

    public int GetId() => _id;
    public int GetProductId() => _productId;
    public int GetUserId() => _userId;
    public int GetRating() => _rating;
    public string GetComment() => _comment;
    public DateTime GetCreatedAt() => _createdAt;

    public string ToCsv() => $"{_id},{_productId},{_userId},{_rating},{_comment},{_createdAt:yyyy-MM-dd}";
}
