using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    // https://stackoverflow.com/a/4818542
    // http://blogs.msdn.com/b/jaredpar/archive/2010/02/19/flattening-class-hierarchies-when-debugging-c.aspx
    // by Jared Par

    //[Obsolete("Doesn't seem to work the way I need it to.", true)]
    //public sealed class FlattenHierarchyProxy
    //{
    //    [DebuggerDisplay("{Value}", Name = "{Name,nq}", Type = "{Type.ToString(),nq}")]
    //    public struct Member
    //    {
    //        public string Name;
    //        public object Value;
    //        public Type Type;
    //        public Member(string name, object value, Type type)
    //        {
    //            Name = name;
    //            Value = value;
    //            Type = type;
    //        }
    //    }

    //    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    //    private readonly object _target;
    //    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    //    private Member[] _memberList;

    //    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    //    public Member[] Items
    //    {
    //        get
    //        {
    //            if (_memberList == null)
    //            {
    //                _memberList = BuildMemberList().ToArray();
    //            }
    //            else
    //            {
    //                _memberList = new Member[]
    //                {
    //                    new Member("count:", _memberList.Length, _memberList.GetType())
    //                };
    //            }
    //            return _memberList;
    //        }
    //    }

    //    public FlattenHierarchyProxy(object target)
    //    {
    //        _target = target;
    //    }

    //    private List<Member> BuildMemberList()
    //    {
    //        var list = new List<Member>();
    //        if (_target == null)
    //        {
    //            return list;
    //        }

    //        var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    //        var type = _target.GetType();
    //        foreach (var field in type.GetFields(flags).Where(x => !x.IsSpecialName).OrderBy(x => x.Name))
    //        {
    //            var value = field.GetValue(_target);
    //            list.Add(new Member(field.Name, value, field.FieldType));
    //        }

    //        foreach (var prop in type.GetProperties(flags).OrderBy(x => x.Name))
    //        {
    //            object value = null;
    //            try
    //            {
    //                value = prop.GetValue(_target, null);
    //            }
    //            catch (Exception ex)
    //            {
    //                value = ex;
    //            }
    //            list.Add(new Member(prop.Name, value, prop.PropertyType));
    //        }

    //        list.Add(new Member("testNaame", "test valuee", typeof(string)));

    //        return list;
    //    }
    //}
}
