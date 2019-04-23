using Talent.Common.Contracts;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Common.Contracts
{
    public interface IMongoCommon: IMongoEntity<string>
    {
        bool IsDeleted { get; set; }
    }
}
