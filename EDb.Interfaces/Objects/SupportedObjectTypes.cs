using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDb.Interfaces.Objects
{
    /// <summary>
    /// Поддерживаемые типы объектов СУБД
    /// </summary>
    public enum SupportedObjectTypes
    {
        Table = 0,
        PrimaryKey = 1,
        ForeignKey = 2,
        Index = 3,
        Trigger = 4,
        Sequence = 5,
        View = 6,
        Synonyms =7,
        StoredProcedure = 8,
        StoredFunction = 9,
        Package = 10,
        Job = 11,
        Rule = 12,
    }
}
