using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Tool;

namespace LogicEntity.Operator
{
    internal class SqlNode
    {
        List<Func<string>> _Strings = new();

        List<KeyValuePair<string, object>> _parameters = new();

        internal Command GetCommand()
        {
            return new Command()
            {
                CommandText = string.Join(" ", _Strings.Select(s => s())),
                Parameters = _parameters.AsEnumerable()
            };
        }

        public SqlNode _Content(string content)
        {
            _Strings.Add(() => content);

            return this;
        }

        public SqlNode _Parameter(object parameter)
        {
            string name = ToolService.UniqueName();

            _parameters.Add(new KeyValuePair<string, object>(name, parameter));

            _Strings.Add(() => name);

            return this;
        }

        public SqlNode _LeftBracket
        {
            get
            {
                _Strings.Add(() => "(");

                return this;
            }
        }

        public SqlNode _RightBracket
        {
            get
            {
                _Strings.Add(() => ")");

                return this;
            }
        }

        internal class Command
        {
            public string CommandText { get; set; }

            public IEnumerable<KeyValuePair<string, object>> Parameters { get; set; }
        }
    }
}
