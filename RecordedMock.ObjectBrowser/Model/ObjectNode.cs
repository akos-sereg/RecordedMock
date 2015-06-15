using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RecordedMock.ObjectBrowser.Model
{
    class ObjectNode
    {
        #region Private Properties

        private string _name;
        private object _value;
        private Type _type;

        #endregion

        #region Constructor

        public ObjectNode(object value)
        {
            if (value == null)
            {
                return;
            }

            ParseObjectTree("root", value, value.GetType());
        }

        public ObjectNode(string name, object value)
        {
            if (value == null)
            {
                return;
            }

            ParseObjectTree(name, value, value.GetType());
        }

        public ObjectNode(object value, Type t)
        {
            if (value == null)
            {
                return;
            }

            ParseObjectTree("root", value, t);
        }

        public ObjectNode(string name, object value, Type t)
        {
            if (value == null)
            {
                return;
            }

            if (value is BitmapImage)
            {
                this._name = name;
                this._value = "BitmapImage";
                return;
            }

            ParseObjectTree(name, value, t);
        }

        #endregion

        #region Public Properties

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public object Value
        {
            get
            {
                return _value;
            }
        }

        public Type Type
        {
            get
            {
                return _type;
            }
        }

        public List<ObjectNode> Children { get; set; }

        #endregion

        #region Private Methods

        private void ParseObjectTree(string name, object value, Type type)
        {
            Children = new List<ObjectNode>();

            _type = type;
            _name = name;

            if (value != null)
            {
                if (value is string && type != typeof(object))
                {
                    if (value != null)
                    {
                        _value = "\"" + value + "\"";
                    }
                }
                else if (value is double || value is bool || value is int || value is float || value is long || value is decimal)
                {
                    _value = value;
                }
                else
                {
                    _value = "{" + value.ToString() + "}";
                }
            }

            PropertyInfo[] props = type.GetProperties();

            if (props.Length == 0 && type.IsClass && value is IEnumerable && !(value is string))
            {
                IEnumerable arr = value as IEnumerable;

                if (arr != null)
                {
                    int i = 0;
                    foreach (object element in arr)
                    {
                        Children.Add(new ObjectNode("[" + i + "]", element, element.GetType()));
                        i++;
                    }

                }
            }

            foreach (PropertyInfo p in props)
            {
                if (p.PropertyType.IsPublic)
                {
                    if (p.PropertyType.IsClass || p.PropertyType.IsArray)
                    {
                        if (p.PropertyType.IsArray)
                        {
                            try
                            {
                                object v = p.GetValue(value, null);
                                IEnumerable arr = v as IEnumerable;

                                ObjectNode arrayNode = new ObjectNode(p.Name, arr.ToString(), typeof(object));

                                if (arr != null)
                                {
                                    int i = 0, k = 0;
                                    ObjectNode arrayNode2;

                                    foreach (object element in arr)
                                    {
                                        //Handle 2D arrays
                                        if (element is IEnumerable && !(element is string))
                                        {
                                            arrayNode2 = new ObjectNode("[" + i + "]", element.ToString(), typeof(object));

                                            IEnumerable arr2 = element as IEnumerable;
                                            k = 0;

                                            foreach (object e in arr2)
                                            {
                                                arrayNode2.Children.Add(new ObjectNode("[" + k + "]", e, e.GetType()));
                                                k++;
                                            }

                                            arrayNode.Children.Add(arrayNode2);
                                        }
                                        else
                                        {
                                            arrayNode.Children.Add(new ObjectNode("[" + i + "]", element, element.GetType()));
                                        }
                                        i++;
                                    }

                                }

                                Children.Add(arrayNode);
                            }
                            catch { }
                        }
                        else
                        {
                            try
                            {
                                if (value is IEnumerable)
                                {
                                    IEnumerable list = (IEnumerable)value;
                                    int cnt = 0;
                                    foreach (Object item in list)
                                    {
                                        Children.Add(new ObjectNode(string.Format("{0}[{1}]", p.Name, cnt++), item));
                                    }
                                }
                                else
                                {
                                    object v = p.GetValue(value);

                                    if (v != null)
                                    {
                                        Children.Add(new ObjectNode(p.Name, v, p.PropertyType));
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    else if (p.PropertyType.IsValueType && !(value is string))
                    {
                        try
                        {
                            object v = p.GetValue(value, null);

                            if (v != null)
                            {
                                Children.Add(new ObjectNode(p.Name, v, p.PropertyType));
                            }
                        }
                        catch { }
                    }
                    else if (value is IDictionary)
                    {
                        foreach (object key in ((IDictionary)value).Keys)
                        {
                            object val = ((IDictionary)value)[key];
                            Children.Add(new ObjectNode(key.ToString(), val));
                        }
                    }
                }
            }
        }

        #endregion
    }
}
