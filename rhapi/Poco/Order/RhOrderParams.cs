using Microsoft.AspNetCore.Mvc;
using rhapi.Enums;
using System;

namespace rhapi.Poco.Order
{
    [Serializable]
    public class RhOrderParams : QueryParams
    {
        //date format strings should be iso8601
        [FromQuery(Name = "created_at_start")]
        public DateTime? CreatedAtStart { get; init; }

        [FromQuery(Name = "created_at_end")]
        public DateTime? CreatedAtEnd { get; init; }

        [FromQuery(Name = "updated_at_start")]
        public DateTime? UpdatedAtStart { get; init; }

        [FromQuery(Name = "updated_at_end")]
        public DateTime? UpdatedAtEnd { get; init; }

        [FromQuery(Name = "symbol")]
        public string Symbol { get; init; }

        [FromQuery(Name = "id")]
        public Guid? Id { get; init; }

        [FromQuery(Name = "side")]
        public Side? Side { get; init; }

        [FromQuery(Name = "state")]
        public OrderState? State { get; init; }

        [FromQuery(Name = "type")]
        public OrderType? Type { get; init; }

        [FromQuery(Name = "cursor")]
        public string Cursor { get; init; }

        [FromQuery(Name = "limit")]
        public int? Limit { get; init; }
    }
}
