using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    /// <summary>
    /// SelectNodeType 之间的互斥关系
    /// </summary>
    internal static class SelectNodeOperationProhibited
    {
        static Dictionary<SelectNodeType, SelectNodeType[]> _operationProhibited = new()
        {
            {
                SelectNodeType.From,
                new SelectNodeType[]
                {
                    SelectNodeType.From
                }
            },
            {
                SelectNodeType.Where,
                new SelectNodeType[]
                {
                    SelectNodeType.From
                }
            },
            {
                SelectNodeType.AggregateFunction,
                new SelectNodeType[]
                {
                    SelectNodeType.From,
                    SelectNodeType.Where,
                    SelectNodeType.AggregateFunction,
                    SelectNodeType.GroupBy,
                    SelectNodeType.Select,
                    SelectNodeType.Distinct,
                    SelectNodeType.Having,
                    SelectNodeType.OrderBy,
                    SelectNodeType.Limit
                }
            },
            {
                SelectNodeType.GroupBy,
                new SelectNodeType[]
                {
                    SelectNodeType.From,
                    SelectNodeType.Where,
                    SelectNodeType.AggregateFunction,
                    SelectNodeType.GroupBy
                }
            },
            {
                SelectNodeType.Select,
                new SelectNodeType[]
                {
                    SelectNodeType.From,
                    SelectNodeType.Where,
                    SelectNodeType.AggregateFunction,
                    SelectNodeType.GroupBy,
                    SelectNodeType.Select
                }
            },
            {
                SelectNodeType.Distinct,
                new SelectNodeType[]
                {
                    SelectNodeType.From,
                    SelectNodeType.AggregateFunction,
                    SelectNodeType.GroupBy
                }
            },
            {
                SelectNodeType.Having,
                new SelectNodeType[]
                {
                    SelectNodeType.From,
                    SelectNodeType.Where,
                    SelectNodeType.AggregateFunction,
                    SelectNodeType.GroupBy,
                    SelectNodeType.Select
                }
            },
            {
                SelectNodeType.OrderBy,
                new SelectNodeType[]
                {
                    SelectNodeType.From,
                    SelectNodeType.Where,
                    SelectNodeType.GroupBy,
                    SelectNodeType.Having
                }
            },
            {
                SelectNodeType.Limit,
                new SelectNodeType[]
                {
                    SelectNodeType.From,
                    SelectNodeType.Where,
                    SelectNodeType.AggregateFunction,
                    SelectNodeType.GroupBy,
                    SelectNodeType.Distinct,
                    SelectNodeType.Having,
                    SelectNodeType.OrderBy
                }
            }
        };


        public static IReadOnlyCollection<SelectNodeType> GetProhibitedNode(SelectNodeType node)
        {
            return _operationProhibited[node];
        }
    }
}
