using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LogicEntity;

namespace LogicEntity.Json
{
    internal static class JsonPathParser
    {
#if DEBUG
        readonly static Regex PathMatch = new(@"(\[[^\[\]]*\])|([^.\[\]]+)");
#endif

        public static MemberAccess[] Parse(string jsonPath)
        {
            if (string.IsNullOrEmpty(jsonPath))
                throw new InvalidJsonPathException(jsonPath);

            List<MemberAccess> result = new();

            char first = jsonPath[0];

            MemberAccess.MemberAccessType? accessType = null;

            if (first != '[' && first != '.')
                accessType = MemberAccess.MemberAccessType.Property;

            List<char> current = new();

            foreach (char c in jsonPath)
            {
                if (c == '[')
                {
                    if (accessType is not null)
                    {
                        result.Add(new MemberAccess()
                        {
                            MemberName = new string(current.ToArray()),
                            AccessType = accessType.Value
                        });

                        current.Clear();
                    }

                    accessType = MemberAccess.MemberAccessType.Index;

                    continue;
                }

                if (c == ']')
                {
                    result.Add(new MemberAccess()
                    {
                        MemberName = new string(current.ToArray()),
                        AccessType = accessType.Value
                    });

                    current.Clear();

                    accessType = null;

                    continue;
                }

                if (accessType == MemberAccess.MemberAccessType.Index)
                {
                    current.Add(c);

                    continue;
                }

                if (c == '.')
                {
                    if (accessType is not null)
                    {
                        result.Add(new MemberAccess()
                        {
                            MemberName = new string(current.ToArray()),
                            AccessType = accessType.Value
                        });

                        current.Clear();
                    }

                    accessType = MemberAccess.MemberAccessType.Property;

                    continue;
                }

                if (accessType is null)
                    throw new InvalidJsonPathException(jsonPath);

                current.Add(c);
            }

            if (accessType is not null)
            {
                result.Add(new MemberAccess()
                {
                    MemberName = new string(current.ToArray()),
                    AccessType = accessType.Value
                });
            }

            return result.ToArray();
        }
    }
}
