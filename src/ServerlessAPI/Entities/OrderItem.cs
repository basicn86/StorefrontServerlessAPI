﻿using Amazon.DynamoDBv2.DataModel;

namespace ServerlessAPI.Entities;

// <summary>
/// Map the Book Class to DynamoDb Table
/// To learn more visit https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DeclarativeTagsList.html
/// </summary>
[DynamoDBTable("StorefrontSAMOrderItem")]
public class OrderItem
{
    ///<summary>
    /// Map c# types to DynamoDb Columns 
    /// to learn more visit https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/MidLevelAPILimitations.SupportedTypes.html
    /// <summary>
    [DynamoDBHashKey] //Partition key
    public int Id { get; set; } = 0;

    //Foreign key to Order
    [DynamoDBProperty]
    public int OrderId { get; set; } = 0;

    [DynamoDBProperty]
    public string Name { get; set; } = string.Empty;
    [DynamoDBProperty]
    public decimal Price { get; set; } = 0;
    [DynamoDBProperty]
    public int Quantity { get; set; } = 0;
}