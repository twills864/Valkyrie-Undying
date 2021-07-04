using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    /// <summary>
    /// Conveniently displays the properties of the tagged object in the debugger view.
    ///
    /// Based on the FlattenHierarchyProxy by Jared Bar.
    /// https://stackoverflow.com/a/4818542
    /// http://blogs.msdn.com/b/jaredpar/archive/2010/02/19/flattening-class-hierarchies-when-debugging-c.aspx
    /// </summary>
    [DebuggerTypeProxy(typeof(DebugViewerDebugView))]
    public sealed class DebugViewer
    {
        [DebuggerDisplay("{Value}", Name = "{Name,nq}", Type = "{Type.ToString(),nq}")]
        public struct Member
        {
            public string Name;
            public object Value;
            public Type Type;
            public Member(string name, object value, Type type)
            {
                Name = name;
                Value = value;
                Type = type;
            }

            public int CompareTo(Member two)
            {
                return Name.CompareTo(two.Name);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public readonly object Host;
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public Member[] Items;

        public DebugViewer(object target)
        {
            Host = target;
            Items = BuildMemberList();
        }

        private Member[] BuildMemberList()
        {
            var membersBelowFinal = new List<Member>();
            if (Host == null)
                return membersBelowFinal.ToArray();

            Type thisType = Host.GetType();
            do
            {
                Add(membersBelowFinal, thisType, true);
                thisType = thisType.BaseType;
            }
            while (thisType != null && !FinalDebugViewLevelAttribute.IsFinal(thisType));

            var membersAboveFinal = new List<Member>();

            while (thisType != null)
            {
                Add(membersAboveFinal, thisType, false);
                thisType = thisType.BaseType;
            }

            membersBelowFinal.Sort((one, two) => one.CompareTo(two));
            membersAboveFinal.Sort((one, two) => one.CompareTo(two));

            var extra = new Member("Base", membersAboveFinal, typeof(object[]));
            membersBelowFinal.Insert(0, extra);

            var ret = membersBelowFinal.ToArray();
            return ret;
        }

        private void Add(List<Member> list, Type thisType, bool showExceptions)
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Instance | BindingFlags.DeclaredOnly;

            var props = thisType.GetProperties(flags);
            foreach (var prop in props.Where(x => x.PropertyType != typeof(DebugViewer)))
            {
                bool isException = PropertyValueIsException(prop, out object value);

                if (isException && !showExceptions)
                    continue;

                string name = prop.Name;
                Type type = prop.PropertyType;
                var add = new Member(name, value, type);
                list.Add(add);
            }

            var fields = thisType.GetFields(flags);
            foreach (var field in fields.Where(x => !x.Name.EndsWith("_BackingField")))
            {
                string name = field.Name;
                object value = field.GetValue(Host);
                Type type = field.FieldType;
                var add = new Member(name, value, type);
                list.Add(add);
            }
        }

        private bool PropertyValueIsException(PropertyInfo prop, out object value)
        {
            bool ret;
            try
            {
                value = prop.GetValue(Host, null);
                ret = false;
            }
            catch (Exception ex)
            {
                value = ex;
                ret = true;
            }
            return ret;
        }

        // Introduce a proxy class to keep item sort order.
        internal class DebugViewerDebugView
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private DebugViewer _DebugViewer;

            public DebugViewerDebugView(DebugViewer debugViewer)
            {
                this._DebugViewer = debugViewer;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public Member[] Items => _DebugViewer.Items;
        }
    }
}
