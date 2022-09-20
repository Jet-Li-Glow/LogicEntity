using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    internal class JsonAccess
    {
        List<string> _jsonPath = new() { SqlNode.SqlString(SqlNode.JsonPathRoot) };

        public JsonAccess(string jsonDocument)
        {
            JsonDocument = jsonDocument;
        }

        public string JsonDocument { get; private set; }

        public string JsonPath => _jsonPath.Count > 1 ? SqlNode.Call(nameof(string.Concat), _jsonPath.ToArray()) : _jsonPath.FirstOrDefault();

        public bool Valid { get; private set; } = false;

        public void SetPathRoot(string root)
        {
            _jsonPath[0] = root;

            Valid = true;
        }

        public void Add(params string[] strs)
        {
            _jsonPath.AddRange(strs);

            Valid = true;
        }

        public override string ToString()
        {
            if (Valid)
                return SqlNode.Call("Json_UnQuote",
                    SqlNode.Call("Json_Extract",
                        JsonDocument,
                        JsonPath
                        )
                    );

            return JsonDocument;
        }
    }
}
