using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Common.Contracts
{
    public interface IMongoEntity<TId>
    {
        TId Id { get; set; }
    }
}
