using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nimaime.Helper.Data
{
	public static class ClassHelper
	{
		/// <summary>
		/// 将DataRow中的数据转为指定类型
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="row">单行数据</param>
		/// <returns>指定类型的对象</returns>
		public static T MapDataRowToClass<T>(DataRow row) where T : new()
		{
			T obj = new T();

			foreach (DataColumn column in row.Table.Columns)
			{
				PropertyInfo property = typeof(T).GetProperty(column.ColumnName);
				if (property != null && row[column] != DBNull.Value)
				{
					property.SetValue(obj, Convert.ChangeType(row[column], property.PropertyType));
				}
			}

			return obj;
		}

		/// <summary>
		/// 将DataTable转为指定类型的序列
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="dataTable">数据表</param>
		/// <returns>指定类型的对象序列</returns>
		public static IEnumerable<T> MapDataTableToClass<T>(DataTable dataTable) where T : new()
		{
			List<T> list = new List<T>();
			foreach (DataRow row in dataTable.Rows)
			{
				list.Add(MapDataRowToClass<T>(row));
			}
			return list.AsEnumerable();
		}

		/// <summary>
		/// 将泛型数据列表转为DataTable
		/// （只支持C#基元类型字段）
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="lstObject">类型列表</param>
		/// <returns>数据表</returns>
		public static DataTable MapClassToDataRow<T>(List<T> lstObject)
		{
			DataTable dataTable = new DataTable();
			Type type = typeof(T);
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			foreach (FieldInfo field in fields)
			{
				if (field.FieldType.IsPrimitive)
				{
					dataTable.Columns.Add(field.Name, field.FieldType);
				}
			}

			foreach (T obj in lstObject)
			{
				DataRow row = dataTable.NewRow();
				foreach (DataColumn column in dataTable.Columns)
				{
					PropertyInfo property = typeof(T).GetProperty(column.ColumnName);
					row[column.ColumnName] = property.GetValue(obj);
				}
				dataTable.Rows.Add(row);
			}

			return dataTable;
		}
	}
}
