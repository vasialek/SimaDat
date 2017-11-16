using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AvUtils
{
    public class Dumper
    {
        private readonly string _collectionLegend = "<COLLECTION>";

        /// <summary>
        /// Dumps all public properties and formats table:  | PropertyName | PropertyValue |
        /// </summary>
        public string Dump(object o, string title = null)
        {
            if (o == null)
            {
                return String.Empty;
            }

            if (o.GetType().IsPrimitive || o.GetType().Equals(typeof(String)))
            {
                return o.ToString();
            }

            StringBuilder sb = new StringBuilder();

            var properties = o.GetType().GetProperties();
            string[] names = properties.Select(x => x.Name).ToArray();
            int maxNameLen = names.Select(x => x.Length).Max() + 1;

            // Do not count length of collections type name, i.e. System.Collections.Generic.List`1[VRAB.RS.DW.CoopPromotionExport.Definition.Models.DiscountCombinationModel
            int maxValueLen = properties
                .Select(x => IsCollection(x) ? _collectionLegend.Length + 8 : (x?.GetValue(o)?.ToString()?.Length ?? 0))
                .Max() + 1;

            int titleLen = title?.Length + 4 ?? 0;
            if (titleLen > maxValueLen)
            {
                maxValueLen = titleLen;
            }

            sb.AppendLine();

            // Title if any
            if (String.IsNullOrEmpty(title) == false)
            {
                sb.AppendLine("-".PadRight(maxNameLen + maxValueLen + 6, '-'));
                sb.Append("| ").Append(title.PadRight(maxNameLen + maxValueLen + 2, ' ')).Append(" |").AppendLine(); 
            }

            sb.AppendLine("-".PadRight(maxNameLen + maxValueLen + 6, '-'));
            sb.Append("| ");
            sb.Append("Name".PadRight(maxNameLen));
            sb.Append("| Value".PadRight(maxValueLen + 3));
            sb.AppendLine("|");
            sb.AppendLine("-".PadRight(maxNameLen + maxValueLen + 6, '-'));

            foreach (var p in properties)
            {
                object v = p.GetValue(o);
                //sb.Append("Is array: " + IsCollection(p));

                sb.Append("| ");
                sb.Append(p.Name.PadRight(maxNameLen));
                sb.Append("| ");
                // Replace collection/array/list type name, i.e. System.Collections.Generic.List`1[VRAB.RS.DW.CoopPromotionExport.Definition.Models.DiscountCombinationModel
                if (IsCollection(p))
                {
                    var collection = v as ICollection;
                    string collectionLegend = String.Format("{0} ({1})", _collectionLegend, collection == null ? "<NO>" : collection.Count.ToString());
                    sb.Append(collectionLegend.PadLeft(maxValueLen));
                }
                else
                {
                    sb.Append((v ?? "").ToString().PadLeft(maxValueLen));
                }

                sb.AppendLine(" |");
            }

            sb.AppendLine("-".PadRight(maxNameLen + maxValueLen + 6, '-'));

            return sb.ToString();
        }

        /// <summary>
        /// Dumps array of objects to string. Each object is displayed as table
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        public string Dump(object[] ar, string title = null)
        {
            return Dump(ar, title, 100);
        }

        public string Dump(object[] ar, string title, int limit)
        {
            StringBuilder sb = new StringBuilder();

            if (String.IsNullOrEmpty(title) == false)
            {
                sb.Append("*** ").Append(title).AppendLine();
            }

            for (int i = 0; i < ar.Length && i < limit; i++)
            {
                sb.AppendLine(Dump(ar[i]));
            }

            return sb.ToString();
        }

        protected bool IsCollection(PropertyInfo pi)
        {
            // String is also IEnumerable, so skip it
            return typeof(String).Equals(pi.PropertyType) == false 
                && typeof(IEnumerable).IsAssignableFrom(pi.PropertyType);
        }
    }
}
