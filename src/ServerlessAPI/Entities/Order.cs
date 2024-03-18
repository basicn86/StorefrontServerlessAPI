using Amazon.DynamoDBv2.DataModel;

namespace ServerlessAPI.Entities;

// <summary>
/// Map the Book Class to DynamoDb Table
/// To learn more visit https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DeclarativeTagsList.html
/// </summary>
[DynamoDBTable("StorefrontSAMOrder")]
public class Order
{
    ///<summary>
    /// Map c# types to DynamoDb Columns 
    /// to learn more visit https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/MidLevelAPILimitations.SupportedTypes.html
    /// <summary>
    [DynamoDBHashKey] //Partition key
    public int Id { get; set; } = 0;
    [DynamoDBProperty]
    public decimal Price { get; set; } = 0;
    [DynamoDBProperty]
    public string Date { get; set; } = string.Empty;

    [DynamoDBIgnore]
    //Order Items, not in this table
    public IList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}